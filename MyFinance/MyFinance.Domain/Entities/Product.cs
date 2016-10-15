using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFinance.Domain.Entities
{
   public class Product
    {
       [Display(Name="Production Date")]
       [DataType(DataType.DateTime)]
       public DateTime DateProd { get; set; }
       [DataType(DataType.MultilineText)]
       public string Description { get; set; }
       [Required(ErrorMessage="Le champ name est obligatoire")]
       [MaxLength(50,ErrorMessage="Taille max = 50")]
       [StringLength(25, ErrorMessage = "Taille max = 25")]
       public string Name { get; set; }
       [DataType(DataType.Currency)]
       public double Price { get; set; }
       public int ProductId { get; set; }
       [Range(0,double.MaxValue)]
       public int Quantity { get; set; }
       [ForeignKey("CategoryId")]
       public virtual Category category { get; set; }
       public virtual ICollection<Provider> providers { get; set; }
       public int? CategoryId { get; set; }
       public string ImageName { get; set; }
    }
}
