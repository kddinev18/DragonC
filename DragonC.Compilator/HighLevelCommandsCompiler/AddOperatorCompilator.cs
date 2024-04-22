using DragonC.Domain.Compilator;
using DragonC.Domain.Lexer;
using DragonC.Lexer;

namespace DragonC.Compilator.HighLevelCommandsCompiler
{
    public class AddOperatorCompilator : BaseHighLevelCommand
    {
        public override HighLevelCommand CommandDefintion { get; set; }
        public AddOperatorCompilator()
        {
            CommandDefintion = new HighLevelCommand()
            {
                CommandDefinition = $"arg1 + arg2",
                CommandIndentificatror = " + ",
                CommandSeparator = " + ",
                Arguments = new List<string>() { "arg1", "arg2" },
                AllowedValuesForArguments = new List<string>() { "REG1", "REG2", "REG3", "REG4", "REG5" },
                AllowLiteralsForArguments = true,
                FormalRules = new List<UnformatedRule>()
                {
                    new UnformatedRule()
                    {
                        Rule = "addCommandStart=>@_@%space%",
                        IsStart = true,
                    },
                    new UnformatedRule()
                    {
                        Rule = "space=> %secondParam%"
                    },
                    new UnformatedRule()
                    {
                        Rule = "secondParam=>@_@"
                    }
                },
                ValidateCommand = ValidateCommand,
                CompileCommand = ComplileCommand
            };
        }


        public override TokenUnit ValidateCommand(TokenUnit command)
        {
            FormalGrammar formalGrammar = new FormalGrammar();
            formalGrammar.SetRules(CommandDefintion.FormalRules);
            formalGrammar.AllowLiteralsForHighLevelCommands = CommandDefintion.AllowLiteralsForArguments;
            formalGrammar.AllowedValuesForHighLevelCommands = CommandDefintion.AllowedValuesForArguments;

            return formalGrammar
                .EvaluateTokens(new List<TokenUnit>() { command })
                .First();
        }

        public override List<LowLevelCommand> ComplileCommand(TokenUnit command)
        {
            string[] splits = command.Token.Split(CommandDefintion.CommandSeparator);
            return null;
        }
    }
}
