using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OnlineExamSystem.Common.Entities;

namespace OnlineExamSystem.DAL.Data.Config
{
    public class UserExamConfiguration : IEntityTypeConfiguration<UserExam>
    {
        public void Configure(EntityTypeBuilder<UserExam> builder)
        {
            builder.HasOne(ue => ue.User)
                .WithMany(u => u.UserExams)
                .HasForeignKey(ue => ue.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(ue => ue.TakenAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
