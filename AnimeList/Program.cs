using AnimeList.DAL;
using AnimeList;
using Microsoft.EntityFrameworkCore;
using AnimeList.Domain.Entity.Account;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AnimeList.Services;
using AnimeList.Hubs;
using NLog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
//Initialize
builder.Services.InitializeRepositories();
builder.Services.InitializeServices();

builder.Services.AddHttpContextAccessor();

var connection = builder.Configuration.GetConnectionString("DefaultConnection");

//Logging
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

//Database
builder.Services.AddDbContext<ApplicationDbContext>(option =>
        option.UseSqlServer(connection));

//Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>();

// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddMvc();
builder.Services.AddControllers();
builder.Services.AddRazorPages();

builder.Services.AddSwaggerGen();

//SignalR
builder.Services.AddSignalR();

//AutoMapper
var config = new AutoMapper.MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AppMappingProfile());
});

builder.Services.AddSingleton(config.CreateMapper());

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient",
       builder => builder
       .WithOrigins("http://localhost:4200")
       .WithOrigins("https://maksbrat.github.io")
       .AllowAnyMethod()
       .AllowAnyHeader());
});

var app = builder.Build();

var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
DbInitializer.Initialize(context);

app.ConfigureCustomExceptionMiddleware();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors("AllowAngularClient");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

app.MapControllers();

app.MapHub<ChatHub>("/chatHub");

app.Run();
