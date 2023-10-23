namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Core;

    public class BlueprintMetaDataDto : BaseDto
    {
        public BlueprintMetaDataDto(Blueprint blueprint)
            : base(blueprint)
        {
            if (blueprint is null)
            {
                throw new ArgumentNullException(nameof(blueprint));
            }

            this.WorkbookId = blueprint.WorkbookId;
            this.Name = blueprint.Name;
            this.Description = blueprint.Description;
        }

        public Guid? WorkbookId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}