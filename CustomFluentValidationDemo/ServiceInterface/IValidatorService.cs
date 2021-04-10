using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomFluentValidationDemo
{
    public interface IValidatorService
    {
        /// <summary>
        /// 默认校验
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        bool Valid<T>(T value, out string message);
        /// <summary>
        /// 按规则校验
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="rule"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        bool Valid<T>(T value, string rule, out string message);
    }
}
