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
            List<string> tokens = Tokenize(formatedText)
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

        private List<string> Tokenize(string input)
        {
            List<string> tokens = new List<string>();
            int i = 0;
            int length = input.Length;
            StringBuilder currentToken = new StringBuilder();
            bool insideBlock = false;
            int braceDepth = 0;

            while (i < length)
            {
                char c = input[i];

                // Detect block start
                if (!insideBlock && c == '{')
                {
                    insideBlock = true;
                    braceDepth = 1;
                    currentToken.Append(c);
                    i++;
                    continue;
                }

                // Inside a block
                if (insideBlock)
                {
                    currentToken.Append(c);

                    if (c == '{') braceDepth++;
                    else if (c == '}') braceDepth--;

                    if (braceDepth == 0)
                    {
                        insideBlock = false;
                        tokens.Add(currentToken.ToString());
                        currentToken.Clear();
                    }

                    i++;
                    continue;
                }

                // Outside a block: handle custom separators
                if (Array.Exists(_tokenSeparators.ToArray(), sep => sep == $"{c}"))
                {
                    // Add trimmed token if there's content
                    string token = currentToken.ToString().TrimEnd();
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        tokens.Add(token);
                    }
                    currentToken.Clear();
                    i++; // Skip separator
                    continue;
                }

                // Default: accumulate character
                currentToken.Append(c);
                i++;
            }

            // Append any trailing content
            string finalToken = currentToken.ToString().TrimEnd();
            if (!string.IsNullOrWhiteSpace(finalToken))
            {
                tokens.Add(finalToken);
            }

            return MergeBlockHeaders(tokens);
        }

        private List<string> MergeBlockHeaders(List<string> tokens)
        {
            var mergedTokens = new List<string>();
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].TrimEnd().EndsWith("{"))
                {
                    // This shouldn't happen due to block capturing logic
                    mergedTokens.Add(tokens[i]);
                }
                else if (tokens[i].Contains("{") && !tokens[i].TrimStart().StartsWith("{"))
                {
                    // This is a header line + block combined
                    mergedTokens.Add(tokens[i]);
                }
                else
                {
                    mergedTokens.Add(tokens[i]);
                }
            }

            return mergedTokens;
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
