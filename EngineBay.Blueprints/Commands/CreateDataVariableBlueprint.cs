namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Core;
    using EngineBay.Persistence;
    using FluentValidation;

    public class CreateDataVariableBlueprint : ICommandHandler<DataVariableBlueprint, DataVariableBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<DataVariableBlueprint> validator;

        private readonly GetApplicationUser getApplicationUserQuery;

        private readonly ClaimsPrincipal claimsPrincipal;

        public CreateDataVariableBlueprint(ClaimsPrincipal claimsPrincipal, GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db, IValidator<DataVariableBlueprint> validator)
        {
            this.claimsPrincipal = claimsPrincipal;
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<DataVariableBlueprintDto> Handle(DataVariableBlueprint dataVariableBlueprint, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(this.claimsPrincipal, cancellation).ConfigureAwait(false);
            this.validator.ValidateAndThrow(dataVariableBlueprint);
            await this.db.DataVariableBlueprints.AddAsync(dataVariableBlueprint, cancellation).ConfigureAwait(false);
            await this.db.SaveChangesAsync(user, cancellation).ConfigureAwait(false);
            return new DataVariableBlueprintDto(dataVariableBlueprint);
        }
    }
}