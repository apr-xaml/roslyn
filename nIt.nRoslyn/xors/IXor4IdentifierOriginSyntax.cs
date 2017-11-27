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
    public interface IXor4IdentifierOriginSyntax : IXor4<LocalDeclarationStatementSyntax, PropertyDeclarationSyntax, FieldDeclarationSyntax, ParameterSyntax, SyntaxNode>
    {
        
    }

    public class Xor4IdentifierOriginSyntax : IXor4IdentifierOriginSyntax
    {
        Xor4<LocalDeclarationStatementSyntax, PropertyDeclarationSyntax, FieldDeclarationSyntax, ParameterSyntax, SyntaxNode> _innerXor4;

        public Xor4IdentifierOriginSyntax(LocalDeclarationStatementSyntax declaringSyntax)
        {
            _innerXor4 = declaringSyntax;
        }

        public Xor4IdentifierOriginSyntax(FieldDeclarationSyntax declaringSyntax1)
        {
            _innerXor4 = declaringSyntax1;
        }

        public Xor4IdentifierOriginSyntax(PropertyDeclarationSyntax declaringSyntax2)
        {
            _innerXor4 = declaringSyntax2;
        }

        public Xor4IdentifierOriginSyntax(ParameterSyntax declaringSyntax3)
        {
            _innerXor4 = declaringSyntax3;
        }

        public SyntaxNode Common => _innerXor4.Common;

        public bool IsA => _innerXor4.IsA;

        public bool IsB => _innerXor4.IsB;

        public bool IsC => _innerXor4.IsC;

        public bool IsD => _innerXor4.IsD;

        public LocalDeclarationStatementSyntax A => _innerXor4.A;

        public PropertyDeclarationSyntax B => _innerXor4.B;

        public FieldDeclarationSyntax C => _innerXor4.C;

        public ParameterSyntax D => _innerXor4.D;



        public Xor4Enum Kind => _innerXor4.Kind;


    
    }
}
