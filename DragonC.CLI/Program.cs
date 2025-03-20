using DragonC.Domain.Compilator;
using DragonC.Domain.Data;
using DragonC.Domain.Lexer.FormalGrammar;

namespace DragonC.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CompilatorData data = new CompilatorData()
            {
                TokenSeparators = new List<string>() { ";", ":" },
                LowLevelCommands = new List<LowLevelCommand>()
                {
                    new LowLevelCommand()
                    {
                        CommandName = "IMM_TO_REGT",
                        MachineCode = "01000101"
                    },
                    new LowLevelCommand()
                    {
                        CommandName = "REGT_TO_REG1",
                        MachineCode = "01110000"
                    },
                    new LowLevelCommand()
                    {
                        CommandName = "REG1_TO_REGT",
                        MachineCode = "01000110"
                    },
                    new LowLevelCommand()
                    {
                        CommandName = "ADD",
                        MachineCode = "10000100"
                    },
                    new LowLevelCommand()
                    {
                        CommandName = "REG3_TO_OUT",
                        MachineCode = "01011110"
                    },
                    new LowLevelCommand()
                    {
                        CommandName = "REG3_TO_REGT",
                        MachineCode = "01011101"
                    },
                    new LowLevelCommand()
                    {
                        CommandName = "REG2_TO_REGT",
                        MachineCode = "01010101"
                    },
                    new LowLevelCommand()
                    {
                        CommandName = "REGT_TO_REG2",
                        MachineCode = "01110001"
                    },
                    new LowLevelCommand()
                    {
                        CommandName = "GO_TO",
                        MachineCode = "11000100",
                        IsConditionalCommand = true
                    },
                },
                BaseFormalRules = new List<UnformatedRule>()
                {
                    new UnformatedRule
                    {
                        Rule = "immValue->&_&",
                        IsStart = true
                    },
                    new UnformatedRule
                    {
                        Rule = "immValueConst->|_|",
                        IsStart = true
                    },
                    new UnformatedRule
                    {
                        Rule = "commandExec->#_#",
                        IsStart = true
                    },



                    new UnformatedRule()
                    {
                        Rule = "constDecl->const%space%",
                        IsStart = true
                    },
                    new UnformatedRule
                    {
                        Rule = "space-> %cnastName%",
                    },
                    new UnformatedRule
                    {
                        Rule = "cnastName->|_|%2ndSpace%"
                    },
                    new UnformatedRule
                    {
                        Rule = "2ndSpace-> %constValue%",
                    },
                    new UnformatedRule
                    {
                        Rule = "constValue->@_@",
                    },



                    new UnformatedRule
                    {
                        Rule = "labelDecl->label%spaceLbl%",
                        IsStart = true
                    },
                    new UnformatedRule
                    {
                        Rule = "spaceLbl-> %labelName%",
                    },
                    new UnformatedRule
                    {
                        Rule = "labelName->|_|",
                    },



                    new UnformatedRule
                    {
                        Rule = "condCommandExec->#_#%spaceCmd%",
                        IsStart = true
                    },
                    new UnformatedRule
                    {
                        Rule = "spaceCmd-> %condCommandParam%",
                    },
                    new UnformatedRule
                    {
                        Rule = "condCommandParam->@_@",
                    },


                    new UnformatedRule
                    {
                        Rule = $"{CompilatorData.DynamicNamesFormalGrammarStartRule}->i%allowedDynamicNamesNext%",
                        IsStart = true
                    },
                    new UnformatedRule
                    {
                        Rule = "allowedDynamicNamesStart->m%allowedDynamicNamesNext%",
                        IsStart = true
                    },
                    new UnformatedRule
                    {
                        Rule = "allowedDynamicNamesNext->m%allowedDynamicNamesNext%",
                    },
                    new UnformatedRule
                    {
                        Rule = "allowedDynamicNamesNext->i%allowedDynamicNamesNext%",
                    },
                    new UnformatedRule
                    {
                        Rule = "allowedDynamicNamesNext->m",
                    },
                }
            };
            Compilator.Compilator compilator = new Compilator.Compilator(data);

            CompiledCode code = compilator.Compile(
            @"
                const imm 1;
                imm;
                IMM_TO_REGT;
                REGT_TO_REG1;

                label main;
                    ADD;
                    REG3_TO_OUT;
                    REG3_TO_REGT;
                    REGT_TO_REG2;
                GO_TO main;
                // 1 + 4;
                // REG3 + REG2;
            "
            .Trim());

            if (code.IsBuildSuccessfully)
            {
                for (int i = 0; i < code.CompiledCommands.Count(); i++)
                {
                    Console.WriteLine(code.CompiledCommands[i]);
                }
            }
            else
            {
                for (int i = 0; i < code.ErrorMessages.Count(); i++)
                {
                    Console.WriteLine(code.ErrorMessages[i]);
                }
            }
        }
    }
}