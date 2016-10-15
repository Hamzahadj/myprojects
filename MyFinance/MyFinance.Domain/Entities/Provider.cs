using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFinance.Domain.Entities
{
   public class Provider
    {
       [Key]
        public int Id { get; set; }
       public string UserName { get; set; }
       [DataType(DataType.Password)]
       [MinLength(8)]
       [Required]
       public string Password { get; set; }
       [DataType(DataType.Password)]
       [NotMapped,Required]
       [Compare("Password")]
       public string ConfirmPassword { get; set; } 
       [EmailAddress,Required]
       public string Email { get; set; }
       public bool IsApproved { get; set; }
       public DateTime? DateCreated { get; set; } // ?  nullable
       //navigation properties
       virtual public ICollection<Product> Products { get; set; }
    }
}
