using System.ComponentModel.DataAnnotations;

namespace EnsekTestAutomation.Models;

public class Orders
{
    [Required]
    public string? fuel { get; set; }
    [Required]
    public Guid? id { get; set; }
    [Required]
    public int? quantity { get; set; }
    [Required]
    public string? time { get; set; }
}