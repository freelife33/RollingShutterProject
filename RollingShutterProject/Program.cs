using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RollingShutterProject.Data;
using RollingShutterProject.Interfaces;
using RollingShutterProject.Repositories;
using RollingShutterProject.Services;
using RollingShutterProject.UnitOfWork;
using Serilog;
using Serilog.Events;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.WithOrigins("https://localhost:7132")    
              .AllowAnyHeader()     
              .AllowAnyMethod()   
              .AllowCredentials();
    });
});

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddScoped<ISensorDataRepository, SensorDataRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IMqttService, MqttService>();
builder.Services.AddSingleton<INotificationService, NotificationService>();



builder.Services.AddHttpContextAccessor();

builder.Services.AddHostedService<AutomationService>();

builder.Services.AddControllers();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000); 
    options.ListenAnyIP(5001, listenOptions =>
    {
        listenOptions.UseHttps(); 
    });
});


//JWT
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]!);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options=>
    {
        options.RequireHttpsMetadata = false; 
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RollingShutter API", Version = "v1" });

    // JWT Kimlik Doðrulama için Güvenlik Tanýmlamasý
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Lütfen 'Bearer {token}' formatýnda JWT token giriniz.",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    // API'nin JWT Kimlik Doðrulama gerektirdiðini Swagger'a bildir
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

//Scop

builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Loglarý dosyaya yazdýrmak için
Log.Logger= new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console() // Konsola log yaz
    .WriteTo.File("Logs/SensorLog-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();
//builder.Services.AddSingleton<Serilog.Extensions.Hosting.DiagnosticContext>();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

var app = builder.Build();


app.UseCors("AllowAllOrigins");

//MQTT

var mqttService= app.Services.GetRequiredService<IMqttService>();
await mqttService.ConnectAsycn(); 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
