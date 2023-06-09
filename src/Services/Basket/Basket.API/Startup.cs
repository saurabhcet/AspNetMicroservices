﻿using Basket.API.GrpcService;
using Basket.API.Repositories;
using Discount.Grpc.Protos;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Basket.API
{
    public class Startup
    {
        // Constructor to initialize the configuration
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Redis configuration
            services.AddStackExchangeRedisCache(options => {
                options.Configuration = Configuration.GetValue<string>("CacheSettings:ConnectionString");
            });

            //General configuration
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddAutoMapper(typeof(Startup));
            
            //Grpc configuration
            services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(
                options => options.Address = new Uri(Configuration.GetValue<string>("GrpcSettings:DiscountUrl")));
           
            services.AddScoped<DiscountGrpcService>();
            
            //MassTransit-RabbitMQ configuration
            services.AddMassTransit(options => {
                options.UsingRabbitMq((ctx, config) =>
                {
                    config.Host(Configuration["EventBusSettings:HostAddress"]);
                });
            });
            //services.AddMassTransitHostedService();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket.API", Version = "v1" });
            });            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.API v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

}
