using agendaback.Repository.Interface;
using agendaback.Repository;
using agendaback.Data;
using Microsoft.EntityFrameworkCore;
using agendaback.ErrorHandling;
using agendaback.AuthSetting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation;
using agendaback.Validations;
using agendaback.Services;
using agendaback.Util;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.WebHost.ConfigureKestrel((context, serverOptions) =>
//{
//    serverOptions.ListenAnyIP(5050);
//});

//database
//var connectionDBAgenda = "Server=192.168.1.6;user ID=agenda;Password=VIVER@tec#10;Database=agenda";
//var connectionDBAgenda = "Server=192.168.1.6;user ID=agenda;Password=VIVER@tec#10;Database=agenda_homologacao"; 
//var connectionDBGLPI = "Server=192.168.1.6;user ID=agenda;Password=VIVER@tec#10;Database=glpi_suporteti";
//var connectionDBGLPISistemas = "Server=192.168.1.6;user ID=agenda;Password=VIVER@tec#10;Database=glpi_suportedesistemas";
var connectionDBAgenda = builder.Configuration.GetConnectionString("DatabaseAgenda");
var connectionDBGLPI = builder.Configuration.GetConnectionString("DatabaseGLPISuporteTI");
var connectionDBGLPISistemas = builder.Configuration.GetConnectionString("DatabaseGLPISuporteSistemas");


builder.Services.AddDbContext<ContextDBAgenda>(options =>
                options.UseMySql(connectionDBAgenda, Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.34-mysql")));
builder.Services.AddDbContext<ContextDBGLPITI>(options =>
                options.UseMySql(connectionDBGLPI, Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.34-mysql")));
builder.Services.AddDbContext<ContextDBGLPISistemas>(options =>
                options.UseMySql(connectionDBGLPISistemas, Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.34-mysql")));

//validacoes
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();
builder.Services.AddSignalR();

builder.Services.AddHostedService<BackgroundServices>();


//interfaces
builder.Services.AddScoped<IUserAgendaModelValidatorRepository, UserAgendaModelValidatorRepository>();
builder.Services.AddTransient<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IEntitiesGLPIRepository, EntitiesGLPIRepository>();
builder.Services.AddScoped<ITicketAgendaRepository, TicketAgendaRepository>();
builder.Services.AddScoped<ITicketGLPIRepository, TicketGLPIRepository>();
builder.Services.AddScoped<ITicketUserGLPIRepository, TicketUserGLPIRepository>();
builder.Services.AddScoped<IUserAgendaRepository, UserAgendaRepository>();


//Criptografia de autenticação
var key = Encoding.ASCII.GetBytes(Settings.Secret);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

//Politicas
builder.Services.AddAuthorizationBuilder()
    //Politicas
               .AddPolicy("Administrator", policy =>
    {
        policy.RequireRole("Administrator");
    })
    //Politicas
               .AddPolicy("Coordinator", policy =>
    {
        policy.RequireRole("Coordinator");
    })
    //Politicas
               .AddPolicy("Supervisor", policy =>
    {
        policy.RequireRole("Supervisor");
    })
    //Politicas
               .AddPolicy("Technician", policy =>
    {
        policy.RequireRole("Technician");
    });

//Cors
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.AllowAnyHeader()
                   .AllowAnyMethod()
                   .SetIsOriginAllowed((host) => true)
                   .AllowCredentials();
    //builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("corsapp");

app.MapHub<HubSocket>("/chat");

app.UseMiddleware<ShowException>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();