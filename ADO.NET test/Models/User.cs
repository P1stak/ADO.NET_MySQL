using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NET_test.Models
{
    public class User
    {
        public string FullName { get; set; }
        public string? Details { get; set; }
        public DateTime JoinDate { get; set; } = DateTime.Now;
        public string? Avatar { get; set; }
        public bool IsActive { get; set; } = true;


        public int Knowledge { get; set; }
        public int Reputation { get; set; }
        public int FollowersCount { get; set; }
    }
}
