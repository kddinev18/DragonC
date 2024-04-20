using DragonC.Domain.Compilator;
using DragonC.Domain.Lexer;
using DragonC.Lexer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.Compilator
{
    public partial class Compilator
    {
        private FormalGrammar _formalGrammar;
        private Tokeniser _tokeniser;
        public Compilator()
        {
            _formalGrammar = new FormalGrammar(_commands);
            _formalGrammar.SetRules(_formalRules);
            _tokeniser = new Tokeniser(_tokenSeparators);
        }
        public CompiledCode Compile(string text)
        {
            List<TokenUnit> tokenUnits = _formalGrammar.EvaluateTokens(_tokeniser.GetTokens(text));
            if (tokenUnits.Any(x => !x.IsValid))
            {
                return new CompiledCode()
                {
                    IsBuildSuccessfully = false,
                    ErrorMessages = tokenUnits.Where(x=>!x.IsValid).SelectMany(x=>x.ErrorMessaes).ToList()
                };
            }
            int codeLines = LoadCodeLines(tokenUnits);

            List<TokenUnit> literalTokens = tokenUnits
                .Where(x => x.Token.StartsWith("const") || x.Token.StartsWith("label"))
                .ToList();
            // make all instaces of labels and const to be surounded by other smbols to make it safer for replacement
            List<TokenUnit> commandTokens = tokenUnits
                .Where(x => !x.Token.StartsWith("const") && !x.Token.StartsWith("label"))
                .ToList();

            ReplaceDybamicValuesWithCorrespodingValue(literalTokens, commandTokens);
            ReplaceCommadsWithCorespondingValue(commandTokens);

            return null;
        }

        private void ReplaceCommadsWithCorespondingValue(List<TokenUnit> commandTokens)
        {
            foreach (TokenUnit token in commandTokens)
            {
                if(token.Token.Split(DynamicCommandIndicator).Count() == 3 &&)
                {

                }
            }
        }

        private void ReplaceDybamicValuesWithCorrespodingValue(List<TokenUnit> literalTokens, List<TokenUnit> commandTokens)
        {
            foreach (TokenUnit literalToken in literalTokens)
            {
                string[] splits = literalToken.Token.Split(' ');
                if (splits.Length == 2 && splits[0] == "label")
                {
                    foreach (TokenUnit command in commandTokens)
                    {
                        command.Token = command.Token.Replace(splits[1], literalToken.CodeLine.ToString());
                        command.Token = command.Token.Replace(splits[1].Replace(DynamicNamesIndicator, DynamicValuesIndicator),
                            literalToken.CodeLine.ToString());
                    }
                }
                else if (splits.Length == 3 && splits[0] == "const")
                {
                    foreach (TokenUnit command in commandTokens)
                    {
                        command.Token = command.Token.Replace(splits[1], splits[2].Replace(DynamicValuesIndicator, ""));
                        command.Token = command.Token.Replace(splits[1].Replace(DynamicNamesIndicator, DynamicValuesIndicator),
                            splits[2].Replace(DynamicValuesIndicator, ""));
                    }
                }
            }
        }

        private int LoadCodeLines(List<TokenUnit> tokenUnits)
        {
            int codeLines = 0;
            foreach (TokenUnit token in tokenUnits)
            {
                if(token.Token.StartsWith("label"))
                {
                    token.CodeLine = codeLines;
                }
                else if (!token.Token.StartsWith("const"))
                {
                    token.CodeLine = ++codeLines;
                }
            }

            return codeLines;
        }
    }
}
