using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomFluentValidationDemo
{
    /// <summary>
    /// 参数校验
    /// </summary>
    public class ParamValidateAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 规则名
        /// </summary>
        private readonly string _ruleName;
        public ParamValidateAttribute(string ruleName = null)
        {
            _ruleName = ruleName;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var validatorService = context.HttpContext.RequestServices.GetService(typeof(IValidatorService)) as IValidatorService;
            string message = string.Empty;

            //依次对参数进行校验
            foreach (var argument in context.ActionArguments)
            {
                if (!validatorService.Valid(argument.Value, _ruleName, out message))
                {
                    //可根据项目结构自行定义返参
                    var result = new
                    {
                        Code = 500,
                        Msg = message
                    };
                    context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(result);
                    break;
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
