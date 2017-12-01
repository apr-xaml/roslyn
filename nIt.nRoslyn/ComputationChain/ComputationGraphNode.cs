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



    public class AbstractComputationGraphNode
    {

        public AbstractComputationGraphNode(SyntaxNode syntax, IXor2ComputationNodeReference specialAntedecent = null, IXor2ComputationNodeReference leftAntedecent = null, IXor2ComputationNodeReference rightAntedecent = null, IReadOnlyList<IXor2ComputationNodeReference> varyingAntedecents = null)
        {
            this.Syntax = syntax;
            this.LeftAntedecent = Maybe.FromNullableReference(leftAntedecent);
            this.RightAntedecent = Maybe.FromNullableReference(rightAntedecent);
            this.MainAntedecent = Maybe.FromNullableReference(specialAntedecent);
            this.OtherVaryingAntedecents = Maybe.FromNullableReference(varyingAntedecents);
        }

        public SyntaxNode Syntax { get; }


        public Maybe<IXor2ComputationNodeReference> MainAntedecent { get; }

        public Maybe<IXor2ComputationNodeReference> LeftAntedecent { get; }

        public Maybe<IXor2ComputationNodeReference> RightAntedecent { get; }
        public Maybe<IReadOnlyList<IXor2ComputationNodeReference>> OtherVaryingAntedecents { get; }
    }





    public class ComputationGraphNode : AbstractComputationGraphNode
    {

        private ComputationGraphNode(SyntaxNode leafSyntax) : base(leafSyntax)
        {

        }

        private ComputationGraphNode(SyntaxNode syntax, IXor2ComputationNodeReference specialAntedecent) : base(syntax, specialAntedecent)
        {

        }

        private ComputationGraphNode(SyntaxNode syntax, IXor2ComputationNodeReference specialAntedecent, IReadOnlyList<IXor2ComputationNodeReference> varyingAntedecents) : base(syntax, specialAntedecent, varyingAntedecents: varyingAntedecents)
        {

        }


        private ComputationGraphNode(SyntaxNode syntax, IXor2ComputationNodeReference leftAntedecent, IXor2ComputationNodeReference rightAntedecent) : base(syntax, leftAntedecent: leftAntedecent, rightAntedecent: rightAntedecent)
        {

        }

        private ComputationGraphNode(SyntaxNode syntax, IReadOnlyList<IXor2ComputationNodeReference> varyingAntedecents) : base(syntax, varyingAntedecents: varyingAntedecents)
        {

        }





        static public ComputationGraphNode FinalFromLiteral(LiteralExpressionSyntax literalSyntax)
            => new ComputationGraphNode(literalSyntax);


        static public ComputationGraphNode FromLocalDeclarationSyntax(LocalDeclarationStatementSyntax syntax, IXor2ComputationNodeReference previous)
         => new ComputationGraphNode(syntax, previous);

        static public ComputationGraphNode FromIdentifierSyntax(IdentifierNameSyntax syntax, Maybe<IXor2ComputationNodeReference> previous)
            => !previous.Exists ? new ComputationGraphNode(syntax) : new ComputationGraphNode(syntax, previous.Value);

        static public ComputationGraphNode FromCastSyntax(CastExpressionSyntax syntax, IXor2ComputationNodeReference previous)
            => new ComputationGraphNode(syntax, previous);

        static public ComputationGraphNode FromReturnStatement(ReturnStatementSyntax syntax, IXor2ComputationNodeReference previous)
            => new ComputationGraphNode(syntax, previous);

        static public ComputationGraphNode FinalFromPropertyDeclaration(PropertyDeclarationSyntax syntax)
            => new ComputationGraphNode(syntax);

        static public ComputationGraphNode FinalFromFieldDeclaration(FieldDeclarationSyntax syntax)
            => new ComputationGraphNode(syntax);

        static public ComputationGraphNode FinalFromParameter(ParameterSyntax syntax)
            => new ComputationGraphNode(syntax);

        static public ComputationGraphNode Binary(BinaryExpressionSyntax syntax, IXor2ComputationNodeReference leftAntedecent, IXor2ComputationNodeReference rightAntedecent)
        {
            return new ComputationGraphNode(syntax, leftAntedecent: leftAntedecent, rightAntedecent: rightAntedecent);
        }


        static public ComputationGraphNode FromObjectCreation(ObjectCreationExpressionSyntax syntax, IReadOnlyList<IXor2ComputationNodeReference> arguments)
            => new ComputationGraphNode(syntax, varyingAntedecents: arguments);

        static public ComputationGraphNode FromInvocation(InvocationExpressionSyntax syntax, IXor2ComputationNodeReference target, IReadOnlyList<IXor2ComputationNodeReference> arguments)
            => new ComputationGraphNode(syntax, target, arguments);

        static public ComputationGraphNode FromParenthesizedExprSyntax(ParenthesizedExpressionSyntax syntax, IXor2ComputationNodeReference previous)
            => new ComputationGraphNode(syntax, previous);

        static public ComputationGraphNode FromMemberAccess(MemberAccessExpressionSyntax syntax, IXor2ComputationNodeReference previous)
            => new ComputationGraphNode(syntax, previous, varyingAntedecents: Empty.List<IXor2ComputationNodeReference>());
    }
}
