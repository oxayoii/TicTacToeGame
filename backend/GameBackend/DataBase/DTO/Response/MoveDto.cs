using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.DTO.Response
{
    public class MoveDto
    {
        public int MoveNumber { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public string PlayerSymbol { get; set; }
        public bool WasRandomized { get; set; }
        public DateTime CreatedAt { get; set; }
        public StringValues ETag { get; set; }
    }
}
