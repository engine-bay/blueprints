namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using FluentValidation;

    public class UpdateWorkbook : ICommandHandler<UpdateParameters<Workbook>, WorkbookDto>
    {
        private readonly BlueprintsEngineWriteDb db;
        private readonly IValidator<Workbook> validator;

        public UpdateWorkbook(BlueprintsEngineWriteDb db, IValidator<Workbook> validator)
        {
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<WorkbookDto> Handle(UpdateParameters<Workbook> updateParameters, CancellationToken cancellation)
        {
            if (updateParameters is null)
            {
                throw new ArgumentNullException(nameof(updateParameters));
            }

            var id = updateParameters.Id;
            var updateWorkbook = updateParameters.Entity;

            if (updateWorkbook is null)
            {
                throw new ArgumentException(nameof(updateWorkbook));
            }

            this.validator.ValidateAndThrow(updateWorkbook);

            var workbook = await this.db.Workbooks.FindAsync(new object[] { id }, cancellation).ConfigureAwait(false);

            if (workbook is null)
            {
                throw new ArgumentException(nameof(workbook));
            }

            workbook.Name = updateWorkbook.Name;
            workbook.Description = updateWorkbook.Description;
            await this.db.SaveChangesAsync(cancellation).ConfigureAwait(false);
            return new WorkbookDto(workbook);
        }
    }
}