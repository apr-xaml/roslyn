using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using nIt.nCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nIt.nRoslyn
{
    public interface IXor2BodyOfMethod : IXor2<BlockSyntax, IXor7ExpressionSyntax>
    {
        SyntaxNode Common { get; }
    }



    public class Xor2BodyOfMethod : IXor2BodyOfMethod
    {
        Xor2<BlockSyntax, IXor7ExpressionSyntax> _innerXor2;

        public Xor2BodyOfMethod(BlockSyntax blockSyntax)
        {
            _innerXor2 = blockSyntax;
        }


        public Xor2BodyOfMethod(IXor7ExpressionSyntax rightHandSideExpression)
        {
            _innerXor2 = new Xor2<BlockSyntax, IXor7ExpressionSyntax>(rightHandSideExpression);
        }

        public SyntaxNode Common => (IsA) ? (SyntaxNode)_innerXor2.A : _innerXor2.B.Common;

        public bool IsA => _innerXor2.IsA;

        public bool IsB => _innerXor2.IsB;

        public BlockSyntax A => _innerXor2.A;

        public IXor7ExpressionSyntax B => _innerXor2.B;

        public Xor2Enum Kind => _innerXor2.Kind;
    }
}
