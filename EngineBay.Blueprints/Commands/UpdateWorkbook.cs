namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;
    using EngineBay.Persistence;
    using FluentValidation;

    public class UpdateWorkbook : ICommandHandler<UpdateParameters<Workbook>, WorkbookDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<Workbook> validator;

        private readonly GetApplicationUser getApplicationUserQuery;

        public UpdateWorkbook(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db, IValidator<Workbook> validator)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<WorkbookDto> Handle(UpdateParameters<Workbook> updateParameters, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation);
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

            var workbook = await this.db.Workbooks.FindAsync(new object[] { id }, cancellation);

            if (workbook is null)
            {
                throw new ArgumentException(nameof(workbook));
            }

            workbook.Name = updateWorkbook.Name;
            workbook.Description = updateWorkbook.Description;
            await this.db.SaveChangesAsync(user, cancellation);
            return new WorkbookDto(workbook);
        }
    }
}