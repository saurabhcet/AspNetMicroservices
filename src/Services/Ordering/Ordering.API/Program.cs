using EventBus.Messages.Common;
using EventBus.Messages.Events;
using MassTransit;
using Ordering.API.EventBusConsumer;
using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(configuration);

//MassTransit-RabbitMQ configuration
builder.Services.AddMassTransit(options => {
    options.AddConsumer<BasketCheckoutConsumer>();
    options.UsingRabbitMq((ctx, config) =>
    {
        config.Host(configuration["EventBusSettings:HostAddress"]);
        config.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c => {
            c.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
        });
    });
});
//services.AddMassTransitHostedService();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.MigrateDatabase<OrderContext>((context, services) =>
{
    var logger = services.GetService<ILogger<OrderContextSeed>>();
    OrderContextSeed.SeedAsync(context, logger).Wait();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
