namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;
    using FluentValidation;

    public class UpdateDataVariableBlueprint : ICommandHandler<UpdateParameters<DataVariableBlueprint>, DataVariableBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<DataVariableBlueprint> validator;

        private readonly GetApplicationUser getApplicationUserQuery;

        public UpdateDataVariableBlueprint(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db, IValidator<DataVariableBlueprint> validator)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<DataVariableBlueprintDto> Handle(UpdateParameters<DataVariableBlueprint> updateParameters, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation);
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

            var dataVariableBlueprint = await this.db.DataVariableBlueprints.FindAsync(new object[] { id }, cancellation);

            if (dataVariableBlueprint is null)
            {
                throw new ArgumentException(nameof(dataVariableBlueprint));
            }

            dataVariableBlueprint.Name = updateDataVariableBlueprint.Name;
            dataVariableBlueprint.Namespace = updateDataVariableBlueprint.Namespace;
            dataVariableBlueprint.Description = updateDataVariableBlueprint.Description;
            await this.db.SaveChangesAsync(user, cancellation);
            return new DataVariableBlueprintDto(dataVariableBlueprint);
        }
    }
}