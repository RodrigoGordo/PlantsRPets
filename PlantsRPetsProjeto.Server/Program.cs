using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using PlantsRPetsProjeto.Server.Models;
using PlantsRPetsProjeto.Server.Data;
using Microsoft.IdentityModel.Tokens;
using PlantsRPetsProjeto.Server.Services;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container
builder.Services.AddControllers();

builder.Services.AddDbContext<PlantsRPetsProjetoServerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PlantsRPetsProjetoServerContext") ?? throw new InvalidOperationException("Connection string 'PlantsRPetsProjetoServerContext' not found.")));

//Add Identity
builder.Services
    .AddIdentityApiEndpoints<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<PlantsRPetsProjetoServerContext>();

// Configure Identity
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    // User settings
    options.User.RequireUniqueEmail = true;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddTransient<IEmailService, SendGridEmailService>();
builder.Services.AddHttpClient<WeatherService>();
builder.Services.AddHttpClient<PlantInfoService>();
builder.Services.AddHttpClient<SustainabilityTipService>();
builder.Services.AddHttpClient<EmojiKitchenService>();
builder.Services.AddScoped<PetGeneratorService>();
builder.Services.AddScoped<PetSeeder>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API",
        Version = "v1"
    });

    // Configura��o de autentica��o no Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insere o token JWT no campo 'Authorization' como: Bearer {token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                Array.Empty<string>()
            }
        });
});

// Enable CORS

builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", policy =>
    {
        policy.WithOrigins("https://localhost:4200", "https://127.0.0.1:4200", "https://plants-r-pets.azurewebsites.net/")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });

    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin() // Permite chamadas de qualquer dom�nio (usar com cuidado em produ��o)
                  .AllowAnyMethod() // Permite qualquer m�todo (GET, POST, etc.)
                  .AllowAnyHeader(); // Permite qualquer cabe�alho na requisi��o
        });
});


var app = builder.Build();

// User e Role seeders
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    var context = services.GetRequiredService<PlantsRPetsProjetoServerContext>();
    var petSeeder = services.GetRequiredService<PetSeeder>();
    
    await context.Database.MigrateAsync();

    // Chamar os seeders
    await RoleSeeder.SeedRoles(roleManager);
    await UserSeeder.SeedUsersAsync(userManager);
    //await PlantSeeder.SeedPlants(context);
    //await TipSeeder.SeedSustainabilityTips(context);
    await petSeeder.SeedAsync();

}


// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Angular");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app
    .MapGroup("/api")
    .MapIdentityApi<User>();

app.MapFallbackToFile("/index.html");

app.Run();
