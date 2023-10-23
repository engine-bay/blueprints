namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Core;

    public class WorkbookMetaDataDto : BaseDto
    {
        public WorkbookMetaDataDto(Workbook workbook)
            : base(workbook)
        {
            if (workbook is null)
            {
                throw new ArgumentNullException(nameof(workbook));
            }

            this.Name = workbook.Name;
            this.Description = workbook.Description;
        }

        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}