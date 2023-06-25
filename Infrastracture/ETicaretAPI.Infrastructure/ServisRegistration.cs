using ETicaretAPI.Application.Services;
using ETicaretAPI.Application.Services.AppUser;
using ETicaretAPI.Application.Services.Storage;
using ETicaretAPI.Application.Services.TokenService;
using ETicaretAPI.Infrastructure.Sevices.Storage;
using ETicaretAPI.Infrastructure.Sevices.TokenService;
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
            serviceCollection.AddScoped<ITokenHandler, TokenHandler>();
        } 

        public static void AddStorage<T>(this IServiceCollection serviceCollection)where T :Storage,IStorage
        {
            serviceCollection.AddScoped<IStorage, T>();
        }
       
    }
}
