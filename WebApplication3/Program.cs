
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Abstaction;
using WebApplication3.Models;
using WebApplication3.Repo;

namespace WebApplication3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAutoMapper(typeof(MappingProFile));

            builder.Services.AddDbContext<ProductContext>(options =>
            options.UseLazyLoadingProxies().UseNpgsql(builder.Configuration.GetConnectionString("ProductDatabase")));

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            //builder.Host.ConfigureContainer<ContainerBuilder>(x => 
            //x.RegisterType<ProductRepository>().As<IProductRepository>());

            builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
            {
                containerBuilder.RegisterType<ProductRepository>()
                .As<IProductRepository>()
                .InstancePerLifetimeScope();
            });

            //builder.Services.AddSingleton<IProductRepository,ProductRepository>();

            builder.Services.AddMemoryCache(mc => 
            mc.TrackStatistics = true);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
