using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ToDo.Data
{
    public class ToDoItemMap : IEntityTypeConfiguration<TodoItem>
    {
        public void Configure(EntityTypeBuilder<TodoItem> entityBuilder)
        {
            entityBuilder.HasKey(i => i.Id); ;
            entityBuilder.Property(i => i.Id).ValueGeneratedOnAdd();
            entityBuilder.Property(i => i.Description).IsRequired();
            entityBuilder.Property(i => i.CreatedDate).IsRequired();
            entityBuilder.Property(i => i.UpdatedDate);
            entityBuilder.Property(i => i.PlannedDate);
            entityBuilder.Property(i => i.CompletedDate).IsRequired();
            entityBuilder.Property(i => i.Priority).HasConversion<int>();
            entityBuilder.Property(i => i.Status).HasConversion<int>();
        }
    }
}
