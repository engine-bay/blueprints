namespace EngineBay.Blueprints
{
    using System;

    public class WorkbookDto : WorkbookMetaDataDto
    {
        public WorkbookDto(Workbook workbook)
            : base(workbook)
        {
            ArgumentNullException.ThrowIfNull(workbook);

            this.Blueprints = workbook.Blueprints?.Select(x => new BlueprintDto(x)).ToList();
        }

        public IEnumerable<BlueprintDto>? Blueprints { get; set; }
    }
}