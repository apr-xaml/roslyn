using Microsoft.CodeAnalysis.CSharp.Syntax;
using nIt.nCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace nIt.nRoslyn
{
    public interface IXor7ExpressionSyntax : IXor7<ParenthesizedExpressionSyntax, InvocationExpressionSyntax, BinaryExpressionSyntax, CastExpressionSyntax, MemberAccessExpressionSyntax, IdentifierNameSyntax, LiteralExpressionSyntax, ExpressionSyntax>
    {
        
    }

    public class Xor7ExpressionSyntax : IXor7ExpressionSyntax
    {
        private Xor7<ParenthesizedExpressionSyntax, InvocationExpressionSyntax, BinaryExpressionSyntax, CastExpressionSyntax, MemberAccessExpressionSyntax, IdentifierNameSyntax, LiteralExpressionSyntax, ExpressionSyntax> _innerXor7;


        public Xor7ExpressionSyntax(ParenthesizedExpressionSyntax syntax)
        {
            _innerXor7 = syntax;
        }

        public Xor7ExpressionSyntax(InvocationExpressionSyntax syntax)
        {
            _innerXor7 = syntax;
        }


        public Xor7ExpressionSyntax(BinaryExpressionSyntax syntax)
        {
            _innerXor7 = syntax;
        }

        public Xor7ExpressionSyntax(MemberAccessExpressionSyntax syntax)
        {
            _innerXor7 = syntax;
        }

        public Xor7ExpressionSyntax(IdentifierNameSyntax syntax)
        {
            _innerXor7 = syntax;
        }

        public Xor7ExpressionSyntax(LiteralExpressionSyntax syntax)
        {
            _innerXor7 = syntax;
        }

        public Xor7ExpressionSyntax(CastExpressionSyntax castExprSyntax)
        {
            _innerXor7 = castExprSyntax;
        }

        public ExpressionSyntax Common => _innerXor7.Common;

        public bool IsA => _innerXor7.IsA;

        public bool IsB => _innerXor7.IsB;

        public bool IsC => _innerXor7.IsC;

        public bool IsD => _innerXor7.IsD;

        public bool IsE => _innerXor7.IsE;

        public bool IsF => _innerXor7.IsF;

        public bool IsG => _innerXor7.IsG;

        public ParenthesizedExpressionSyntax A => _innerXor7.A;
        public InvocationExpressionSyntax B => _innerXor7.B;

        public BinaryExpressionSyntax C => _innerXor7.C;

        public CastExpressionSyntax D => _innerXor7.D;

        public MemberAccessExpressionSyntax E => _innerXor7.E;

        public IdentifierNameSyntax F => _innerXor7.F;

        public LiteralExpressionSyntax G => _innerXor7.G;

        public Xor7Enum Kind => _innerXor7.Kind;

        static public Xor7ExpressionSyntax FromExpression(SyntaxNode syntaxueSyntax)
        {
            switch (syntaxueSyntax)
            {
                case ParenthesizedExpressionSyntax parenthesizedExprSyntax:
                    {
                        return new Xor7ExpressionSyntax(parenthesizedExprSyntax);
                    }
                case InvocationExpressionSyntax invoSyntax:
                    {
                        return new Xor7ExpressionSyntax(invoSyntax);
                    }
                case BinaryExpressionSyntax binaryExprSyntax:
                    {
                        return new Xor7ExpressionSyntax(binaryExprSyntax);
                    }
                case CastExpressionSyntax castExprSyntax:
                    {
                        return new Xor7ExpressionSyntax(castExprSyntax);
                    }
                case MemberAccessExpressionSyntax memberAccessExpr:
                    {
                        return new Xor7ExpressionSyntax(memberAccessExpr);
                    }
                case IdentifierNameSyntax identifierSyntax:
                    {
                        return new Xor7ExpressionSyntax(identifierSyntax);
                    }
                case LiteralExpressionSyntax literalSyntax:
                    {
                        return new Xor7ExpressionSyntax(literalSyntax);
                    }
                case ArgumentSyntax argSyntax:
                    {
                        throw NotPreparedForThatCase.CannotHappenException;
                    }
                default:
                    throw NotPreparedForThatCase.UnexpectedTypeException(syntaxueSyntax.GetType());

            }
        }

    }
}
