namespace EngineBay.Blueprints
{
    using System;
    using EngineBay.Core;

    public class BlueprintMetaDataDto : BaseDto
    {
        public BlueprintMetaDataDto(Blueprint blueprint)
            : base(blueprint)
        {
            ArgumentNullException.ThrowIfNull(blueprint);

            this.WorkbookId = blueprint.WorkbookId;
            this.Name = blueprint.Name;
            this.Description = blueprint.Description;
        }

        public Guid? WorkbookId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}