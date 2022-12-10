using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEFU_Quest.Domain.Identity
{
    public class RoleDTO : IdentityRole
    {
        public UserStatus Status { get; set; }

        public string Description { get; set; }
    }
}
