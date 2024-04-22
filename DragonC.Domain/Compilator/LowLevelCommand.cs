namespace DragonC.Domain.Compilator
{
    public class LowLevelCommand
    {
        public string CommandName { get; set; }
        public bool IsConditionalCommand { get; set; } = false;
        public string MachineCode { get; set; }
    }
}
