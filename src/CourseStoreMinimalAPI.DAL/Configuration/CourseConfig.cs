using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseStoreMinimalAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseStoreMinimalAPI.DAL.Configuration
{
    internal class CourseConfig : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.Property(c => c.ImageUrl).HasMaxLength(200).IsRequired().IsUnicode(false);
            builder.Property(c => c.Description).HasMaxLength(1000);
            builder.Property(c => c.Title).HasMaxLength(200);
            builder.HasMany(c => c.Comments).WithOne().HasForeignKey(c => c.CourseId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
