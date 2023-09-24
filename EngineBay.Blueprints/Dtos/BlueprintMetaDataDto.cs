namespace EngineBay.Blueprints
{
    using System;

    public class BlueprintMetaDataDto
    {
        public BlueprintMetaDataDto(Blueprint blueprint)
        {
            if (blueprint is null)
            {
                throw new ArgumentNullException(nameof(blueprint));
            }

            this.Id = blueprint.Id;
            this.WorkbookId = blueprint.WorkbookId;
            this.Name = blueprint.Name;
            this.Description = blueprint.Description;
        }

        public Guid Id { get; set; }

        public Guid? WorkbookId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}