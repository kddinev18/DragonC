namespace DragonC.Domain.Lexer
{
    public class TokenUnit
    {
        public string Token { get; set; }
        public int TextLine { get; set; }
        public int CodeLine { get; set; }
        public int StartCharaterPosition { get; set; }
        public int EndCharacterPosition { get; set; }
        public bool IsValid { get; set; }
        public List<string>? ErrorMessaes { get; set; }
        public int? StartCharacterOfErrorPosition { get; set; }

        public string GetErrors()
        {
            return string.Join(' ', ErrorMessaes) + $" Line of error: {TextLine}, character: {StartCharaterPosition + StartCharacterOfErrorPosition}";
        }

        public static bool operator ==(TokenUnit token1, TokenUnit token2)
        {
            if (token1 is null && token2 is null)
            {
                return true;
            }
            if (token1 is null || token2 is null)
            {
                return false;
            }
            return token1.Token == token2.Token &&
                token1.TextLine == token2.TextLine &&
                token1.StartCharaterPosition == token2.StartCharaterPosition;
        }

        public static bool operator !=(TokenUnit token1, TokenUnit token2)
        {
            if (token1 is null && token2 is null)
            {
                return false;
            }
            if (token1 is null || token2 is null)
            {
                return true;
            }
            return token1.Token != token2.Token ||
                token1.TextLine != token2.TextLine ||
                token1.StartCharaterPosition == token2.StartCharaterPosition;
        }
    }
}
