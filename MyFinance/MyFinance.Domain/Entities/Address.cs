using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFinance.Domain.Entities
{
   [ComplexType]
   public class Address
    {
        public string City { get; set; }
        public string StreetAddress { get; set; }
    }
}
