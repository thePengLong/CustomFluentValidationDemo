using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomFluentValidationDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("Get")]
        public ActionResult GetUser([FromBody] UserRequest input)
        {
            var userValidator = new UserValidator();

            var validResult = userValidator.Validate(input);

            if(!validResult.IsValid)
            {
                return new JsonResult(new { Code = 500, Msg = validResult.Errors[0].ErrorMessage });
            }

            return new JsonResult("已获取用户信息");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("Update"), ParamValidate("Update")]
        public ActionResult UpdateUser([FromBody] UserRequest input)
        {
            return new JsonResult("已更新用户信息");
        }

        [HttpPost("Delete"), ParamValidate()]
        public ActionResult DeleteUser([FromBody] UserRequest input)
        {
            return new JsonResult("已删除用户信息");
        }
    }
}
