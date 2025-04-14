using DragonC.API.Data;
using DragonC.API.Models.Entities;
using DragonC.Domain.API;
using DragonC.Domain.Compilator;
using DragonC.Domain.Lexer.FormalGrammar;

namespace DragonC.API.Services
{
	public class CodeConfigurationService : ICodeConfigurationService
	{
		private readonly DragonCDbContext _context;

		public CodeConfigurationService(DragonCDbContext context)
		{
			_context = context;
		}

		public void Create(CodeConfigurationDTO codeConfigurationDTO)
		{
			foreach (string token in codeConfigurationDTO.TokenSeparators) {
				this._context.Add(new TokenSeparator
				{
					Separator = token,
					ProjectId = codeConfigurationDTO.ProjectId
				});
			}

			foreach(DragonC.Domain.Compilator.LowLevelCommand lowLevelCommand in codeConfigurationDTO.LowLevelCommands)
			{
				this._context.Add(new DragonC.API.Models.Entities.LowLevelCommand
				{
					Name = lowLevelCommand.CommandName,
					MachineCode = lowLevelCommand.MachineCode,
					IsConditional = lowLevelCommand.IsConditionalCommand,
					ProjectId = codeConfigurationDTO.ProjectId
				});
			}

			foreach(UnformatedRule unformatedRule in codeConfigurationDTO.BaseFormalRules)
			{
				this._context.Add(new FormalRule
				{
					IsStart = unformatedRule.IsStart,
					Rule = unformatedRule.Rule,
					ProjectId = codeConfigurationDTO.ProjectId
				});
			}

			foreach(string codes in codeConfigurationDTO.HighLevelCommandsCode)
			{
				this._context.Add(new Models.Entities.HighLevelCommand
				{
					Code = codes,
					ProjectId = codeConfigurationDTO.ProjectId,
				});
			}

			this._context.SaveChanges();
		}

		public CodeConfigurationDTO Get(int Id)
		{
			List<string> tokens = (from tok in this._context.TokenSeparators
								   where tok.ProjectId == Id
								   select tok.Separator).ToList();

			List<DragonC.Domain.Compilator.LowLevelCommand> lowLevelCommands = (from llc in this._context.LowLevelCommands
													  where llc.ProjectId == Id
													  select new DragonC.Domain.Compilator.LowLevelCommand
													  {
														  CommandName = llc.Name,
														  IsConditionalCommand = llc.IsConditional,
														  MachineCode = llc.MachineCode,
													  }).ToList();
			List<String> highLevelCommands = (from hlc in this._context.HighLevelCommands
														where hlc.ProjectId == Id
														select hlc.Code).ToList();
			List<UnformatedRule> baseFormalRules = (from fru in this._context.FormalRules
													where fru.ProjectId == Id
													select new UnformatedRule
													{
														IsStart = fru.IsStart,
														Rule = fru.Rule
													}).ToList();
			return new CodeConfigurationDTO
			{
				TokenSeparators = tokens,
				LowLevelCommands = lowLevelCommands,
				BaseFormalRules = baseFormalRules,
				HighLevelCommandsCode = highLevelCommands
			};
		}

		public void Update(CodeConfigurationDTO codeConfigurationDTO)
		{
			List<TokenSeparator> tokens = (from tok in this._context.TokenSeparators
										   where tok.ProjectId == codeConfigurationDTO.ProjectId
										   select tok).ToList();
			List<Models.Entities.File> files = (from pro in this._context.Projects
												join fil in this._context.Files on pro.ProcessorFileId equals fil.Id
												where pro.Id == codeConfigurationDTO.ProjectId
												select fil).ToList();
			List<Models.Entities.HighLevelCommand> highLevelCommands = (from hig in this._context.HighLevelCommands
														where hig.ProjectId == codeConfigurationDTO.ProjectId
																		select hig).ToList();

			List<FormalRule> formalRules = (from fru in this._context.FormalRules
											where fru.ProjectId == codeConfigurationDTO.ProjectId
											select fru).ToList();

			List<Models.Entities.LowLevelCommand> lowLevelCommands = (from llc in this._context.LowLevelCommands
													  where llc.ProjectId == codeConfigurationDTO.ProjectId
																	  select llc).ToList();

			this._context.TokenSeparators.RemoveRange(tokens);
			this._context.Files.RemoveRange(files);
			this._context.HighLevelCommands.RemoveRange(highLevelCommands);
			this._context.FormalRules.RemoveRange(formalRules);
			this._context.LowLevelCommands.RemoveRange(lowLevelCommands);

			this.Create(codeConfigurationDTO);
		}
	}
}
