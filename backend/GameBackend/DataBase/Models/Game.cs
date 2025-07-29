using System.ComponentModel.DataAnnotations;

namespace DataBase.Models;

public class Game : BaseEntity
{
    public int Id { get; set; }
    [Required] 
    public string[] BoardState { get; set; }

    [Required] 
    [MaxLength(1)] 
    public string CurrentPlayerSymbol { get; set; } = "X";
    public bool IsCompleted { get; set; } = false;

    [MaxLength(1)] 
    public string? WinnerSymbol { get; set; }
    [Required] 
    public int BoardSize { get; set; } = 3;
    public DateTime? FinishedAt { get; set; }
    public ICollection<Move> Movies { get; set; }
}