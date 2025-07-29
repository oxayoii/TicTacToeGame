using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Models
{
    public class Move : BaseEntity
    {
        public int GameId { get; set; }
        public Game Game { get; set; }
        
        [Required]
        public int Row { get; set; }

        [Required]
        public int Column { get; set; } 

        [Required]
        [MaxLength(1)]
        public string PlayerSymbol { get; set; } 
        public bool WasRandomized { get; set; } = false;
        public int MoveNumber { get; set; }
    }
}
