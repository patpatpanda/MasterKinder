using MasterKinder.Data;
using MasterKinder.Services;
using MasterKinder.Models; // Se till att inkludera ApplicationUser-modellen
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.ApplicationInsights.Extensibility;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    }); // Enable API controllers with JSON options

// Configure DbContexts
builder.Services.AddDbContext<MrDb>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultSQLConnection"),
        sqlServerOptions => sqlServerOptions.CommandTimeout(90) // Timeout set to 90 seconds
    ));

// Add memory cache services
builder.Services.AddMemoryCache();

// Add Database Developer Page Exception Filter (useful for debugging in development)
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
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

// Add Identity services with ApplicationUser model
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // Set according to your confirmation policy
})
    .AddEntityFrameworkStores<MrDb>()  // Use your custom DbContext (MrDb)
    .AddDefaultTokenProviders();       // Add token providers for password reset, etc.

// Add HttpClient services for external API requests
builder.Services.AddHttpClient<GeocodeService>();

// Add CORS policy to allow requests from specific origins
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", builder =>
    {
        builder.WithOrigins("http://localhost:3000", "https://xn--frskolekollen-imb.se")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials(); // Tillåt att skicka autentiseringsinformation
    });
});





// Lägg till Application Insights
builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage(); // Show detailed errors in development
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Serve static files from wwwroot
app.UseRouting();

// Use CORS policy
app.UseCors("AllowSpecificOrigins");

// Use Authentication and Authorization

app.UseAuthentication();
app.UseAuthorization();

// Log requests and responses (optional, useful for debugging)
app.Use(async (context, next) =>
{
    Console.WriteLine($"Handling request: {context.Request.Path}");
    await next.Invoke();
    Console.WriteLine($"Finished handling request.");
});

// Map API controllers
app.MapControllers();

// Map Razor Pages
app.MapRazorPages();

// Serve the React app's index.html as a fallback for all other routes (SPA support)
app.MapFallbackToFile("{*path:nonfile}", "index.html");

app.Run();
