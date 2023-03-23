namespace EngineBay.Blueprints.Tests
{
    using EngineBay.Persistence;

    public class MockApplicationUser : ApplicationUser
    {
        public MockApplicationUser()
        {
            this.Name = "mockUser";
            this.CreatedById = default(Guid);
            this.LastUpdatedById = default(Guid);
        }
    }
}
