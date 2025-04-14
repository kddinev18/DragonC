using DragonC.Domain.Exceptions;
using DragonC.Domain.Lexer.Tokeniser;
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
            List<string> tokens = ParseStatements(formatedText)
                .Select(x => formatToken(x).Trim())
                .Where(x => !x.StartsWith("//"))
                .ToList();

            List<TokenUnit> result = new List<TokenUnit>();
            foreach (string token in tokens.SkipLast(1))
            {
                Tuple<int, int, int> tokenPosition = FindSubstringLocation(text, token);
                result.Add(new TokenUnit()
                {
                    Token = token,
                    TextLine = tokenPosition.Item1,
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

        private List<string> ParseStatements(string input)
        {
            var result = new List<string>();
            var current = new StringBuilder();
            int braceDepth = 0;

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                current.Append(c);

                if (c == '{')
                {
                    braceDepth++;
                }
                else if (c == '}')
                {
                    braceDepth--;

                    if (braceDepth == 0)
                    {
                        // Completed a block
                        result.Add(current.ToString().Trim());
                        current.Clear();
                        continue; // Continue without appending next char to this block
                    }
                }

                // If we're outside a block and hit a separator
                if (braceDepth == 0 && _tokenSeparators.Contains($"{c}"))
                {
                    string statement = current.ToString().TrimEnd();

                    // Remove the separator at the end
                    if (statement.Length > 0 && _tokenSeparators.Contains($"{statement[^1]}"))
                        statement = statement[..^1].TrimEnd();

                    if (!string.IsNullOrWhiteSpace(statement))
                        result.Add(statement);

                    current.Clear();
                }
            }

            // Add any leftover content
            if (current.Length > 0)
            {
                string remaining = current.ToString().TrimEnd();
                if (remaining.Length > 0 && _tokenSeparators.Contains($"{remaining[^1]}"))
                    remaining = remaining[..^1].TrimEnd();

                if (!string.IsNullOrWhiteSpace(remaining))
                    result.Add(remaining);
            }

            return result;
        }

        private void CompileForHighLevelCommands(List<string> tokens)
        {
            throw new NotImplementedException();
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
