namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;
    using FluentValidation;

    public class UpdateInputDataVariableBlueprint : ICommandHandler<UpdateParameters<InputDataVariableBlueprint>, InputDataVariableBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<InputDataVariableBlueprint> validator;

        private readonly GetApplicationUser getApplicationUserQuery;

        public UpdateInputDataVariableBlueprint(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db, IValidator<InputDataVariableBlueprint> validator)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<InputDataVariableBlueprintDto> Handle(UpdateParameters<InputDataVariableBlueprint> updateParameters, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation).ConfigureAwait(false);
            if (updateParameters is null)
            {
                throw new ArgumentNullException(nameof(updateParameters));
            }

            var id = updateParameters.Id;
            var updateInputDataVariableBlueprint = updateParameters.Entity;

            if (updateInputDataVariableBlueprint is null)
            {
                throw new ArgumentException(nameof(updateInputDataVariableBlueprint));
            }

            this.validator.ValidateAndThrow(updateInputDataVariableBlueprint);

            var inputDataVariableBlueprint = await this.db.InputDataVariableBlueprints.FindAsync(new object[] { id }, cancellation).ConfigureAwait(false);

            if (inputDataVariableBlueprint is null)
            {
                throw new ArgumentException(nameof(inputDataVariableBlueprint));
            }

            inputDataVariableBlueprint.Name = updateInputDataVariableBlueprint.Name;
            inputDataVariableBlueprint.Namespace = updateInputDataVariableBlueprint.Namespace;
            inputDataVariableBlueprint.Type = updateInputDataVariableBlueprint.Type;
            await this.db.SaveChangesAsync(user, cancellation).ConfigureAwait(false);
            return new InputDataVariableBlueprintDto(inputDataVariableBlueprint);
        }
    }
}