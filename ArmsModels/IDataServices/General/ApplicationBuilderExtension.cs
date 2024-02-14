//using Microsoft.AspNetCore.Builder;
//using Microsoft.Extensions.DependencyInjection;

//namespace ArmsServices.DataServices
//{
//    public static class ApplicationBuilderExtension
//    {

//        public static void UseSqlTableDependency<T>(this IApplicationBuilder applicationBuilder, string connectionString)
//            where T : ISqlTableDependencyService
//        {
//            var serviceProvider = applicationBuilder.ApplicationServices;
//            var service = serviceProvider.GetService<T>();
//            service?.SubscribeTableDependency();
//        }
//    }
//}
