using MyFinance.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFinance.Data.Configurations
{
    class AddressConfiguration : ComplexTypeConfiguration<Address>
    {
        public AddressConfiguration()
        {
            Property(p => p.City).IsRequired();
            Property(p => p.StreetAddress).HasMaxLength(50)
            .IsOptional();
        }
    }
}
