using System.Runtime.Serialization;

namespace BerryServer.Route.Api.User.Entities
{
    [DataContract]
    public class UserAccount
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        public int DeptID { get; set; }
        public int RankID { get; set; }
        public int RoleID { get; set; }
        public int Status { get; set; }
        public string Name { get; set; }
        public string AccountId { get; set; }
        public string AccountPw { get; set; }
    }
}
