using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.Domain.Lexer
{
    public class TokenUnit
    {
        public string Token { get; set; }
        public int TextLine { get; set; }
        public int CodeLine { get; set; }
        public int StartCharaterPosition { get; set; }
        public int EndCharacterPosition { get; set; }
        public bool IsValid { get; set; }
        public List<string>? ErrorMessaes { get; set; }
        public int? StartCharacterOfErrorPosition { get; set; }

        public string GetErrors()
        {
            return string.Join(' ', ErrorMessaes) + $" Line of error: {TextLine}, character: {StartCharaterPosition + StartCharacterOfErrorPosition}";
        }
    }
}
