using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DoAnChuyenNganh.Contract.Repositories.Interface;
using DoAnChuyenNganh.Repositories.UOW;

namespace DoAnChuyenNganh.Services
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRepositories();
        }
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
