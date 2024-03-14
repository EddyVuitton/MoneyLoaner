using MoneyLoaner.WebAPI.Extensions;
using MoneyLoaner.WebAPI.Helpers;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
WebAPIHelper.AddServices(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DefaultModelsExpandDepth(-1);
        c.DocExpansion(docExpansion: DocExpansion.None);
        c.EnableTryItOutByDefault();
    });
}

/*
 * Podczas ka¿dego uruchomienia aplikacji migruj bazê DBContext.cs do lokalnego serwera SQL (localdb)\\MSSQLLocalDB
*/
if (app.Environment.IsDevelopment()) //Tylko i wy³¹cznie na œrodowisku deweloperskim
{
    app.ReMigrateDatabase();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();