using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.DTO.Response
{
    public class GameDto
    {
        public int Id { get; set; }
        public int BoardSize { get; set; }
        public string[] BoardState { get; set; }
        public string CurrentPlayerSymbol { get; set; }
        public bool IsCompleted { get; set; }
        public string? WinnerSymbol { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public List<MoveDto> Moves { get; set; } = new();
    }
}
