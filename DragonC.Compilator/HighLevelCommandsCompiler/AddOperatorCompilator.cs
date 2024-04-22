using DragonC.Compilator.HighLevelCommandsCompiler.Base;
using DragonC.Domain.Compilator;
using DragonC.Domain.Lexer;
using DragonC.Lexer;

namespace DragonC.Compilator.HighLevelCommandsCompiler
{
    public class AddOperatorCompilator : BaseHighLevelCommand, IAllowConsts
    {
        private List<string> _registers = new List<string>() { "REG1", "REG2", "REG3", "REG4", "REG5" };
        public override HighLevelCommand CommandDefintion { get; set; }
        public AddOperatorCompilator()
        {
            CommandDefintion = new HighLevelCommand()
            {
                CommandDefinition = $"arg1 + arg2",
                CommandIndentificatror = " + ",
                CommandSeparator = " + ",
                Arguments = new List<string>() { "arg1", "arg2" },
                AllowedValuesForArguments = _registers,
                AllowLiteralsForArguments = true,
                FormalRules = new List<UnformatedRule>()
                {
                    new UnformatedRule()
                    {
                        Rule = "addCommandStart->@_@%space%",
                        IsStart = true,
                    },
                    new UnformatedRule()
                    {
                        Rule = "space-> %operator%"
                    },
                    new UnformatedRule()
                    {
                        Rule = "operator->+%2ndSpace%"
                    },
                    new UnformatedRule()
                    {
                        Rule = "2ndSpace-> %2ndOperator%"
                    },
                    new UnformatedRule()
                    {
                        Rule = "2ndSpace->@_@"
                    }
                },
                ValidateCommand = ValidateCommand,
                CompileCommand = CompileCommand,
                GetClearCommand = GetClearCommand,
                SetConsts = SetConsts
            };
        }


        public override TokenUnit ValidateCommand(TokenUnit command)
        {
            FormalGrammar formalGrammar = new FormalGrammar();
            formalGrammar.SetRules(CommandDefintion.FormalRules);
            formalGrammar.AllowLiteralsForHighLevelCommands = CommandDefintion.AllowLiteralsForArguments;
            formalGrammar.AllowedValuesForHighLevelCommands = CommandDefintion.AllowedValuesForArguments;
            formalGrammar.NumberOfArguments = CommandDefintion.Arguments.Count;

            return formalGrammar
                .EvaluateTokens(new List<TokenUnit>() { command }, true)
                .First();
        }

        public override List<string> CompileCommand(TokenUnit command)
        {
            List<string> lowLevelCommands = new List<string>();
            string[] splits = command.Token.Split(CommandDefintion.CommandSeparator);
            string arg1 = splits[0].Replace(Compilator.DynamicValuesIndicator, "");
            string arg2 = splits[1].Replace(Compilator.DynamicValuesIndicator, "");

            bool isArg1Literal = int.TryParse(arg1, out int argValue1);
            bool isArg2Literal = int.TryParse(arg2, out int argValue2);

            switch ((isArg1Literal, isArg2Literal))
            {
                case (true, true):
                    lowLevelCommands.AddRange(SetImmediateValue(Guid.NewGuid(), argValue1, "REG1"));
                    lowLevelCommands.AddRange(SetImmediateValue(Guid.NewGuid(), argValue2, "REG2"));
                    break;
                case (true, false):
                    lowLevelCommands.AddRange(SetImmediateValue(Guid.NewGuid(), argValue1, "REG1"));
                    lowLevelCommands.AddRange(OverrideRegister(arg2, "REG2"));
                    break;
                case (false, true):
                    lowLevelCommands.AddRange(OverrideRegister(arg1, "REG1"));
                    lowLevelCommands.AddRange(SetImmediateValue(Guid.NewGuid(), argValue2, "REG2"));
                    break;
                case (false, false):
                    lowLevelCommands.AddRange(SetImmediateValue(Guid.NewGuid(), argValue1, "REG1"));
                    lowLevelCommands.AddRange(SetImmediateValue(Guid.NewGuid(), argValue2, "REG2"));
                    break;

            }
            lowLevelCommands.Add($"ADD");

            return lowLevelCommands;
        }

        public void SetConsts(List<TokenUnit> tokens)
        {
            foreach (TokenUnit token in tokens)
            {
                string[] splits = token.Token.Split(" ");
                if (splits.Length == 3 && splits[0] == "const")
                {
                    CommandDefintion.AllowedValuesForArguments.Add(splits[1]);
                }
            }
        }

        private List<string> SetImmediateValue(Guid constName, int value, string register)
        {
            return new List<string>
            {
                $"const {constName} {value}",
                $"{constName}",
                $"IMM_TO_REGT",
                $"REGT_TO_{register}".ToUpper()
            };
        }

        private List<string> OverrideRegister(string register1, string register2)
        {
            return new List<string>
            {
                $"{register1}_TO_REGT".ToUpper(),
                $"REGT_TO_{register2}".ToUpper(),
            };
        }

        public override string GetClearCommand(string command)
        {
            return command.Replace(CommandDefintion.AllowLiteralsForArguments ? Compilator.DynamicValuesIndicator : Compilator.DynamicNamesIndicator, "");
        }
    }
}
