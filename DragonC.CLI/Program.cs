using DragonC.Domain.Compilator;

namespace DragonC.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Compilator.Compilator compilator = new Compilator.Compilator();

            CompiledCode code = compilator.Compile(
            @"
            const imm 1;

            imm;
            IMM_TO_REGT;
            REGT_TO_REG1;

            label main:
                ADD;
                REG3_TO_OUT;
                REG3_TO_REGT;
                REGT_TO_REG2;
            GO_TO imm;

            //label main2:
            //    imm;
            //    IMM_TO_REGT;
            //    REGT_TO_REG1;
            //GO_TO main2;
            "
            .Trim());

            if(code.IsBuildSuccessfully)
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