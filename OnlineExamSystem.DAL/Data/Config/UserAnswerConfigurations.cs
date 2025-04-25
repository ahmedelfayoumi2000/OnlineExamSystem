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
    public class UserAnswerConfigurations : IEntityTypeConfiguration<UserAnswer>
    {
        public void Configure(EntityTypeBuilder<UserAnswer> builder)
        {
            builder.HasOne(ua => ua.UserExam)
                   .WithMany(ue => ue.UserAnswers)
                   .HasForeignKey(ua => ua.UserExamId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ua => ua.Question)
                   .WithMany()
                   .HasForeignKey(ua => ua.QuestionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(ua => ua.SelectedOption)
                   .IsRequired();
        }
    }
}
