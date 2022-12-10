using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEFU_Quest.Domain.Identity
{
    public  class UserDTO : IdentityUser
    {
        public string SecondName { get; set; }

        public string FirstName { get; set; }

        public string Patronymic { get; set; }

        public string Status { get; set; }

        public byte[] Photo { get; set; }

        public virtual UniverGroup UniverGroup { get; set; }

        public string Dormitory { get; set; }
    }
}
