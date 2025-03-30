using DragonC.GUI.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.GUI.Services
{
    public class CommandPluginProjectService : ICommandPluginProjectService
    {
        public string GenerateProject(string[] referenceDllPaths)
        {
            string tempRoot = Path.Combine(Path.GetTempPath(), "DragonCPluginTemp_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempRoot);

            string classFileName = "HighLevelCommand_" + Guid.NewGuid().ToString("N") + ".cs";
            string classFilePath = Path.Combine(tempRoot, classFileName);

            File.WriteAllText(classFilePath, GetDefaultClassCode());
            
            string csprojFilePath = Path.Combine(tempRoot, "DragonCPluginTemp.csproj");
            File.WriteAllText(csprojFilePath, GenerateCsProjContent(referenceDllPaths));

            return tempRoot;
        }

        private string GenerateCsProjContent(string[] dllPaths)
        {
            var references = string.Join(Environment.NewLine, dllPaths.Select(dll =>
    $@"    <Reference Include=""{Path.GetFileNameWithoutExtension(dll)}"">
      <HintPath>{dll}</HintPath>
    </Reference>"));

            return $@"<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <LangVersion>latest</LangVersion>
  </PropertyGroup>
  <ItemGroup>
{references}
  </ItemGroup>
</Project>";
        }

        private string GetDefaultClassCode()
        {
            return @"using DragonC.Compilator.HighLevelCommandsCompiler.Base;
using DragonC.Domain.Compilator;
using DragonC.Domain.Data;
using DragonC.Domain.Exceptions;
using DragonC.Domain.Lexer.FormalGrammar;
using DragonC.Domain.Lexer.Tokeniser;
using DragonC.Lexer;

namespace DragonC.Compilator.HighLevelCommandsCompiler
{
    public class HighLevelCommand : BaseHighLevelCommand, IAllowConsts
    {
        public HighLevelCommand(CompilatorData data) : base(data)
        {
        }

        public override HighLevelCommand CommandDefintion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override List<string> CompileCommand(TokenUnit command, List<TokenUnit> tokens)
        {
            throw new NotImplementedException();
        }

        public override string GetClearCommand(string command)
        {
            throw new NotImplementedException();
        }

        public void SetConsts(List<TokenUnit> tokens)
        {
            throw new NotImplementedException();
        }

        public override TokenUnit ValidateCommand(TokenUnit command)
        {
            throw new NotImplementedException();
        }
    }
}";
        }
    }
}
