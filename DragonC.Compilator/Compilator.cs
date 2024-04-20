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
            List<TokenUnit> tokenUnits = _formalGrammar.EvaluateTokens(_tokeniser.GetTokens(text));
            if (tokenUnits.Any(x => !x.IsValid))
            {
                return new CompiledCode()
                {
                    IsBuildSuccessfully = false,
                    ErrorMessages = tokenUnits.Where(x=>!x.IsValid).SelectMany(x=>x.ErrorMessaes).ToList()
                };
            }
            int codeLines = LoadCodeLines(tokenUnits);

            List<TokenUnit> literalTokens = tokenUnits
                .Where(x => x.Token.StartsWith("const") || x.Token.StartsWith("label"))
                .ToList();
            // make all instaces of labels and const to be surounded by other smbols to make it safer for replacement
            List<TokenUnit> commandTokens = tokenUnits
                .Where(x => !x.Token.StartsWith("const") && !x.Token.StartsWith("label"))
                .ToList();

            ReplaceDybamicValuesWithCorrespodingValue(literalTokens, commandTokens);

            return null;
        }

        private void ReplaceDybamicValuesWithCorrespodingValue(List<TokenUnit> literalTokens, List<TokenUnit> commandTokens)
        {
            foreach (TokenUnit commandToken in commandTokens)
            {
                if(literalTokens.Where(x => commandToken.Token.Contains(x.Token)).Any())
                {

                }
            }
        }

        private int LoadCodeLines(List<TokenUnit> tokenUnits)
        {
            int codeLines = 0;
            foreach (TokenUnit token in tokenUnits)
            {
                if(!token.Token.StartsWith("const") && !token.Token.StartsWith("label"))
                {
                    token.CodeLine = ++codeLines;
                }
            }

            return codeLines;
        }
    }
}
