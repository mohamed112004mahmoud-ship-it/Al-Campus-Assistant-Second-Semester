using Microsoft.EntityFrameworkCore;
using Al_Campus_Assistant.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

// إضافة Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Al Campus Assistant API",
        Version = "v1",
        Description = "API نظام مساعد الحرم الجامعي",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "الفريق التقني",
            Email = "support@campus.edu"
        }
    });
    
    // لإضافة التعليقات العربية
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (System.IO.File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// CORS
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Al Campus Assistant API v1");
        c.RoutePrefix = "swagger"; // جعل swagger الصفحة الرئيسية
        c.DocumentTitle = "Al Campus Assistant API Documentation";
    });
}

app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseAuthorization();
app.MapControllers();

// Create database
try
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
    Console.WriteLine("✅ تم تطبيق EF Core migrations بنجاح!");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ خطأ: {ex.Message}");
}

// Test endpoints
app.MapGet("/", () => Results.Redirect("/swagger")); // Redirect to Swagger
app.MapGet("/health", () => new 
{ 
    status = "healthy", 
    database = "SQL Server",
    time = DateTime.Now,
    swagger = "/swagger" 
});

app.Run();