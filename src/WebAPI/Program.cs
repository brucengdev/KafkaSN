using QueueClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

var queueToUse = builder.Configuration["QUEUE"];
if(queueToUse == "rabbitmq")
{
    builder.Services.AddRabbitMQConfig(builder.Configuration);
    builder.Services.AddSingleton<IEventProducer, RabbitMQEventProducer>();
} else {
    builder.Services.AddKafkaProducerConfig(builder.Configuration);
    builder.Services.AddSingleton<IEventProducer, KafkaProducer>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

app.Run();