namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;
    using FluentValidation;

    public class UpdateOutputDataVariableBlueprint : ICommandHandler<UpdateParameters<OutputDataVariableBlueprint>, OutputDataVariableBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<OutputDataVariableBlueprint> validator;

        private readonly GetApplicationUser getApplicationUserQuery;

        public UpdateOutputDataVariableBlueprint(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db, IValidator<OutputDataVariableBlueprint> validator)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<OutputDataVariableBlueprintDto> Handle(UpdateParameters<OutputDataVariableBlueprint> updateParameters, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation);
            if (updateParameters is null)
            {
                throw new ArgumentNullException(nameof(updateParameters));
            }

            var id = updateParameters.Id;
            var updateOutputDataVariableBlueprint = updateParameters.Entity;

            if (updateOutputDataVariableBlueprint is null)
            {
                throw new ArgumentException(nameof(updateOutputDataVariableBlueprint));
            }

            this.validator.ValidateAndThrow(updateOutputDataVariableBlueprint);

            var outputDataVariableBlueprint = await this.db.OutputDataVariableBlueprints.FindAsync(new object[] { id }, cancellation);

            if (outputDataVariableBlueprint is null)
            {
                throw new ArgumentException(nameof(outputDataVariableBlueprint));
            }

            outputDataVariableBlueprint.Name = updateOutputDataVariableBlueprint.Name;
            outputDataVariableBlueprint.Namespace = updateOutputDataVariableBlueprint.Namespace;
            outputDataVariableBlueprint.Type = updateOutputDataVariableBlueprint.Type;
            await this.db.SaveChangesAsync(user, cancellation);
            return new OutputDataVariableBlueprintDto(outputDataVariableBlueprint);
        }
    }
}