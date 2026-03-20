namespace BerryServer.Route.Api.User.Entities
{
    public class UserRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool CanView { get; set; }
        public bool CanCreate { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        public bool CanControl { get; set; }
    }
}
