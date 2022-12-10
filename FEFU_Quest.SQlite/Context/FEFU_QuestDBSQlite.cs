using FEFU_Quest.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FEFU_Quest.SQlite.Context
{
    public class FEFU_QuestDBSQlite : IdentityDbContext<UserDTO, RoleDTO, string>
    {
        public DbSet<UserDTO> Users { get; set; }

        public DbSet<UniverGroup> UniverGroups { get; set; }

        public DbSet<RoleDTO> Roles { get; set; }

        public FEFU_QuestDBSQlite(DbContextOptions<FEFU_QuestDBSQlite> options) : base(options) { }
    }
}
