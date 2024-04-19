using DragonC.Domain.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.Lexer
{
    public class Tokeniser
    {
        private List<string> _tokenSeparators;
        public Tokeniser(List<string> tokenSeparators)
        {
            _tokenSeparators = tokenSeparators;
        }

        public List<TokenUnit> GetTokens(string text)
        {
            string formatedText = FormatText(text);
            List<string> tokens = formatedText.Split(_tokenSeparators.ToArray(), StringSplitOptions.None)
                .Select(x => formatToken(x))
                .ToList();
            if (tokens.Last() != "")
            {
                throw new SyntaxException($"Missing ; or :");
            }

            List<TokenUnit> result = new List<TokenUnit>();
            foreach (string token in tokens.SkipLast(1))
            {
                Tuple<int, int, int> tokenPosition = FindSubstringLocation(text, token);
                result.Add(new TokenUnit()
                {
                    Token = token,
                    CodeLine = tokenPosition.Item1,
                    StartCharaterPosition = tokenPosition.Item2,
                    EndCharacterPosition = tokenPosition.Item3,
                    IsValid = true
                });
            }

            for (int i = 0; i < result.Count(); i++)
            {
                result[i].Token = formatToken(result[i].Token);
            }

            return result;
        }

        public static Tuple<int, int, int> FindSubstringLocation(string unformattedText, string token)
        {
            string[] lines = unformattedText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            for (int i = 0; i < lines.Length; i++)
            {
                int index = lines[i].IndexOf(token);
                if (index != -1)
                {
                    int endIndex = index + token.Length - 1;

                    return Tuple.Create(i + 1, index + 1, endIndex + 1);
                }
            }

            return Tuple.Create(-1, -1, -1);
        }

        private string formatToken(string token)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (char c in token)
            {
                if (c != '\n' && c != '\r' && c != '\t')
                {
                    stringBuilder.Append(c);
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
