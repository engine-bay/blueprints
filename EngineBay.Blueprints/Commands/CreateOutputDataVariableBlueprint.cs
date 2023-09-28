namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;
    using FluentValidation;

    public class CreateOutputDataVariableBlueprint : ICommandHandler<OutputDataVariableBlueprint, OutputDataVariableBlueprintDto>
    {
        private readonly BlueprintsWriteDbContext db;
        private readonly IValidator<OutputDataVariableBlueprint> validator;

        private readonly GetApplicationUser getApplicationUserQuery;

        public CreateOutputDataVariableBlueprint(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db, IValidator<OutputDataVariableBlueprint> validator)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
            this.validator = validator;
        }

        /// <inheritdoc/>
        public async Task<OutputDataVariableBlueprintDto> Handle(OutputDataVariableBlueprint outputDataVariableBlueprint, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation);
            this.validator.ValidateAndThrow(outputDataVariableBlueprint);
            await this.db.OutputDataVariableBlueprints.AddAsync(outputDataVariableBlueprint, cancellation);
            await this.db.SaveChangesAsync(user, cancellation);
            return new OutputDataVariableBlueprintDto(outputDataVariableBlueprint);
        }
    }
}