namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;
    using FluentValidation;

    public class CreateWorkbook : ICommandHandler<Workbook, WorkbookDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<Workbook> validator;

        private readonly GetApplicationUser getApplicationUserQuery;

        public CreateWorkbook(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db, IValidator<Workbook> validator)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<WorkbookDto> Handle(Workbook workbook, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation);
            this.validator.ValidateAndThrow(workbook);
            await this.db.Workbooks.AddAsync(workbook, cancellation);
            await this.db.SaveChangesAsync(user, cancellation);
            return new WorkbookDto(workbook);
        }
    }
}