namespace EngineBay.Blueprints
{
    using System.Security.Claims;
    using EngineBay.Authentication;
    using EngineBay.Core;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;

    public class DeleteWorkbook : ICommandHandler<Guid, WorkbookDto>
    {
        private readonly BlueprintsWriteDbContext db;

        private readonly GetApplicationUser getApplicationUserQuery;

        public DeleteWorkbook(GetApplicationUser getApplicationUserQuery, BlueprintsWriteDbContext db)
        {
            this.getApplicationUserQuery = getApplicationUserQuery;
            this.db = db;
        }

        /// <inheritdoc/>
        public async Task<WorkbookDto> Handle(Guid id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation)
        {
            var user = await this.getApplicationUserQuery.Handle(claimsPrincipal, cancellation);
            var workbook = await this.db.Workbooks
                .Include(x => x.Blueprints)
                        .ThenInclude(blueprint => blueprint.ExpressionBlueprints)
                            .ThenInclude(expressionBlueprint => expressionBlueprint.InputDataTableBlueprints)
                .Include(x => x.Blueprints)
                    .ThenInclude(x => x.ExpressionBlueprints)
                        .ThenInclude(x => x.InputDataVariableBlueprints)
                .Include(x => x.Blueprints)
                    .ThenInclude(x => x.ExpressionBlueprints)
                        .ThenInclude(x => x.OutputDataVariableBlueprint)
                .Include(x => x.Blueprints)
                    .ThenInclude(x => x.DataVariableBlueprints)
                .Include(x => x.Blueprints)
                    .ThenInclude(x => x.TriggerBlueprints)
                        .ThenInclude(x => x.TriggerExpressionBlueprints)
                            .ThenInclude(x => x.InputDataVariableBlueprint)
                .Include(x => x.Blueprints)
                    .ThenInclude(x => x.TriggerBlueprints)
                        .ThenInclude(x => x.OutputDataVariableBlueprint)
                .Include(x => x.Blueprints)
                        .ThenInclude(x => x.DataTableBlueprints)
                            .ThenInclude(x => x.DataTableColumnBlueprints)
                    .Include(x => x.Blueprints)
                        .ThenInclude(x => x.DataTableBlueprints)
                            .ThenInclude(x => x.DataTableRowBlueprints)
                                .ThenInclude(x => x.DataTableCellBlueprints)
                .Where(workbook => workbook.Id == id)
                .AsExpandable()
                .FirstAsync(cancellation)
                ;

            if (workbook is null)
            {
                throw new ArgumentException(nameof(workbook));
            }

            this.db.Workbooks.Remove(workbook);
            await this.db.SaveChangesAsync(user, cancellation);
            return new WorkbookDto(workbook);
        }
    }
}