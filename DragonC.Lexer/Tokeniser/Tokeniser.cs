using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.Lexer.Tokeniser
{
    public class Tokeniser
    {
        private List<string> _tokenSeparators;
        public Tokeniser(List<string> tokenSeparators)
        {
            _tokenSeparators = tokenSeparators;
        }

        public List<string> ReplaceDeynamicValues(List<string> tokens)
        {
            foreach (string token in tokens)
            {

            }
        }

        public List<string> GetTokens(string text)
        {
            text = FormatText(text);
            return text.Split(_tokenSeparators.ToArray(), StringSplitOptions.None)
                .Select(x => FormatToken(x + ";"))
                .SkipLast(1)
                .ToList();
        }

        private string FormatToken(string token)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < token.Length; i++)
            {
                if (token[i] != '\r' && token[i] != '\t' && token[i] != '\n')
                {
                    stringBuilder.Append(token[i]);
                }
            }
            return stringBuilder.ToString();
        }

        private string FormatText(string text)
        {
            string[] lines = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            var filteredLines = lines.Where(line => !string.IsNullOrWhiteSpace(line));

            return string.Join("\n", filteredLines);
        }
    }
}
