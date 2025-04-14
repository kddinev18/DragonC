using DragonC.Compilator.HighLevelCommandsCompiler.Base;
using DragonC.Domain.Compilator;
using DragonC.Domain.Data;
using DragonC.Domain.Lexer.Tokeniser;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.HLCC.Services
{
    public class CommandPluginProjectService : ICommandPluginProjectService
    {
        private readonly string _projectFolder;
        private readonly string _csprojFilePath;

        public CommandPluginProjectService()
        {
            _projectFolder = Path.Combine(Path.GetTempPath(), "DragonCPluginTemp");
            _csprojFilePath = Path.Combine(_projectFolder, "DragonCPluginTemp.csproj");

            // Ensure directory exists
            Directory.CreateDirectory(_projectFolder);
        }

        public List<HighLevelCommand> LoadHighLevelCommands(CompilatorData data)
        {
            // 1. Get all .cs files
            var sourceFiles = Directory.GetFiles(_projectFolder, "*.cs");

            if (!sourceFiles.Any())
                return new List<HighLevelCommand>();

            // 2. Parse all files into syntax trees
            var syntaxTrees = sourceFiles.Select(path => CSharpSyntaxTree.ParseText(File.ReadAllText(path))).ToList();

            var referencePaths = new List<string>();

            string exePath = AppContext.BaseDirectory;
            // 3. Add your app-specific DLLs
            referencePaths.AddRange(new[]
            {
                Path.Combine(exePath, "DragonC.Domain.dll"),
                Path.Combine(exePath, "DragonC.Lexer.dll"),
                Path.Combine(exePath, "DragonC.Compilator.dll")
            });

            // 3.1. Add system assemblies dynamically
            var trustedAssemblies = AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")?.ToString();
            if (!string.IsNullOrEmpty(trustedAssemblies))
            {
                var requiredSystemDlls = new[]
                {
                    "System.Runtime.dll",
                    "System.Private.CoreLib.dll",
                    "System.Collections.dll",
                    "System.Linq.dll",
                    "System.Console.dll",
                    "System.Text.RegularExpressions.dll",
                    "System.IO.dll",
                    "System.Reflection.dll",
                    "netstandard.dll"
                };

                var systemPaths = trustedAssemblies
                    .Split(Path.PathSeparator)
                    .Where(p => requiredSystemDlls.Any(name => p.EndsWith(name, StringComparison.OrdinalIgnoreCase)))
                    .Distinct();

                referencePaths.AddRange(systemPaths);
            }

            var references = referencePaths
                .Where(File.Exists)
                .Select(p => MetadataReference.CreateFromFile(p))
                .ToList();

            // 4. Compile into in-memory assembly
            var compilation = CSharpCompilation.Create(
                "DynamicHighLevelCommands",
                syntaxTrees,
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            );

            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);

            if (!result.Success)
            {
                var errors = result.Diagnostics
                    .Where(d => d.Severity == DiagnosticSeverity.Error)
                    .Select(d => d.ToString());

                throw new Exception("Compilation failed:\n" + string.Join("\n", errors));
            }

            ms.Seek(0, SeekOrigin.Begin);
            var assembly = Assembly.Load(ms.ToArray());

            // 5. Instantiate all derived types
            var baseType = typeof(BaseHighLevelCommand);
            var derivedTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(baseType));

            var commands = new List<HighLevelCommand>();

            foreach (var type in derivedTypes)
            {
                var instance = (BaseHighLevelCommand)Activator.CreateInstance(type, data);
                commands.Add(instance.CommandDefintion);
            }

            return commands;
        }

        public HighLevelCommandFile GenerateProject(string[] referenceDllPaths)
        {
            // Generate a new .cs file
            string classFileName = "HighLevelCommand_" + Guid.NewGuid().ToString("N") + ".cs";
            string classFilePath = Path.Combine(_projectFolder, classFileName);
            string fileContent = GetDefaultClassCode();
            File.WriteAllText(classFilePath, fileContent);

            // Generate .csproj if not exists
            if (!File.Exists(_csprojFilePath))
            {
                File.WriteAllText(_csprojFilePath, GenerateCsProjContent(referenceDllPaths));
            }

            return new HighLevelCommandFile()
            {
                FileName = classFileName,
                FullFilePath = classFilePath,
                ProjectPath = _projectFolder,
                FileContent = fileContent
            };
        }

        private string GenerateCsProjContent(string[] dllPaths)
        {
            var references = string.Join(Environment.NewLine, dllPaths.Select(dll =>
    $@"    <Reference Include=""{Path.GetFileNameWithoutExtension(dll)}"">
      <HintPath>{dll}</HintPath>
    </Reference>"));

            return $@"<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
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
using System.Collections.Generic;
using System.Linq;
using System.Private.CoreLib;
using System.Reflection;
using System;

namespace DragonC.Compilator.HighLevelCommandsCompiler
{
    public class HighLevelCommandImplementation : BaseHighLevelCommand, IAllowConsts
    {
        public HighLevelCommandImplementation(CompilatorData data) : base(data)
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
