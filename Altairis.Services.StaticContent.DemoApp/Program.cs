using Altairis.Services.StaticContent;
using Altairis.Services.StaticContent.DemoApp.Data;
using Altairis.Services.StaticContent.ViewComponents;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DemoDbContext>(options => {
    options.UseSqlite(builder.Configuration.GetConnectionString("StaticContent"));
});

// Add static content services
builder.Services.Configure<StaticContentViewComponentOptions>(options => {
    options.SurroundingElementName = "div";
    options.SurroundingElementAttributes.Add("class", "static");
    options.SurroundingElementAttributes.Add("title", "Firma \"Ďábel & syn\".");
});
builder.Services.AddStaticContent()
    .UseMarkdownFormatter()
    .UseFileStaticContentStore("./App_Data/Content");

// Add Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

// Migrate DB to latest version
using var scope = app.Services.CreateScope();
using var dc = scope.ServiceProvider.GetRequiredService<DemoDbContext>();
dc.Database.Migrate();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.Run();
