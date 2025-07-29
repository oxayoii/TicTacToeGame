using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Models;

public class GameConfiguration : BaseEntity
{
    [Required] 
    public int DefaultBoardSize { get; set; }

    [Required] 
    public int WinCondition { get; set; }

    [Required] 
    public int RandomMoveChancePercent { get; set; }

    public int RandomMoveInterval { get; set; }
}