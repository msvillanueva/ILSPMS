using ILSPMS.Web.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ILSPMS.Web.Infrastructure.Validators
{
    public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidator()
        {
            RuleFor(s => s.Username).NotEmpty().WithMessage("Invalid Username");
            RuleFor(s => s.Password).NotEmpty().WithMessage("Invalid Password");
        }
    }
}