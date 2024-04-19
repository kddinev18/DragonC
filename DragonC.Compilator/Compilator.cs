using DragonC.Domain.Compilator;
using DragonC.Domain.Lexer;
using DragonC.Lexer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.Compilator
{
    public partial class Compilator
    {
        private FormalGrammar _formalGrammar;
        private Tokeniser _tokeniser;
        public Compilator()
        {
            _formalGrammar = new FormalGrammar(_commands);
            _formalGrammar.SetRules(_formalRules);
            _tokeniser = new Tokeniser(_tokenSeparators);
        }
        public CompiledCode Compile(string text)
        {
            List<TokenUnit> tokenUnits = new List<TokenUnit>();
            List<TokenUnit> syntaxCheckTokens = _formalGrammar.EvaluateTokens(tokenUnits);
            if (syntaxCheckTokens.Any(x => !x.IsValid))
            {
                return new CompiledCode()
                {
                    IsBuildSuccessfully = false,
                    ErrorMessages = syntaxCheckTokens.Where(x=>!x.IsValid).SelectMany(x=>x.ErrorMessaes).ToList()
                };
            }

            List<TokenUnit> literalTokens = tokenUnits
                .Where(x => x.Token.StartsWith("const") || x.Token.StartsWith("label"))
                .ToList();
            // make all instaces of labels and const to be surounded by other smbols to make it safer for replacement
            List<TokenUnit> commandTokens = tokenUnits
                .Where(x => !x.Token.StartsWith("const") && !x.Token.StartsWith("label"))
                .ToList();

        }
    }
}
