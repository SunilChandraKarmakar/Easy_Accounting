namespace EasyAccountingAPI.Application.ApplicationLogics.MasterSettings.AccessControl.ActionLogic.Validator
{
    public class ActionCreateValidator : AbstractValidator<Model.ActionCreateModel>
    {
        public ActionCreateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(2, 50).WithMessage("Name must be between 2 and 50 characters.");
        }
    }

    public class ActionUpdateValidator : AbstractValidator<Model.ActionUpdateModel>
    {
        public ActionUpdateValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Invalid action id.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(2, 50).WithMessage("Name must be between 2 and 50 characters.");
        }
    }
}