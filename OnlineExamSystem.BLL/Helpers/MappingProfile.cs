using AutoMapper;
using OnlineExamSystem.BLL.Dtos;
using OnlineExamSystem.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamSystem.BLL.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Exam, ExamDto>();
            CreateMap<ExamDto, Exam>();
            CreateMap<Question, QuestionDto>();
            CreateMap<QuestionDto, Question>();
            CreateMap<Option, OptionDto>();
            CreateMap<OptionDto, Option>();
            CreateMap<UserExam, UserExamDto>();
              
            CreateMap<UserExamDto, UserExam>();
        }
    }
}
