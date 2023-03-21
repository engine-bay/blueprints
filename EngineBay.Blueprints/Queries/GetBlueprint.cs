namespace EngineBay.Blueprints
{
    using EngineBay.Core;

    public class GetBlueprint : IQueryHandler<Guid, BlueprintDto>
    {
        private readonly BlueprintsQueryDbContext db;

        public GetBlueprint(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<BlueprintDto> Handle(Guid id, CancellationToken cancellation)
        {
            var blueprint = await this.db.Blueprints.FindAsync(new object[] { id }, cancellation).ConfigureAwait(false);

            if (blueprint is null)
            {
                throw new ArgumentException(nameof(blueprint));
            }

            return new BlueprintDto(blueprint);
        }
    }
}