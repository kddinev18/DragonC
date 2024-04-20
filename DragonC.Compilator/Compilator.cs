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
                    ErrorMessages = tokenUnits.Where(x=>!x.IsValid).Select(x=>x.GetErrors()).ToList()
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
            CompileConditionalCommands(commandTokens.Where(x=>x.Token.Contains(' ')).ToList());

            commandTokens = commandTokens.OrderBy(x => x.CodeLine).ToList();
            return new CompiledCode()
            {
                IsBuildSuccessfully = true,
                CompiledCommands = commandTokens.Select(x=>x.Token).ToList(),
            };
        }

        private void CompileConditionalCommands(List<TokenUnit> commandTokens)
        {
            foreach (TokenUnit token in commandTokens)
            {
                string[] splits = token.Token.Split(' ');
                token.Token = splits[0];

                commandTokens.Add(new TokenUnit()
                {
                    Token = splits[1],
                    TextLine = token.TextLine,
                    CodeLine = token.CodeLine - 1,
                    StartCharaterPosition = token.StartCharaterPosition,
                    EndCharacterPosition = token.EndCharacterPosition,
                    IsValid = token.IsValid,
                    ErrorMessaes = token.ErrorMessaes,
                    StartCharacterOfErrorPosition = token.StartCharacterOfErrorPosition,
                });
            }
        }

        private void ReplaceCommadsWithCorespondingValue(List<TokenUnit> commandTokens)
        {
            foreach (TokenUnit token in commandTokens)
            {
                string[] splits = token.Token.Split(DynamicCommandIndicator);
                if (splits.Length == 3)
                {
                    Command command = _commands
                    .Where(x => splits[1] == x.CommandName)
                    .First();

                    token.Token = token.Token.Replace($"{DynamicCommandIndicator}{splits[1]}{DynamicCommandIndicator}",
                        command.MachineCode.ToString());
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
                string[] splits = token.Token.Split(' ');

                if (splits.Length == 2 && 
                    splits[0].StartsWith(DynamicCommandIndicator) && 
                    splits[0].EndsWith(DynamicCommandIndicator) &&
                    splits[1].StartsWith(DynamicValuesIndicator) &&
                    splits[1].EndsWith(DynamicValuesIndicator))
                {
                    token.CodeLine = ++codeLines;
                    codeLines++;
                }
                else if (token.Token.StartsWith("label"))
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
