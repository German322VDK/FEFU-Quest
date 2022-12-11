using FEFU_Quest.Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace FEFU_Quest.Domain.Email
{
    public class EmailConfirm : Entity
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Hash { get; set; }
    }
}
