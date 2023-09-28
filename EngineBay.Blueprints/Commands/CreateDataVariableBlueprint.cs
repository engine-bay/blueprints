namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;
    using FluentValidation;

    public class CreateDataVariableBlueprint : ICommandHandler<DataVariableBlueprint, DataVariableBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<DataVariableBlueprint> validator;

        private readonly GetApplicationUser getApplicationUserQuery;

        public CreateDataVariableBlueprint(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db, IValidator<DataVariableBlueprint> validator)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<DataVariableBlueprintDto> Handle(DataVariableBlueprint dataVariableBlueprint, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation);
            this.validator.ValidateAndThrow(dataVariableBlueprint);
            await this.db.DataVariableBlueprints.AddAsync(dataVariableBlueprint, cancellation);
            await this.db.SaveChangesAsync(user, cancellation);
            return new DataVariableBlueprintDto(dataVariableBlueprint);
        }
    }
}