using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Data.EntityConfigurations
{
    internal class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("accounts");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id);
            builder.Property(a => a.CurrencyName).IsRequired();
            builder.Property(e => e.Amount).HasDefaultValue(0).IsRequired();
            builder.Property(a => a.ClientId).IsRequired();

            builder.HasOne(a => a.Client) // Каждому счету соответствует один клиент
               .WithMany(c => c.Accounts) // У клиента может быть много счетов
               .HasForeignKey(cl => cl.ClientId); // Внешний ключ

        }
    }
}
