using MoneyLoaner.Server;
using MoneyLoaner.WebUI.EntryPoint;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServices();

builder.Services.AddRazorComponents(options =>
{
    options.DetailedErrors = true;
});

if (builder.Environment.IsProduction())
{
    //Na potrzeby Dockera
    builder.WebHost.UseUrls("http://0.0.0.0:80");
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();