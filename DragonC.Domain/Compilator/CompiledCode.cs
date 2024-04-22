namespace DragonC.Domain.Compilator
{
    public class CompiledCode
    {
        public bool IsBuildSuccessfully { get; set; }
        public List<string>? CompiledCommands { get; set; }
        public List<string>? InterMediateCommands { get; set; }
        public List<string>? ErrorMessages { get; set; }
    }
}
