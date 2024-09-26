using MasterKinder.Data;
using MasterKinder.Services;
using MasterKinder.Models; // Se till att inkludera ApplicationUser-modellen
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
        sqlServerOptions => sqlServerOptions.CommandTimeout(300) // Timeout in seconds, here it's set to 300 seconds (5 minutes)
    ));

builder.Services.AddMemoryCache();
builder.Services.AddDatabaseDeveloperPageExceptionFilter(); // Lägg till denna rad

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
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddEntityFrameworkStores<MrDb>()  // Use your own DbContext (MrDb)
    .AddDefaultTokenProviders();       // Add token providers for password reset, etc.

// Add memory cache services
builder.Services.AddMemoryCache();

// Add HttpClient services
builder.Services.AddHttpClient<GeocodeService>();

// Add CORS policy (allow both localhost and production URL)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", builder =>
    {
        builder.WithOrigins("http://localhost:3000", "https://xn--frskolekollen-imb.se")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials(); // If necessary for cookies (though JWT doesn’t need them)
    });
});

var app = builder.Build();
app.UseAuthentication(); // Add this before Authorization
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage(); // Lägg till denna rad för att visa detaljerade fel i utvecklingsläge
}

app.Use(async (context, next) =>
{
    Console.WriteLine($"Handling request: {context.Request.Path}");
    await next.Invoke();
    Console.WriteLine($"Finished handling request.");
});

app.UseHttpsRedirection();
app.UseStaticFiles(); // Serve static files from wwwroot
app.UseRouting();
app.UseCors("AllowSpecificOrigins"); // Använd samma CORS-policy överallt

// Use Authentication and Authorization middleware
app.UseAuthentication(); // Lägg till detta för att aktivera autentisering
app.UseAuthorization();

app.MapControllers(); // Enable API routes

// Map Razor Pages
app.MapRazorPages();

// Serve the React app's index.html as a fallback for all other routes
app.MapFallbackToFile("{*path:nonfile}", "index.html");

app.Run();
