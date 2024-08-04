using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skps_services.Models
{
    public class UserNew
    {
        public string LocalId { get; set; }
        public string FederatedId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public bool EmailVerified { get; set; }
        public string PhotoUrl { get; set; }
        public string MobileNumber { get; set; }
    }
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Uid { get; set; }
    }

    public class UserA
    {
        public Dictionary<string, User> UserAs { get; set; }
    }
}
