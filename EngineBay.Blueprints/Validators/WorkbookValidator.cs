namespace EngineBay.Blueprints
{
    using FluentValidation;

    public class WorkbookValidator : AbstractValidator<Workbook>
    {
        public WorkbookValidator()
        {
            this.RuleFor(workbook => workbook.Name).NotNull().NotEmpty();
            this.RuleFor(workbook => workbook.Description);
            this.RuleForEach(workbook => workbook.Blueprints).SetValidator(new BlueprintValidator());
        }
    }
}