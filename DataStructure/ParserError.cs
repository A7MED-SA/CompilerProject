using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainConsole.DataStructure
{
    public class ParserError
    {
        public int Line { get; set; }
        public required string Message { get; set; }
        public required string TokenFound { get; set; }
        public required string Expected { get; set; }

        public override string ToString()
        {
            return $"[خطأ - السطر {Line}] {Message}\n  وجد: '{TokenFound}'\n  متوقع: {Expected}";
        }
    }
}
