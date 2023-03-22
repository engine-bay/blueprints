namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using EngineBay.Persistence;
    using FluentValidation;

    public class CreateWorkbook : ICommandHandler<Workbook, ApplicationUser, WorkbookDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<Workbook> validator;

        public CreateWorkbook(BlueprintsWriteDbContext db, IValidator<Workbook> validator)
        {
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<WorkbookDto> Handle(Workbook workbook, ApplicationUser user, CancellationToken cancellation)
        {
            this.validator.ValidateAndThrow(workbook);
            await this.db.Workbooks.AddAsync(workbook, cancellation).ConfigureAwait(false);
            await this.db.SaveChangesAsync(user, cancellation).ConfigureAwait(false);
            return new WorkbookDto(workbook);
        }
    }
}