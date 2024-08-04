using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skps_services.Models
{
    public class FirebaseAuthLink
    {
        public string IdToken { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
        public string Created { get; set; }
        public User User { get; set; }
    }
}
