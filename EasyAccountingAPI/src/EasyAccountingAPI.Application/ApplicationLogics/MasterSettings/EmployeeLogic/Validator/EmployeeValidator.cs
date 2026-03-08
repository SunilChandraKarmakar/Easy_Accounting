namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.EmployeeLogic.Validator
{
    public class EmployeeCreateValidator : AbstractValidator<Model.EmployeeCreateModel>
    {
        public EmployeeCreateValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Please, provide full name.")
                .Length(2, 100).WithMessage("Full name must be between 2 and 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Please, provide email address.")
                .EmailAddress().WithMessage("Please provide a valid email address.")
                .Length(10, 50).WithMessage("Email must be between 10 and 50 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Please, provide password.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

            RuleFor(x => x.CompanyId)
                .GreaterThan(0).When(x => x.CompanyId.HasValue).WithMessage("Company id must be greater than 0.");

            RuleFor(x => x.Phone)
                .MaximumLength(30).WithMessage("Phone cannot exceed 30 characters.");
        }
    }

    public class EmployeeUpdateValidator : AbstractValidator<Model.EmployeeUpdateModel>
    {
        public EmployeeUpdateValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Invalid employee id.");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Please, provide full name.")
                .Length(2, 100).WithMessage("Full name must be between 2 and 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Please, provide email address.")
                .EmailAddress().WithMessage("Please provide a valid email address.")
                .Length(10, 50).WithMessage("Email must be between 10 and 50 characters.");

            RuleFor(x => x.Phone)
                .MaximumLength(30).WithMessage("Phone cannot exceed 30 characters.");

            RuleFor(x => x.CompanyId)
                .GreaterThan(0).When(x => x.CompanyId.HasValue).WithMessage("Company id must be greater than 0.");
        }
    }
}