using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WorkWithDb.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the WorkWithDbUser class
    public class WorkWithDbUser : IdentityUser
    {
        public string DateReg { get; set; }
        public string DateSignUp { get; set; }
        public bool Status { get; set; }
    }
}
