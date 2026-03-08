namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.ModuleLogic.Validator
{
    public class ModuleCreateValidator : AbstractValidator<Model.ModuleCreateModel>
    {
        public ModuleCreateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Module name is required.")
                .Length(2, 100).WithMessage("Module name must be between 2 and 100 characters.");
        }
    }

    public class ModuleUpdateValidator : AbstractValidator<Model.ModuleUpdateModel>
    {
        public ModuleUpdateValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Invalid module id.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Module name is required.")
                .Length(2, 100).WithMessage("Module name must be between 2 and 100 characters.");
        }
    }
}