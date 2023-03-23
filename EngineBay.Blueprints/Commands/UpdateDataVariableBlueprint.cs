namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Core;
    using EngineBay.Persistence;
    using FluentValidation;

    public class UpdateDataVariableBlueprint : ICommandHandler<UpdateParameters<DataVariableBlueprint>, DataVariableBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<DataVariableBlueprint> validator;

        private readonly GetApplicationUser getApplicationUserQuery;

        private readonly ClaimsPrincipal claimsPrincipal;

        public UpdateDataVariableBlueprint(ClaimsPrincipal claimsPrincipal, GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db, IValidator<DataVariableBlueprint> validator)
        {
            this.claimsPrincipal = claimsPrincipal;
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<DataVariableBlueprintDto> Handle(UpdateParameters<DataVariableBlueprint> updateParameters, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(this.claimsPrincipal, cancellation).ConfigureAwait(false);
            if (updateParameters is null)
            {
                throw new ArgumentNullException(nameof(updateParameters));
            }

            var id = updateParameters.Id;
            var updateDataVariableBlueprint = updateParameters.Entity;

            if (updateDataVariableBlueprint is null)
            {
                throw new ArgumentException(nameof(updateDataVariableBlueprint));
            }

            this.validator.ValidateAndThrow(updateDataVariableBlueprint);

            var dataVariableBlueprint = await this.db.DataVariableBlueprints.FindAsync(new object[] { id }, cancellation).ConfigureAwait(false);

            if (dataVariableBlueprint is null)
            {
                throw new ArgumentException(nameof(dataVariableBlueprint));
            }

            dataVariableBlueprint.Name = updateDataVariableBlueprint.Name;
            dataVariableBlueprint.Namespace = updateDataVariableBlueprint.Namespace;
            dataVariableBlueprint.Description = updateDataVariableBlueprint.Description;
            await this.db.SaveChangesAsync(user, cancellation).ConfigureAwait(false);
            return new DataVariableBlueprintDto(dataVariableBlueprint);
        }
    }
}