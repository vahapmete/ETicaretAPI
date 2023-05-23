using ETicaretAPI.Application.Services;
using ETicaretAPI.Application.Services.Storage;
using ETicaretAPI.Infrastructure.Sevices;
using ETicaretAPI.Infrastructure.Sevices.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure
{
    public static class ServisRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IStorageService, StorageService>();
        }

        public static void AddStorage<T>(this IServiceCollection serviceCollection)where T :class,IStorage
        {
            serviceCollection.AddScoped<IStorage, T>();
        }
       
    }
}
