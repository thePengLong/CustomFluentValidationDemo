using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CustomFluentValidationDemo
{
    public sealed class ValidatorService : IValidatorService
    {
        private IDictionary<string, Func<IValidationContext, ValidationResult>> _validaBehaviorSet;
       
        private IDictionary<string, Func<IValidationContext, ValidationResult>> _validaRuleBehaviorSet;

        private IDictionary<Type, IValidator> _validatorSet;

        public ValidatorService()
        {
            _validaBehaviorSet = new ConcurrentDictionary<string, Func<IValidationContext, ValidationResult>>();
            _validaRuleBehaviorSet = new ConcurrentDictionary<string, Func<IValidationContext, ValidationResult>>();
            _validatorSet = new ConcurrentDictionary<Type, IValidator>();
            RegisterValidator();
        }

        /// <summary>
        /// 自动注册服务
        /// </summary>
        private void RegisterValidator()
        {
            var assemblyConfig = new List<Assembly>();

            //集中放置校验类时
            assemblyConfig.Add(Assembly.GetAssembly(typeof(UserValidator)));

            ////分开放置校验类时
            //foreach (string filePath in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "CustomFluentValidationDemo.dll"))
            //{
            //    assemblyConfig.Add(Assembly.LoadFrom(filePath));
            //}

            foreach (var assembly in assemblyConfig)
            {
                foreach (var i in assembly.GetTypes())
                {
                    if (i.IsInterface) continue;

                    foreach (var type in i.GetInterfaces())
                    {
                        if (type.Name == "IValidator`1")
                        {
                            _validatorSet[type.GenericTypeArguments[0]] = (IValidator)Activator.CreateInstance(i);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool Valid<T>(T value, out string message)
        {
            message = string.Empty;

            Type type = value.GetType();

            string typeName = type.ToString();

            if (!_validatorSet.ContainsKey(type))
            {
                message = $"未找到{value}相对应的验证类";
                return false;
            }

            //验证途中如果没有 则新加入验证方法
            if (!_validaBehaviorSet.ContainsKey(typeName))
            {
                _validaBehaviorSet.TryAdd(typeName, _validatorSet[type].Validate);
            }

            var context = new ValidationContext<T>(value);

            ValidationResult result = _validaBehaviorSet[typeName](context);

            if (result.IsValid) return true;

            message = result.Errors?[0].ErrorMessage;

            return false;
        }

        /// <summary>
        /// 根据规则名来验证
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="rule"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool Valid<T>(T value, string rule, out string message)
        {
            message = string.Empty;

            if (string.IsNullOrEmpty(rule)) return Valid(value, out message);

            Type type = value.GetType();

            string typeName = type.ToString();

            if (!_validatorSet.ContainsKey(type))
            {
                message = $"未找到{value}相对应的验证类";
                return false;
            }

            IEnumerable<string> ruleSetNames = from x in rule.Split(',', ';') select x.Trim();

            IValidatorSelector selector = ValidatorOptions.Global.ValidatorSelectors.RulesetValidatorSelectorFactory(ruleSetNames.ToArray());

            var context = new ValidationContext<T>(value, new PropertyChain(), selector);

            //验证途中如果没有 则新加入验证方法
            if (!_validaRuleBehaviorSet.ContainsKey(typeName))
            {
                _validaRuleBehaviorSet[typeName] = _validatorSet[type].Validate;
            }

            ValidationResult result = _validaRuleBehaviorSet[typeName](context);

            if (result.IsValid) return true;

            message = result.Errors?[0].ErrorMessage;

            return false;
        }
    }
}
