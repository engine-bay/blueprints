namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;
    using FluentValidation;

    public class CreateInputDataVariableBlueprint : ICommandHandler<InputDataVariableBlueprint, InputDataVariableBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<InputDataVariableBlueprint> validator;

        private readonly GetApplicationUser getApplicationUserQuery;

        public CreateInputDataVariableBlueprint(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db, IValidator<InputDataVariableBlueprint> validator)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<InputDataVariableBlueprintDto> Handle(InputDataVariableBlueprint inputDataVariableBlueprint, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation);
            this.validator.ValidateAndThrow(inputDataVariableBlueprint);
            await this.db.InputDataVariableBlueprints.AddAsync(inputDataVariableBlueprint, cancellation);
            await this.db.SaveChangesAsync(user, cancellation);
            return new InputDataVariableBlueprintDto(inputDataVariableBlueprint);
        }
    }
}