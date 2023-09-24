namespace EngineBay.Blueprints
{
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class GetWorkbookMetaData : IQueryHandler<Guid, WorkbookMetaDataDto>
    {
        private readonly BlueprintsQueryDbContext db;

        public GetWorkbookMetaData(BlueprintsQueryDbContext db)
        {
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<WorkbookMetaDataDto> Handle(Guid id, CancellationToken cancellation)
        {
            return await this.db.Workbooks
                .Where(workbook => workbook.Id == id)
                .Select(workbook => new WorkbookMetaDataDto(workbook))
                .AsExpandable()
                .FirstAsync(cancellation)
                .ConfigureAwait(false);
        }
    }
}