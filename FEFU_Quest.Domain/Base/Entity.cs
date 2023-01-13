using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEFU_Quest.Domain.Base
{
    public abstract class Entity
    {
        /// <summary>
        /// Уникальный Идентефикатор
        /// </summary>
        [Required]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }


}

public enum week
{
    Mo = 0,
    Tu = 1,
    We = 2,
    Th = 3,
    Fr = 4,
    Sa = 5,
    Su = 6
}