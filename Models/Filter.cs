using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Al_Campus_Assistant.Models;

public class Filter
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public string FilterType { get; set; } = string.Empty; // courses, exams, announcements, professors
    
    public string CriteriaJson { get; set; } = "{}";
    
    [NotMapped]
    public Dictionary<string, object> Criteria
    {
        get => JsonSerializer.Deserialize<Dictionary<string, object>>(string.IsNullOrEmpty(CriteriaJson) ? "{}" : CriteriaJson) ?? new();
        set => CriteriaJson = JsonSerializer.Serialize(value);
    }
    
    public int UserId { get; set; }
    public virtual User User { get; set; } = null!;
    
    public bool IsActive { get; set; } = true;
    public bool IsGlobal { get; set; } = false;
    public bool IsDefault { get; set; } = false;
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? LastUsedAt { get; set; }
    
    public int UsageCount { get; set; } = 0;
    public int SortOrder { get; set; } = 0;
    
    public string? Color { get; set; } = "#3B82F6";
    public string? Icon { get; set; } = "filter";
}

public class CreateFilterRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public string FilterType { get; set; } = string.Empty;
    
    public Dictionary<string, object> Criteria { get; set; } = new();
    
    public bool IsGlobal { get; set; } = false;
}

public class UpdateFilterRequest
{
    public string? Name { get; set; }
    public Dictionary<string, object>? Criteria { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsGlobal { get; set; }
}

public class ApplyFilterRequest
{
    public int FilterId { get; set; }
    public Dictionary<string, object>? AdditionalCriteria { get; set; }
}