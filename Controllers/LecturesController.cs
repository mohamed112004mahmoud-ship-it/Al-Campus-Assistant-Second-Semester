using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Al_Campus_Assistant.Data;
using Al_Campus_Assistant.Models;

namespace Al_Campus_Assistant.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LecturesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public LecturesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllLectures()
    {
        try
        {
            var lectures = await _context.Lectures
                .Include(l => l.Course)
                .OrderBy(l => l.LectureDate)
                .ThenBy(l => l.StartTime)
                .Select(l => new
                {
                    l.Id,
                    l.Title,
                    l.Description,
                    l.LectureDate,
                    StartTime = l.StartTime.ToString(@"hh\:mm"),
                    EndTime = l.EndTime.ToString(@"hh\:mm"),
                    l.Location,
                    l.LectureType,
                    l.Status,
                    Course = new
                    {
                        l.Course.Id,
                        l.Course.Name,
                        l.Course.Code
                    }
                })
                .ToListAsync();

            return Ok(new
            {
                success = true,
                message = $"تم جلب {lectures.Count} محاضرة",
                data = lectures
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "حدث خطأ في جلب المحاضرات",
                error = ex.Message
            });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLecture(int id)
    {
        try
        {
            var lecture = await _context.Lectures
                .Include(l => l.Course)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lecture == null)
                return NotFound(new { success = false, message = "المحاضرة غير موجودة" });

            return Ok(new
            {
                success = true,
                data = new
                {
                    lecture.Id,
                    lecture.Title,
                    lecture.Description,
                    lecture.LectureDate,
                    StartTime = lecture.StartTime.ToString(@"hh\:mm"),
                    EndTime = lecture.EndTime.ToString(@"hh\:mm"),
                    lecture.Location,
                    lecture.LectureType,
                    lecture.Status,
                    Course = new
                    {
                        lecture.Course.Id,
                        lecture.Course.Name,
                        lecture.Course.Code
                    }
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "حدث خطأ في جلب المحاضرة",
                error = ex.Message
            });
        }
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateLecture([FromBody] CreateLectureRequest request)
    {
        try
        {
            var lecture = new Lecture
            {
                Title = request.Title,
                Description = request.Description,
                CourseId = request.CourseId,
                LectureDate = request.LectureDate,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Location = request.Location,
                LectureType = request.LectureType ?? "نظري",
                IsMandatory = request.IsMandatory,
                IsOnline = request.IsOnline,
                OnlineLink = request.OnlineLink,
                Instructor = request.Instructor,
                Status = "مجدولة",
                MaxCapacity = request.MaxCapacity,
                CreatedAt = DateTime.Now
            };

            _context.Lectures.Add(lecture);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "تم إنشاء المحاضرة بنجاح",
                lectureId = lecture.Id
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "حدث خطأ في إنشاء المحاضرة",
                error = ex.Message
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLecture(int id, [FromBody] UpdateLectureRequest request)
    {
        try
        {
            var lecture = await _context.Lectures.FindAsync(id);
            if (lecture == null)
                return NotFound(new { success = false, message = "المحاضرة غير موجودة" });

            if (!string.IsNullOrEmpty(request.Title)) lecture.Title = request.Title;
            if (!string.IsNullOrEmpty(request.Description)) lecture.Description = request.Description;
            if (request.LectureDate.HasValue) lecture.LectureDate = request.LectureDate.Value;
            if (request.StartTime.HasValue) lecture.StartTime = request.StartTime.Value;
            if (request.EndTime.HasValue) lecture.EndTime = request.EndTime.Value;
            if (!string.IsNullOrEmpty(request.Location)) lecture.Location = request.Location;
            if (!string.IsNullOrEmpty(request.Status)) lecture.Status = request.Status;
            if (request.IsOnline.HasValue) lecture.IsOnline = request.IsOnline.Value;
            if (!string.IsNullOrEmpty(request.OnlineLink)) lecture.OnlineLink = request.OnlineLink;
            if (request.MaxCapacity.HasValue) lecture.MaxCapacity = request.MaxCapacity.Value;

            lecture.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "تم تحديث المحاضرة بنجاح"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "حدث خطأ في تحديث المحاضرة",
                error = ex.Message
            });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLecture(int id)
    {
        try
        {
            var lecture = await _context.Lectures.FindAsync(id);
            if (lecture == null)
                return NotFound(new { success = false, message = "المحاضرة غير موجودة" });

            _context.Lectures.Remove(lecture);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "تم حذف المحاضرة بنجاح"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "حدث خطأ في حذف المحاضرة",
                error = ex.Message
            });
        }
    }

    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcomingLectures()
    {
        try
        {
            var lectures = await _context.Lectures
                .Include(l => l.Course)
                .Where(l => l.LectureDate >= DateTime.Now)
                .Where(l => l.Status == "مجدولة")
                .OrderBy(l => l.LectureDate)
                .ThenBy(l => l.StartTime)
                .Select(l => new
                {
                    l.Id,
                    l.Title,
                    l.LectureDate,
                    StartTime = l.StartTime.ToString(@"hh\:mm"),
                    l.Location,
                    l.IsOnline,
                    Course = new
                    {
                        l.Course.Name,
                        l.Course.Code
                    }
                })
                .ToListAsync();

            return Ok(new
            {
                success = true,
                message = $"تم جلب {lectures.Count} محاضرة قادمة",
                data = lectures
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "حدث خطأ في جلب المحاضرات القادمة",
                error = ex.Message
            });
        }
    }
}