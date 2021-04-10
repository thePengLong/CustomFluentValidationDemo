using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace CustomFluentValidationDemo
{
    public class UserValidator : AbstractValidator<UserRequest>
    {
        public UserValidator()
        {
            //遇到第一个失败即停止
            CascadeMode = CascadeMode.Stop;
            //校验姓名不能为空
            RuleFor(i => i.Name).NotEmpty().WithMessage("姓名不能为空");

            RuleSet("Update", () =>
            {
                RuleFor(i => i.Name).NotEmpty().WithMessage("姓名不能为空");
                RuleFor(i => i.Sex).NotEmpty().WithMessage("性别不能为空");
            });
        }
    }
}
