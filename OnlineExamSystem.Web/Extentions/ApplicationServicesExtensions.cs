using OnlineExamSystem.BLL.Interfaces;
using OnlineExamSystem.BLL.Repositories;
using OnlineExamSystem.BLL.Services;

namespace OnlineExamSystem.Web.Extentions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
          services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
          services.AddScoped<IExamRepository, ExamRepository>();
          services.AddScoped<IQuestionRepository, QuestionRepository>();
          services.AddScoped<IUserExamRepository, UserExamRepository>();
          services.AddScoped<IUserAnswerRepository, UserAnswerRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IExamService, ExamService>();
            services.AddScoped<IAdminService, AdminService>();

            return services;
        }
    }
}
