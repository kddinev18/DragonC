using DragonC.Compilator.HighLevelCommandsCompiler.Base;
using DragonC.Domain.Compilator;
using DragonC.Domain.Data;
using DragonC.Domain.Lexer.FormalGrammar;
using DragonC.Domain.Lexer.Tokeniser;
using DragonC.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.Compilator.HighLevelCommandsCompiler
{
    class IfCompilator : BaseHighLevelCommand
    {
        private List<string> _condCommands = new List<string>() { "GO_TO" };
        private string _ifHeader;
        private string _ifBody;
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
            (string Header, string Body) ifStatement = ExtractHeaderAndBody(ifStatementOriginal);
            _ifBody = ifStatement.Body;
            _ifHeader = ifStatement.Header;

            command.Token = _ifHeader;
            TokenUnit token = formalGrammar
                .EvaluateTokens(new List<TokenUnit>() { command }, true)
                .First();

            token.Token = ifStatementOriginal;

            return token;
        }

        public override List<string> CompileCommand(TokenUnit command, List<TokenUnit> tokens)
        {

        }

        public override string GetClearCommand(string command)
        {
            return command.Replace(CommandDefintion.AllowLiteralsForArguments ? _data.DynamicValuesIndicator : _data.DynamicNamesIndicator, "");
        }

        private (string Header, string Body) ExtractHeaderAndBody(string block)
        {
            if (string.IsNullOrWhiteSpace(block))
                return (string.Empty, string.Empty);

            int openBraceIndex = block.IndexOf('{');
            int closeBraceIndex = block.LastIndexOf('}');

            if (openBraceIndex == -1 || closeBraceIndex == -1 || closeBraceIndex <= openBraceIndex)
                throw new ArgumentException("Invalid block structure. Missing or misaligned braces.");

            string header = block.Substring(0, openBraceIndex).Trim();

            string body = block.Substring(openBraceIndex + 1, closeBraceIndex - openBraceIndex - 1).Trim();

            return (header, body);
        }
    }
}
