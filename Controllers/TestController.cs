using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Al_Campus_Assistant.Data;
using Microsoft.EntityFrameworkCore;

namespace Al_Campus_Assistant.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config;

    public TestController(ApplicationDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [HttpGet("sqlserver")]
    public IActionResult TestSqlServer()
    {
        try
        {
            var connectionString = _config.GetConnectionString("DefaultConnection");
            using var connection = new SqlConnection(connectionString);
            connection.Open();
            
            var command = new SqlCommand("SELECT @@VERSION, DB_NAME()", connection);
            using var reader = command.ExecuteReader();
            reader.Read();
            
            return Ok(new
            {
                success = true,
                message = "✅ الاتصال بـ SQL Server ناجح",
                sqlVersion = reader[0],
                database = reader[1],
                timestamp = DateTime.Now
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "❌ فشل الاتصال بـ SQL Server",
                error = ex.Message
            });
        }
    }

    [HttpGet("tables")]
    public async Task<IActionResult> CheckTables()
    {
        try
        {
            var userCount = await _context.Users.CountAsync();
            var lectureCount = await _context.Lectures.CountAsync();
            var filterCount = await _context.Filters.CountAsync();
            
            return Ok(new
            {
                success = true,
                tables = new
                {
                    Users = userCount > 0 ? "✅ بها بيانات" : "⭕ فارغة",
                    Lectures = lectureCount > 0 ? "✅ بها بيانات" : "⭕ فارغة",
                    Filters = filterCount > 0 ? "✅ بها بيانات" : "⭕ فارغة"
                },
                counts = new
                {
                    Users = userCount,
                    Lectures = lectureCount,
                    Filters = filterCount
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "❌ خطأ في التحقق من الجداول",
                error = ex.Message
            });
        }
    }

    [HttpGet("lectures")]
    public async Task<IActionResult> TestLectures()
    {
        try
        {
            var lectures = await _context.Lectures
                .Include(l => l.Course)
                .Take(5)
                .Select(l => new
                {
                    l.Id,
                    l.Title,
                    l.LectureDate,
                    StartTime = l.StartTime.ToString(@"hh\:mm"),
                    l.Location,
                    Course = l.Course.Name
                })
                .ToListAsync();
            
            return Ok(new
            {
                success = true,
                message = $"تم العثور على {lectures.Count} محاضرة",
                data = lectures
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "❌ خطأ في جلب المحاضرات",
                error = ex.Message
            });
        }
    }
}