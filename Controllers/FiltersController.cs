using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Al_Campus_Assistant.Data;
using Al_Campus_Assistant.Models;
using System.Text.Json;

namespace Al_Campus_Assistant.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FiltersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public FiltersController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserFilters(int userId)
    {
        try
        {
            var filters = await _context.Filters
                .Where(f => f.UserId == userId || f.IsGlobal)
                .Select(f => new
                {
                    f.Id,
                    f.Name,
                    f.FilterType,
                    CriteriaJson = f.CriteriaJson, // ← تغيير هنا
                    f.UserId,
                    f.IsActive,
                    f.IsGlobal,
                    f.IsDefault,
                    f.CreatedAt,
                    f.LastUsedAt,
                    f.UsageCount,
                    f.Color,
                    f.Icon
                })
                .ToListAsync();

            // تحويل JSON بعد جلب البيانات
            var result = filters.Select(f => new
            {
                f.Id,
                f.Name,
                f.FilterType,
                Criteria = JsonSerializer.Deserialize<Dictionary<string, object>>(f.CriteriaJson) ?? new(),
                f.UserId,
                f.IsActive,
                f.IsGlobal,
                f.IsDefault,
                f.CreatedAt,
                f.LastUsedAt,
                f.UsageCount,
                f.Color,
                f.Icon
            }).ToList();

            return Ok(new
            {
                success = true,
                message = "تم جلب الفلاتر بنجاح",
                data = result
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "حدث خطأ في جلب الفلاتر",
                error = ex.Message
            });
        }
    }

    [HttpPost("create/{userId}")]
    public async Task<IActionResult> CreateFilter(int userId, [FromBody] CreateFilterRequest request)
    {
        try
        {
            var filter = new Filter
            {
                Name = request.Name,
                FilterType = request.FilterType,
                CriteriaJson = JsonSerializer.Serialize(request.Criteria ?? new Dictionary<string, object>()),
                UserId = userId,
                IsGlobal = request.IsGlobal,
                CreatedAt = DateTime.Now
            };

            _context.Filters.Add(filter);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "تم إنشاء الفلتر بنجاح",
                filterId = filter.Id
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "حدث خطأ في إنشاء الفلتر",
                error = ex.Message
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFilter(int id, [FromBody] UpdateFilterRequest request)
    {
        try
        {
            var filter = await _context.Filters.FindAsync(id);
            if (filter == null)
                return NotFound(new { success = false, message = "الفلتر غير موجود" });

            if (!string.IsNullOrEmpty(request.Name))
                filter.Name = request.Name;

            if (request.Criteria != null)
                filter.CriteriaJson = JsonSerializer.Serialize(request.Criteria);

            if (request.IsActive.HasValue)
                filter.IsActive = request.IsActive.Value;

            if (request.IsGlobal.HasValue)
                filter.IsGlobal = request.IsGlobal.Value;

            filter.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "تم تحديث الفلتر بنجاح"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "حدث خطأ في تحديث الفلتر",
                error = ex.Message
            });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFilter(int id)
    {
        try
        {
            var filter = await _context.Filters.FindAsync(id);
            if (filter == null)
                return NotFound(new { success = false, message = "الفلتر غير موجود" });

            _context.Filters.Remove(filter);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "تم حذف الفلتر بنجاح"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "حدث خطأ في حذف الفلتر",
                error = ex.Message
            });
        }
    }

    [HttpPost("apply/{userId}")]
    public async Task<IActionResult> ApplyFilter(int userId, [FromBody] ApplyFilterRequest request)
    {
        try
        {
            var filter = await _context.Filters.FindAsync(request.FilterId);
            if (filter == null)
                return NotFound(new { success = false, message = "الفلتر غير موجود" });

            // Update usage count
            filter.UsageCount++;
            filter.LastUsedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            // Return success
            return Ok(new
            {
                success = true,
                message = "تم تطبيق الفلتر بنجاح",
                filter = new
                {
                    filter.Name,
                    filter.FilterType,
                    filter.UsageCount
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "حدث خطأ في تطبيق الفلتر",
                error = ex.Message
            });
        }
    }
}