using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using ProductApi.Repo;
using ProductApi.Repository.Files;
using ProductApi.Repository.Products;
using ProductApi.Services.Files;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
string connectionString = builder.Configuration.GetConnectionString("conn");
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddTransient<IProductRepository, ProductService>();




WebApplication app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
    RequestPath = "/Resources"
});
app.UseAuthorization();

app.MapControllers();

app.Run();
