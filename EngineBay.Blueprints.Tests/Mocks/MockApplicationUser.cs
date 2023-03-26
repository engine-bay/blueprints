namespace EngineBay.Blueprints.Tests
{
    using EngineBay.Persistence;

    public class MockApplicationUser : ApplicationUser
    {
        public MockApplicationUser()
        {
            this.Username = Guid.NewGuid().ToString();
            this.CreatedById = default(Guid);
            this.LastUpdatedById = default(Guid);
        }
    }
}
