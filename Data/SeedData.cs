using Al_Campus_Assistant.Models;
using Microsoft.EntityFrameworkCore;

namespace Al_Campus_Assistant.Data;

public static class SeedData
{
    public static void Initialize(ApplicationDbContext context)
    {
        Console.WriteLine("🌱 بدء إضافة البيانات الأولية...");

        // 1. Add sample user
        if (!context.Users.Any())
        {
            AddSampleUsers(context);
        }
        
        // 2. Add sample professors
        if (!context.Professors.Any())
        {
            AddSampleProfessors(context);
        }
        
        // 3. Add sample courses
        if (!context.Courses.Any())
        {
            AddSampleCourses(context);
        }
        
        // 4. Add sample lectures
        if (!context.Lectures.Any())
        {
            AddSampleLectures(context);
        }
        
        // 5. Add sample filters
        if (!context.Filters.Any())
        {
            AddSampleFilters(context);
        }
        
        // 6. Add sample announcements
        if (!context.Announcements.Any())
        {
            AddSampleAnnouncements(context);
        }
        
        // 7. Add sample exams
        if (!context.Exams.Any())
        {
            AddSampleExams(context);
        }
        
        Console.WriteLine("✅ تمت إضافة جميع البيانات الأولية");
    }

    private static void AddSampleUsers(ApplicationDbContext context)
    {
        var users = new List<User>
        {
            new User
            {
                FirstName = "أحمد",
                LastName = "محمد",
                Email = "ahmed@test.com",
                PhoneNumber = "01012345678",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test@123"),
                Role = "Student",
                CreatedAt = DateTime.Now,
                IsActive = true,
                AgreeToTerms = true,
                IsAbove18 = true
            },
            new User
            {
                FirstName = "سارة",
                LastName = "علي",
                Email = "sara@test.com",
                PhoneNumber = "01098765432",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test@123"),
                Role = "Professor",
                CreatedAt = DateTime.Now,
                IsActive = true,
                AgreeToTerms = true,
                IsAbove18 = true
            }
        };
        
        context.Users.AddRange(users);
        context.SaveChanges();
        Console.WriteLine($"✅ تم إنشاء {users.Count} مستخدم");
    }

    private static void AddSampleProfessors(ApplicationDbContext context)
    {
        var professors = new List<Professor>
        {
            new Professor
            {
                Name = "د. أحمد محمد",
                Email = "ahmed.prof@university.edu",
                Department = "علوم الحاسب",
                Office = "مبنى أ - مكتب 301",
                Phone = "0101112233",
                OfficeHours = "الإثنين والأربعاء 10-12"
            },
            new Professor
            {
                Name = "د. سارة علي",
                Email = "sara.prof@university.edu",
                Department = "هندسة البرمجيات",
                Office = "مبنى ب - مكتب 205",
                Phone = "0102223344",
                OfficeHours = "الثلاثاء والخميس 9-11"
            }
        };
        
        context.Professors.AddRange(professors);
        context.SaveChanges();
        Console.WriteLine($"✅ تم إنشاء {professors.Count} أستاذ");
    }

    private static void AddSampleCourses(ApplicationDbContext context)
    {
        var professors = context.Professors.ToList();
        
        var courses = new List<Course>
        {
            new Course
            {
                Code = "CS101",
                Name = "مقدمة في البرمجة",
                Description = "مقدمة في أساسيات البرمجة باستخدام C#",
                CreditHours = 3,
                ProfessorId = professors[0].Id
            },
            new Course
            {
                Code = "CS201",
                Name = "قواعد البيانات",
                Description = "تصميم وإدارة قواعد البيانات",
                CreditHours = 3,
                ProfessorId = professors[1].Id
            },
            new Course
            {
                Code = "CS301",
                Name = "هندسة البرمجيات",
                Description = "مبادئ هندسة البرمجيات",
                CreditHours = 4,
                ProfessorId = professors[0].Id
            }
        };
        
        context.Courses.AddRange(courses);
        context.SaveChanges();
        Console.WriteLine($"✅ تم إنشاء {courses.Count} مادة دراسية");
    }

    private static void AddSampleLectures(ApplicationDbContext context)
    {
        var courses = context.Courses.Take(2).ToList();
        if (!courses.Any()) return;
        
        var lectures = new List<Lecture>();
        var rand = new Random();
        
        foreach (var course in courses)
        {
            for (int i = 1; i <= 4; i++)
            {
                var lectureDate = DateTime.Now.AddDays(rand.Next(1, 30));
                lectures.Add(new Lecture
                {
                    Title = $"المحاضرة {i} - {course.Name}",
                    Description = $"محاضرة رقم {i} في مادة {course.Name}",
                    CourseId = course.Id,
                    LectureDate = lectureDate,
                    StartTime = new TimeSpan(8 + (i * 2), 0, 0),
                    EndTime = new TimeSpan(10 + (i * 2), 0, 0),
                    Location = $"قاعة {100 + i}",
                    LectureType = i % 2 == 0 ? "نظري" : "عملي",
                    IsMandatory = true,
                    IsOnline = false,
                    Instructor = "د. غير محدد",
                    Status = "مجدولة",
                    MaxCapacity = 50,
                    CreatedAt = DateTime.Now.AddDays(-rand.Next(1, 10))
                });
            }
        }
        
        context.Lectures.AddRange(lectures);
        context.SaveChanges();
        Console.WriteLine($"✅ تم إنشاء {lectures.Count} محاضرة تجريبية");
    }

    private static void AddSampleFilters(ApplicationDbContext context)
    {
        var user = context.Users.FirstOrDefault();
        if (user == null) return;
        
        var filters = new List<Filter>
        {
            new Filter
            {
                Name = "المقررات الدراسية",
                FilterType = "courses",
                CriteriaJson = "{}",
                UserId = user.Id,
                IsActive = true,
                IsDefault = true,
                CreatedAt = DateTime.Now,
                Color = "#3B82F6",
                Icon = "book"
            },
            new Filter
            {
                Name = "الامتحانات القادمة",
                FilterType = "exams",
                CriteriaJson = "{\"upcomingOnly\":true}",
                UserId = user.Id,
                IsActive = true,
                IsDefault = true,
                CreatedAt = DateTime.Now,
                Color = "#EF4444",
                Icon = "calendar"
            },
            new Filter
            {
                Name = "المحاضرات القادمة",
                FilterType = "courses", // يمكن تغييرها لـlectures لو بتحتاج
                CriteriaJson = "{\"type\":\"upcoming\"}",
                UserId = user.Id,
                IsActive = true,
                IsDefault = false,
                CreatedAt = DateTime.Now,
                Color = "#10B981",
                Icon = "video"
            }
        };
        
        context.Filters.AddRange(filters);
        context.SaveChanges();
        Console.WriteLine($"✅ تم إنشاء {filters.Count} فلتر تجريبي");
    }

    private static void AddSampleAnnouncements(ApplicationDbContext context)
    {
        var announcements = new List<Announcement>
        {
            new Announcement
            {
                Title = "بداية الفصل الدراسي",
                Content = "يبدأ الفصل الدراسي الجديد يوم الأحد القادم",
                Date = DateTime.Now.AddDays(3),
                IsImportant = true,
                Category = "أكاديمي"
            },
            new Announcement
            {
                Title = "ورشة عمل تقنية",
                Content = "ورشة عمل حول تطوير التطبيقات باستخدام Flutter",
                Date = DateTime.Now.AddDays(5),
                IsImportant = false,
                Category = "فعاليات"
            }
        };
        
        context.Announcements.AddRange(announcements);
        context.SaveChanges();
        Console.WriteLine($"✅ تم إنشاء {announcements.Count} إعلان تجريبي");
    }

    private static void AddSampleExams(ApplicationDbContext context)
    {
        var courses = context.Courses.Take(2).ToList();
        if (!courses.Any()) return;
        
        var exams = new List<Exam>
        {
            new Exam
            {
                CourseId = courses[0].Id,
                ExamType = "منتصف الفصل",
                ExamDate = DateTime.Now.AddDays(15),
                Time = "10:00 صباحاً",
                Location = "قاعة 101",
                Duration = 3
            },
            new Exam
            {
                CourseId = courses[1].Id,
                ExamType = "منتصف الفصل",
                ExamDate = DateTime.Now.AddDays(20),
                Time = "01:00 مساءً",
                Location = "قاعة 202",
                Duration = 2
            }
        };
        
        context.Exams.AddRange(exams);
        context.SaveChanges();
        Console.WriteLine($"✅ تم إنشاء {exams.Count} امتحان تجريبي");
    }
}