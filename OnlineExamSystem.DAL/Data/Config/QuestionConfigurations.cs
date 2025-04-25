using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineExamSystem.Common.Models;

namespace OnlineExamSystem.DAL.Data.Config
{
    public class QuestionConfigurations : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasOne(q => q.Exam)
                   .WithMany(e => e.Questions)
                   .HasForeignKey(q => q.ExamId)
                   .OnDelete(DeleteBehavior.Cascade); 

            builder.Property(q => q.Title)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(q => q.Option1)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(q => q.Option2)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(q => q.Option3)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(q => q.Option4)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(q => q.CorrectOption)
                   .IsRequired();
        }
    }
}
