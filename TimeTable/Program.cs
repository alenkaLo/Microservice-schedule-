using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using TimeTable.Controllers;
using TimeTable.Data;
using TimeTable.Models.Repository;
using TimeTable.Services;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddHostedService<ConsumerBase>();

// Add services to the container.
builder.Services.AddDbContext<LessonDbContext>(
    options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(LessonDbContext)));
    });


var consumerConfig = new ConsumerConfig //It is necessary to take the value of the variable BootstrapServers from appsettings.json
{
    BootstrapServers = "localhost:9092",
    GroupId = "LessonService",
    AutoOffsetReset = AutoOffsetReset.Earliest
};
builder.Services.AddSingleton<IConsumer<Ignore, string>>(sp =>
    new ConsumerBuilder<Ignore, string>(consumerConfig).Build());
builder.Services.AddHostedService<TaskConsumer>();
builder.Services.AddSingleton<KafkaModule>(new KafkaModule());

builder.Services.AddControllers();
builder.Services.AddScoped<ILessonService, LessonService>();
builder.Services.AddScoped<ILessonRepository, LessonRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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