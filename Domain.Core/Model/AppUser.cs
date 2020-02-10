using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core.Model
{
    public class AppUser : IdentityUser
	{
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Dashboard> Dashboards { get; set; }
    }
}
