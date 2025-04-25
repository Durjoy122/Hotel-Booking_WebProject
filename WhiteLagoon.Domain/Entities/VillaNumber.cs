using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteLagoon.Domain.Entities
{
    public class VillaNumber
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Villa Number")]
        [Required(ErrorMessage = "Villa Number is required")]
        public int Villa_Number { get; set; }

        [ForeignKey("Villa")]
        [Required(ErrorMessage = "Villa is required")]
        public int VillaId { get; set; }
        public Villa Villa { get; set; }

        public string? SpecialDetails { get; set; }
    }
}
