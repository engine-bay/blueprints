namespace EngineBay.Blueprints
{
    using System;

    public class WorkbookDto : WorkbookMetaDataDto
    {
        public WorkbookDto(Workbook workbook)
            : base(workbook)
        {
            if (workbook is null)
            {
                throw new ArgumentNullException(nameof(workbook));
            }

            this.Blueprints = workbook.Blueprints?.Select(x => new BlueprintDto(x)).ToList();
        }

        public IEnumerable<BlueprintDto>? Blueprints { get; set; }
    }
}