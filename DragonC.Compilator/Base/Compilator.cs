﻿using DragonC.Compilator.HighLevelCommandsCompiler.Base;
using DragonC.Domain.Compilator;
using DragonC.Domain.Lexer.Tokeniser;
using DragonC.Lexer;
using System.Data;
using System.Reflection;

namespace DragonC.Compilator
{
    public partial class Compilator
    {
        private FormalGrammar _formalGrammar;
        private Tokeniser _tokeniser;

        public Compilator()
        {
            _formalGrammar = new FormalGrammar(LowLevelCommands);
            _formalGrammar.DynamicNamesIndicator = DynamicNamesIndicator;
            _formalGrammar.DynamicValuesIndicator = DynamicValuesIndicator;
            _formalGrammar.DynamicCommandIndicator = DynamicCommandIndicator;
            _formalGrammar.NonTerminalIndicator = NonTerminalIndicator;
            _formalGrammar.SetRules(BaseFormalRules);

            _tokeniser = new Tokeniser(TokenSeparators);
        }

        private static List<HighLevelCommand> LoadHighLevelCommands()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            var derivedTypes = assembly.GetTypes()
                               .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(BaseHighLevelCommand)));

            List<HighLevelCommand> highLevelCommands = new List<HighLevelCommand>();
            foreach (Type type in derivedTypes)
            {
                BaseHighLevelCommand instance = (BaseHighLevelCommand)Activator.CreateInstance(type);
                highLevelCommands.Add(instance.CommandDefintion);
            }

            return highLevelCommands;
        }

        public CompiledCode Compile(string text)
        {
            List<TokenUnit> highLevelTokens = _tokeniser.GetTokens(text);
            AddDependancies(highLevelTokens);
            text = TranspileHighLevelCommands(text, CheckForHighLevelCommands(text), highLevelTokens);
            List<TokenUnit> tokenUnits = _formalGrammar.EvaluateTokens(ReorderTokens(highLevelTokens, _tokeniser.GetTokens(text)));
            if (tokenUnits.Any(x => !x.IsValid))
            {
                return new CompiledCode()
                {
                    IsBuildSuccessfully = false,
                    ErrorMessages = tokenUnits.Where(x => !x.IsValid).Select(x => x.GetErrors()).ToList()
                };
            }
            int codeLines = LoadCodeLines(tokenUnits);

            List<TokenUnit> literalTokens = tokenUnits
                .Where(x => x.Token.StartsWith("const") || x.Token.StartsWith("label"))
                .ToList();
            List<TokenUnit> commandTokens = tokenUnits
                .Where(x => !x.Token.StartsWith("const") && !x.Token.StartsWith("label"))
                .ToList();

            CompiledCode compiledCode = new CompiledCode();
            compiledCode.InterMediateCommands = commandTokens
                .Select(x => x.Token)
                .ToList();

            ReplaceDybamicValuesWithCorrespodingValue(literalTokens, commandTokens);
            ReplaceCommadsWithCorespondingValue(commandTokens);
            commandTokens = CompileConditionalCommands(commandTokens);

            commandTokens = commandTokens.OrderBy(x => x.CodeLine).ToList();

            compiledCode.IsBuildSuccessfully = true;
            compiledCode.CompiledCommands = commandTokens
                    .Select(x => Convert.ToString(Convert.ToInt32(x.Token, 2), 16).PadLeft(2, '0').ToUpper())
                    .ToList();
            return compiledCode;
        }

        private void AddDependancies(List<TokenUnit> tokens)
        {
            Compilator.HighLevelCommands
                .Where(x => x.SetConsts != null)
                .ToList()
                .ForEach(x => x.SetConsts.Invoke(tokens));
        }

        private List<TokenUnit> ReorderTokens(List<TokenUnit> beforeTranspilation, List<TokenUnit> afterTranspilation)
        {
            List<TokenUnit> result = new List<TokenUnit>();
            foreach (TokenUnit token in afterTranspilation)
            {
                TokenUnit tokenBeforeTranspilation = beforeTranspilation
                    .Where(x => x == token)
                    .FirstOrDefault();
                if (tokenBeforeTranspilation != null)
                {
                    token.TextLine = tokenBeforeTranspilation.TextLine;
                    token.StartCharaterPosition = tokenBeforeTranspilation.StartCharaterPosition;
                    token.EndCharacterPosition = tokenBeforeTranspilation.EndCharacterPosition;
                }
                result.Add(token);
            }
            return result;
        }

        private string TranspileHighLevelCommands(string text, List<HighLevelCommandToken> HighLevelCommandTokens, List<TokenUnit> tokenUnits)
        {
            foreach (HighLevelCommandToken highLevelCommandToken in HighLevelCommandTokens)
            {
                List<string> lowLevelCommands = highLevelCommandToken.HighLevelCommand.CompileCommand.Invoke(highLevelCommandToken.Token, tokenUnits);
                string lowLevelCommandsText = string.Join(";\n", lowLevelCommands);
                text = text.Replace(highLevelCommandToken.HighLevelCommand.GetClearCommand.Invoke(highLevelCommandToken.Token.Token), lowLevelCommandsText);
            }

            return text;
        }

        private List<HighLevelCommandToken> CheckForHighLevelCommands(string text)
        {
            List<TokenUnit> tokenUnits = _tokeniser.GetTokens(text);
            List<HighLevelCommandToken> result = new List<HighLevelCommandToken>();
            foreach (TokenUnit tokenUnit in tokenUnits)
            {
                foreach (HighLevelCommand highLevelCommand in HighLevelCommands)
                {
                    if (highLevelCommand.ValidateCommand.Invoke(tokenUnit).IsValid == true)
                    {
                        result.Add(new HighLevelCommandToken()
                        {
                            HighLevelCommand = highLevelCommand,
                            Token = tokenUnit
                        });
                    }
                }
            }
            return result;
        }

        private List<TokenUnit> CompileConditionalCommands(List<TokenUnit> commandTokens)
        {
            List<TokenUnit> result = new List<TokenUnit>();
            foreach (TokenUnit token in commandTokens)
            {
                string[] splits = token.Token.Split(' ');
                if (splits.Length == 2)
                {
                    result.Add(new TokenUnit()
                    {
                        Token = splits[1],
                        TextLine = token.TextLine,
                        CodeLine = token.CodeLine,
                        StartCharaterPosition = token.StartCharaterPosition,
                        EndCharacterPosition = token.EndCharacterPosition,
                        IsValid = token.IsValid,
                        ErrorMessaes = token.ErrorMessaes,
                        StartCharacterOfErrorPosition = token.StartCharacterOfErrorPosition,
                    });

                    token.Token = splits[0];
                    token.CodeLine++;
                    result.Add(token);
                }
                else
                {
                    result.Add(token);
                }
            }

            return result;
        }

        private void ReplaceCommadsWithCorespondingValue(List<TokenUnit> commandTokens)
        {
            foreach (TokenUnit token in commandTokens)
            {
                string[] splits = token.Token.Split(DynamicCommandIndicator);
                if (splits.Length == 1 && splits[0].StartsWith(DynamicLiteralIndicator) && splits[0].EndsWith(DynamicLiteralIndicator))
                {
                    token.Token = Convert.ToString(int.Parse(token.Token.Replace(DynamicLiteralIndicator, "")), 2).PadLeft(8, '0');
                }
                if (splits.Length == 3)
                {
                    LowLevelCommand command = LowLevelCommands
                    .Where(x => splits[1] == x.CommandName)
                    .First();

                    token.Token = token.Token.Replace($"{DynamicCommandIndicator}{splits[1]}{DynamicCommandIndicator}",
                        command.MachineCode);
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
                        command.Token = command.Token.Replace(splits[1], Convert.ToString(literalToken.CodeLine, 2).PadLeft(8, '0'));
                        command.Token = command.Token.Replace(splits[1].Replace(DynamicNamesIndicator, DynamicValuesIndicator),
                            Convert.ToString(literalToken.CodeLine, 2).PadLeft(8, '0'));
                    }
                }
                else if (splits.Length == 3 && splits[0] == "const")
                {
                    foreach (TokenUnit command in commandTokens)
                    {
                        if (command.Token.StartsWith(DynamicLiteralIndicator) && command.Token.EndsWith(DynamicLiteralIndicator))
                        {
                            command.Token = command.Token.Replace(DynamicLiteralIndicator, "");
                        }
                        command.Token = command.Token.Replace(splits[1],
                            Convert.ToString(int.Parse(splits[2].Replace(DynamicValuesIndicator, "")), 2)).PadLeft(8, '0');
                        command.Token = command.Token.Replace(splits[1].Replace(DynamicNamesIndicator, DynamicValuesIndicator),
                            Convert.ToString(int.Parse(splits[2].Replace(DynamicValuesIndicator, "")), 2)).PadLeft(8, '0');
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
