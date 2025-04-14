using DragonC.Compilator.HighLevelCommandsCompiler.Base;
using DragonC.Domain.Compilator;
using DragonC.Domain.Data;
using DragonC.Domain.Lexer.FormalGrammar;
using DragonC.Domain.Lexer.Tokeniser;
using DragonC.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.Compilator.HighLevelCommandsCompiler
{
    class IfCompilator : BaseHighLevelCommand
    {
        private List<string> _condCommands = new List<string>() { "GO_TO" };
        public IfCompilator(CompilatorData data) : base(data)
        {
            CommandDefintion = new HighLevelCommand()
            {
                CommandDefinition = @"if(cond)
{
code
}",
                CommandIndentificatror = "if",
                CommandSeparator = "if",
                Arguments = new List<string>() { "cond" },
                AllowedValuesForArguments = _condCommands,
                AllowLiteralsForArguments = false,
                FormalRules = new List<UnformatedRule>()
                {
                    new UnformatedRule()
                    {
                        Rule = "ifCommandStart->if(%space%",
                        IsStart = true
                    },
                    new UnformatedRule()
                    {
                        Rule = "space-> %condCommand%",
                    },
                    new UnformatedRule()
                    {
                        Rule = "condCommand->#_#%2ndSpace%",
                    },
                    new UnformatedRule()
                    {
                        Rule = "2ndSpace-> )%figureBracket%",
                    },
                    new UnformatedRule()
                    {
                        Rule = "space-> %condCommand%",
                    },
                    new UnformatedRule()
                    {
                        Rule = "condCommand->#_#%2ndSpace%",
                    },
                    new UnformatedRule()
                    {
                        Rule = "2ndSpace-> )",
                    },
                },
                ValidateCommand = ValidateCommand,
                CompileCommand = CompileCommand,
                GetClearCommand = GetClearCommand,
            };
        }

        public override HighLevelCommand CommandDefintion { get; set; }

        public override TokenUnit ValidateCommand(TokenUnit command, List<TokenUnit> tokens)
        {
            FormalGrammar formalGrammar = new FormalGrammar();
            formalGrammar.SetRules(CommandDefintion.FormalRules);
            formalGrammar.AllowLiteralsForHighLevelCommands = CommandDefintion.AllowLiteralsForArguments;
            formalGrammar.AllowedValuesForHighLevelCommands = CommandDefintion.AllowedValuesForArguments;
            formalGrammar.NumberOfArguments = CommandDefintion.Arguments.Count;

            string ifStatementOriginal = command.Token;
            try
            {
                (string Header, string Body) ifStatement = ExtractHeaderAndBody(ifStatementOriginal);


                command.Token = ifStatement.Header;
                TokenUnit token = formalGrammar
                    .EvaluateTokens(new List<TokenUnit>() { command }, true)
                    .First();

                token.Token = ifStatementOriginal;

                return token;
            }
            catch
            {
                return new TokenUnit()
                {
                    IsValid = false
                };
            }
        }

        public override List<string> CompileCommand(TokenUnit command, List<TokenUnit> tokens)
        {
            (string Header, string Body) ifStatement = ExtractHeaderAndBody(command.Token);
            string ifLabelName = $"ifStatementBlock{Guid.NewGuid()}";
            string endIfLabelName = $"endIfStatementBlock{Guid.NewGuid()}";

            string ifLabelDefinition = $"label {ifLabelName}";
            string endIfLabelDefinition = $"label {endIfLabelName}";

            string condCommand = ExtractCondition(ifStatement.Header);

            List<string> result = new List<string>();

            result.Add(string.Concat(condCommand, " ", ifLabelName));
            result.Add(string.Concat("GO_TO", " ", endIfLabelName));
            result.Add(ifLabelDefinition);

            List<string> ifBody = ifStatement.Body.Split(_data.TokenSeparators.ToArray(), StringSplitOptions.None)
                .Select(x => x.Trim())
                .SkipLast(1)
                .ToList();

            foreach (var commandInIf in ifBody)
            {
                result.Add(commandInIf);
            }
            result.Add(endIfLabelDefinition);


            return result;
        }

        public override string GetClearCommand(string command)
        {
            string clearCommand = command.Replace(CommandDefintion.AllowLiteralsForArguments ? _data.DynamicValuesIndicator : _data.DynamicNamesIndicator, "");
            return clearCommand;
        }

        private string ExtractCondition(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            int openParenIndex = input.IndexOf('(');
            int closeParenIndex = input.LastIndexOf(')');

            string condition = input.Substring(openParenIndex + 1, closeParenIndex - openParenIndex - 1).Trim();

            return condition;
        }

        private (string Header, string Body) ExtractHeaderAndBody(string block)
        {
            if (string.IsNullOrWhiteSpace(block))
                return (string.Empty, string.Empty);

            int openBraceIndex = block.IndexOf('{');
            int closeBraceIndex = block.LastIndexOf('}');

            string header = block.Substring(0, openBraceIndex).Trim();

            string body = block.Substring(openBraceIndex + 1, closeBraceIndex - openBraceIndex - 1).Trim();

            return (header, body);
        }
    }
}
