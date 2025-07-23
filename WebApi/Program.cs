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
using Domain.Factory.TrainingPeriodFactory;
using Application.IPublisher;

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
builder.Services.AddTransient<TrainingPeriodService>();

builder.Services.AddTransient<IMessagePublisher, MassTransitPublisher>();

//Repositories
builder.Services.AddTransient<ITrainingPeriodRepository, TrainingPeriodRepositoryEF>();
builder.Services.AddTransient<ITrainingSubjectRepository, TrainingSubjectRepositoryEF>();
builder.Services.AddTransient<ITrainingModuleRepository, TrainingModuleRepositoryEF>();
//Factories
builder.Services.AddTransient<ITrainingSubjectFactory, TrainingSubjectFactory>();
builder.Services.AddTransient<ITrainingModuleFactory, TrainingModuleFactory>();
builder.Services.AddTransient<ITrainingPeriodFactory, TrainingPeriodFactory>();

//Mappers
builder.Services.AddTransient<TrainingSubjectDataModelConverter>();
builder.Services.AddTransient<TrainingModuleDataModelConverter>();
builder.Services.AddTransient<TrainingPeriodDataModelConverter>();
builder.Services.AddAutoMapper(cfg =>
{
    //DataModels
    cfg.AddProfile<DataModelMappingProfile>();

    //DTO
    cfg.CreateMap<TrainingSubject, TrainingSubjectDTO>();
    cfg.CreateMap<TrainingModule, TrainingModuleDTO>();
    cfg.CreateMap<TrainingModule, UpdatedTrainingModuleDTO>();
    cfg.CreateMap<TrainingSubject, UpdatedTrainingSubjectDTO>();

    cfg.CreateMap<TrainingPeriod, TrainingPeriodDTO>();
    cfg.CreateMap<TrainingPeriodDTO, TrainingPeriod>();
    cfg.CreateMap<TrainingPeriod, CreateTrainingPeriodDTO>()
            .ForMember(dest => dest.InitDate, opt => opt.MapFrom(src => src.PeriodDate.InitDate))
            .ForMember(dest => dest.FinalDate, opt => opt.MapFrom(src => src.PeriodDate.FinalDate));
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
       cfg.ReceiveEndpoint($"trainingMicroService-cmd-{instance}", e =>
       {
           e.ConfigureConsumer<TrainingSubjectCreatedConsumer>(context);
           e.ConfigureConsumer<TrainingModuleCreatedConsumer>(context);
           e.ConfigureConsumer<TrainingSubjectUpdatedConsumer>(context);
           e.ConfigureConsumer<TrainingModuleUpdatedConsumer>(context);

       });
   });
});
builder.Services.AddScoped<IMessagePublisher, MassTransitPublisher>();
//sender:


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

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

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
