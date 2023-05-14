using Basket.API;
using Basket.API.Repositories;

internal class Program
{
    private static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
        //var builder = WebApplication.CreateBuilder(args);

        //// Add services to the container.
        //builder.Services.AddControllers();
        //// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        //builder.Services.AddEndpointsApiExplorer();
        //builder.Services.AddSwaggerGen();


        //var app = builder.Build();

        //// Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        //{
        //    app.UseSwagger();
        //    app.UseSwaggerUI();
        //}

        //app.UseAuthorization();

        //app.MapControllers();


        ////builder.Services.AddStackExchangeRedisCache(options =>
        ////{
        ////    options.Configuration = app.Configuration.GetValue<string>("CacheSettings:ConnectionString");
        ////});
        ////builder.Services.AddScoped<IBasketRepository, BasketRepository>();

        //app.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
          Host.CreateDefaultBuilder(args)
              .ConfigureWebHostDefaults(webBuilder =>
              {
                  webBuilder.UseStartup<Startup>();
              });
}