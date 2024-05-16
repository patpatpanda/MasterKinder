using Microsoft.Extensions.DependencyInjection;
using MasterKinder.Data;
using MasterKinder.Pages;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection")));
builder.Services.AddTransient<CsvService>();

var app = builder.Build();

// Load CSV Data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var csvService = services.GetRequiredService<CsvService>();
    await csvService.OnGetAsync();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
// https://virki.se/
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
