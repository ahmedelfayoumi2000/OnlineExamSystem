using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OnlineExamSystem.Common.Models;

namespace OnlineExamSystem.DAL.Data.Config
{
    public class UserExamConfigurations : IEntityTypeConfiguration<UserExam>
    {
        public void Configure(EntityTypeBuilder<UserExam> builder)
        {
            builder.HasKey(ue => ue.Id); 
            builder.Property(ue => ue.Id)
                   .ValueGeneratedOnAdd(); 

            builder.HasOne(ue => ue.User)
                   .WithMany()
                   .HasForeignKey(ue => ue.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ue => ue.Exam)
                   .WithMany(e => e.UserExams)
                   .HasForeignKey(ue => ue.ExamId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(ue => ue.Score)
                   .IsRequired();

            builder.Property(ue => ue.IsPassed)
                   .IsRequired();

            builder.Property(ue => ue.TakenAt)
                   .IsRequired()
                   .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
