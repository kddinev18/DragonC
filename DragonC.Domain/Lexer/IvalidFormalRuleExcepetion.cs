using System.Runtime.Serialization;

namespace DragonC.Domain.Lexer
{
    [Serializable]
    public class IvalidFormalRuleExcepetion : Exception
    {
        public IvalidFormalRuleExcepetion()
        {
        }

        public IvalidFormalRuleExcepetion(string? message) : base(message)
        {
        }

        public IvalidFormalRuleExcepetion(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected IvalidFormalRuleExcepetion(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}