namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using FluentValidation;

    public class CreateWorkbook : ICommandHandler<Workbook, WorkbookDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<Workbook> validator;

        public CreateWorkbook(BlueprintsWriteDbContext db, IValidator<Workbook> validator)
        {
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<WorkbookDto> Handle(Workbook workbook, CancellationToken cancellation)
        {
            this.validator.ValidateAndThrow(workbook);
            await this.db.Workbooks.AddAsync(workbook, cancellation).ConfigureAwait(false);
            await this.db.SaveChangesAsync(cancellation).ConfigureAwait(false);
            return new WorkbookDto(workbook);
        }
    }
}