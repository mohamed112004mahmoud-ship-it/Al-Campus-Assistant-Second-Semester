using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Al_Campus_Assistant.Models;

public class Lecture
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "اسم المحاضرة مطلوب")]
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "رقم المادة مطلوب")]
    public int CourseId { get; set; }
    public virtual Course Course { get; set; } = null!;
    
    [Required(ErrorMessage = "تاريخ المحاضرة مطلوب")]
    public DateTime LectureDate { get; set; }
    
    [Required(ErrorMessage = "وقت البدء مطلوب")]
    public TimeSpan StartTime { get; set; }
    
    [Required(ErrorMessage = "وقت الانتهاء مطلوب")]
    public TimeSpan EndTime { get; set; }
    
    [Required(ErrorMessage = "مكان المحاضرة مطلوب")]
    public string Location { get; set; } = string.Empty;
    
    public string? LectureType { get; set; } = "نظري";
    public bool IsMandatory { get; set; } = true;
    public bool IsOnline { get; set; } = false;
    public string? OnlineLink { get; set; }
    public string? Instructor { get; set; }
    public string? Status { get; set; } = "مجدولة";
    public int? AttendanceCount { get; set; }
    public int? MaxCapacity { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }

    // إضافة القيم الافتراضية
    public ICollection<LectureMaterial> Materials { get; set; } = new List<LectureMaterial>();
    public ICollection<LectureAttendance> Attendances { get; set; } = new List<LectureAttendance>();
}

public class LectureMaterial
{
    public int Id { get; set; }
    public int LectureId { get; set; }
    public virtual Lecture Lecture { get; set; } = null!;
    public string FileName { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string? Description { get; set; }
    public bool IsPublic { get; set; } = true;
    public int DownloadCount { get; set; } = 0;
    public DateTime UploadedAt { get; set; } = DateTime.Now;
    public int UploadedBy { get; set; }
}

public class LectureAttendance
{
    public int Id { get; set; }
    public int LectureId { get; set; }
    public virtual Lecture Lecture { get; set; } = null!;
    public int StudentId { get; set; }
    public virtual User Student { get; set; } = null!;
    public DateTime AttendanceTime { get; set; } = DateTime.Now;
    public string Status { get; set; } = "حاضر";
    public bool IsExcused { get; set; } = false;
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class CreateLectureRequest
{
    [Required] public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    [Required] public int CourseId { get; set; }
    [Required] public DateTime LectureDate { get; set; }
    [Required] public TimeSpan StartTime { get; set; }
    [Required] public TimeSpan EndTime { get; set; }
    [Required] public string Location { get; set; } = string.Empty;
    public string? LectureType { get; set; }
    public bool IsMandatory { get; set; } = true;
    public bool IsOnline { get; set; } = false;
    public string? OnlineLink { get; set; }
    public string? Instructor { get; set; }
    public int? MaxCapacity { get; set; }
}

public class UpdateLectureRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? LectureDate { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public string? Location { get; set; }
    public string? Status { get; set; }
    public bool? IsOnline { get; set; }
    public string? OnlineLink { get; set; }
    public int? MaxCapacity { get; set; }
}

public class MarkAttendanceRequest
{
    [Required] public int StudentId { get; set; }
    [Required] public string Status { get; set; } = "حاضر";
    public bool IsExcused { get; set; } = false;
    public string? Notes { get; set; }
}

public class UploadMaterialRequest
{
    [Required] public string FileName { get; set; } = string.Empty;
    [Required] public string FileType { get; set; } = string.Empty;
    [Required] public string FileUrl { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string? Description { get; set; }
    public bool IsPublic { get; set; } = true;
}