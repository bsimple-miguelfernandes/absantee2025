using Application.DTO;
using Application.DTO.TrainingModule;
using Application.DTO.TrainingSubject;
using Application.Services;
using Domain.Factory;
using Domain.IRepository;
using Domain.Models;
using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.Resolvers;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using InterfaceAdapters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var environment = builder.Environment.EnvironmentName;

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();
builder.Services.AddDbContext<AbsanteeContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    );


//Services
builder.Services.AddTransient<TrainingSubjectService>();
builder.Services.AddTransient<TrainingModuleService>();

//Repositories
builder.Services.AddTransient<ITrainingSubjectRepository, TrainingSubjectRepositoryEF>();
builder.Services.AddTransient<ITrainingModuleRepository, TrainingModuleRepositoryEF>();
//Factories
builder.Services.AddTransient<ITrainingSubjectFactory, TrainingSubjectFactory>();
builder.Services.AddTransient<ITrainingModuleFactory, TrainingModuleFactory>();

//Mappers
builder.Services.AddTransient<TrainingSubjectDataModelConverter>();
builder.Services.AddTransient<TrainingModuleDataModelConverter>();
builder.Services.AddAutoMapper(cfg =>
{
    //DataModels
    cfg.AddProfile<DataModelMappingProfile>();

    //DTO
    cfg.CreateMap<TrainingSubject, TrainingSubjectDTO>();
    cfg.CreateMap<TrainingModule, TrainingModuleDTO>();
});
// MassTransit
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<TrainingModuleCreatedConsumer>();
    x.AddConsumer<TrainingSubjectCreatedConsumer>();
    x.AddConsumer<TrainingModuleUpdatedConsumer>();
    x.AddConsumer<TrainingSubjectUpdatedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost");
        var instance = InstanceInfo.InstanceId;
        cfg.ReceiveEndpoint($"trainingMicroService-query-{instance}", e =>
        {
            e.ConfigureConsumer<TrainingModuleCreatedConsumer>(context);
            e.ConfigureConsumer<TrainingSubjectCreatedConsumer>(context);
            e.ConfigureConsumer<TrainingSubjectUpdatedConsumer>(context);
            e.ConfigureConsumer<TrainingModuleUpdatedConsumer>(context);
        });
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Instance1")
{
    Console.WriteLine("⚠️ Swagger habilitado");
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed((host) => true)
                .AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
