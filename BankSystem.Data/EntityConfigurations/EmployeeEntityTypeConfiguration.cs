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
    public class EmployeeEntityTypeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("employees");

            builder.Property(e => e.Id);
            builder.Property(e => e.Name).HasMaxLength(100).IsRequired();
            builder.Property(e => e.Surname).HasMaxLength(100).IsRequired();
            builder.Property(e => e.PhoneNumber).HasMaxLength(15).IsRequired();
            builder.HasIndex(e => e.PhoneNumber).IsUnique();
            builder.Property(e => e.Passport).HasMaxLength(50).IsRequired();
            builder.HasIndex(e => e.Passport).IsUnique();
            builder.Property(e => e.Address).HasMaxLength(100).IsRequired();
            builder.Property(e => e.Date).IsRequired();
            builder.Property(e => e.Bonus);

            builder.Property(e => e.Position).HasMaxLength(50).IsRequired();
            builder.Property(e => e.Salary).IsRequired();
            builder.Property(e => e.DateStartWork).IsRequired();
            builder.Property(e => e.Contract).HasMaxLength(200).IsRequired();

            builder.HasKey(e => e.Id);
        }
    }
}
