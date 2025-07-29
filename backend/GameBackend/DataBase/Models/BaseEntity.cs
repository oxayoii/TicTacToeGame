using System.ComponentModel.DataAnnotations;

namespace DataBase.Models;

public class BaseEntity
{
    [Key]
    public int Id { get; set; }
    
    public DateTime Created { get; set; }
    
    
    public DateTime Modified { get; set; }
}