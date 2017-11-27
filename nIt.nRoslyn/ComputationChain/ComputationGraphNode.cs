using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using nIt.nCommon;
using nItCIT.nCommon.nFSharp;

namespace nIt.nRoslyn
{
    public class ComputationGraphNode
    {

        public SyntaxNode Syntax { get; }


        public Maybe<IXor2ComputationNodeReference> SpecialAntedecent { get; }
        public Maybe<IReadOnlyList<IXor2ComputationNodeReference>> OtherAntedecents { get; }


        private ComputationGraphNode(SyntaxNode syntax, IXor2ComputationNodeReference specialAntedecent, IReadOnlyList<IXor2ComputationNodeReference> otherAntedecents)
        {
            this.Syntax = syntax;
            this.SpecialAntedecent = Maybe.FromReference(specialAntedecent);
            this.OtherAntedecents = Maybe.FromReference(otherAntedecents);
        }


        private ComputationGraphNode(SyntaxNode syntax, IReadOnlyList<IXor2ComputationNodeReference> otherAntedecents)
        {
            this.Syntax = syntax;
            this.SpecialAntedecent = Maybe.NoValue;
            this.OtherAntedecents = Maybe.FromReference(otherAntedecents);
        }


        private ComputationGraphNode(SyntaxNode syntax)
        {
            this.Syntax = syntax;
            this.SpecialAntedecent = Maybe.NoValue;
            this.OtherAntedecents = Maybe.NoValue;
        }




        static public ComputationGraphNode FinalFromLiteral(LiteralExpressionSyntax literalSyntax)
            => new ComputationGraphNode(literalSyntax);


        static public ComputationGraphNode FromLocalDeclarationSyntax(LocalDeclarationStatementSyntax syntax, IXor2ComputationNodeReference previous)
         => new ComputationGraphNode(syntax, previous.IntoList());

        static public ComputationGraphNode FromIdentifierSyntax(IdentifierNameSyntax syntax, IXor2ComputationNodeReference previous)
            => new ComputationGraphNode(syntax, previous.IntoList());

        static public ComputationGraphNode FromCastSyntax(CastExpressionSyntax syntax, IXor2ComputationNodeReference previous)
            => new ComputationGraphNode(syntax, previous.IntoList());

        static public ComputationGraphNode FromReturnStatement(ReturnStatementSyntax syntax, IXor2ComputationNodeReference previous)
            => new ComputationGraphNode(syntax, previous.IntoList());

        static public ComputationGraphNode FinalFromPropertyDeclaration(PropertyDeclarationSyntax syntax)
            => new ComputationGraphNode(syntax);

        static public ComputationGraphNode FinalFromFieldDeclaration(FieldDeclarationSyntax syntax)
            => new ComputationGraphNode(syntax);

        static public ComputationGraphNode FinalFromParameter(ParameterSyntax syntax)
            => new ComputationGraphNode(syntax);

        static public ComputationGraphNode Binary(BinaryExpressionSyntax syntax, IXor2ComputationNodeReference previousLeft, IXor2ComputationNodeReference previousRight)
        {
            var previous = new[] { previousLeft, previousRight }.ToList();
            return new ComputationGraphNode(syntax, previous);
        }


        static public ComputationGraphNode FromObjectCreation(ObjectCreationExpressionSyntax syntax, IReadOnlyList<IXor2ComputationNodeReference> arguments)
            => new ComputationGraphNode(syntax, arguments);

        static public ComputationGraphNode FromInvocation(InvocationExpressionSyntax syntax, IXor2ComputationNodeReference target, IReadOnlyList<IXor2ComputationNodeReference> arguments)
            => new ComputationGraphNode(syntax, target, arguments);

        static public ComputationGraphNode FromParenthesizedExprSyntax(ParenthesizedExpressionSyntax syntax, IXor2ComputationNodeReference previous)
            => new ComputationGraphNode(syntax, previous.IntoList());

        static public ComputationGraphNode FromMemberAccess(MemberAccessExpressionSyntax syntax, IXor2ComputationNodeReference previous)
            => new ComputationGraphNode(syntax, previous.IntoList());
    }
}
