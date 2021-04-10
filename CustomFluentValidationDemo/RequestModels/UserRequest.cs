using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomFluentValidationDemo
{
    /// <summary>
    /// 用户入参请求
    /// </summary>
    public class UserRequest
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
    }
}
