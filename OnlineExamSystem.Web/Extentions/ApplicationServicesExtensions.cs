using OnlineExamSystem.BLL.Helpers;
using OnlineExamSystem.BLL.ServiceInterfaces;
using OnlineExamSystem.BLL.Services;
using OnlineExamSystem.Common.Interfaces;
using OnlineExamSystem.DAL.Repositories;

namespace OnlineExamSystem.Web.Extentions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IExamRetrievalService, ExamRetrievalService>();
            services.AddScoped<IExamModificationService, ExamModificationService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IUserExamService, UserExamService>();
            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }
    }
}
