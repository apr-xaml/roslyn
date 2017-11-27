using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using _nIt.nRoslyn.SyntaxAnalyserExamples;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using nIt.nRoslyn;
using _nIt.nTestingFramework;
using System.Collections.Generic;
using nIt.nCommon;

namespace _nIt.nRoslyn
{
    [TestClass]
    public partial class UT_Computation_chain_analysis
    {

        private Maybe<ComputationGraphNode> _Assert_ReturnStatement(ComputationGraphNode node)
        {

            Assert.That.IsOfType<ReturnStatementSyntax>(node.Syntax);
            return node.OtherAntedecents.Value.Single();
        }


        private Maybe<ComputationGraphNode> _Assert_Identifier(ComputationGraphNode node, string variableName)
        {
            var ident = (IdentifierNameSyntax)node.Syntax;
            Assert.AreEqual(variableName, ident.Identifier.ToString());
            return node.OtherAntedecents.Value.Single();
        }

        private Maybe<ComputationGraphNode> _Assert_LocalDeclaration(ComputationGraphNode node, string variableName)
        {
            var localDecl = (LocalDeclarationStatementSyntax)node.Syntax;
            var varName = SyntaxOperations.GetVariableTokenSingle(localDecl).ToString();
            Assert.AreEqual(variableName, varName);
            return node.OtherAntedecents.Value.Single();
        }

        private Maybe<ComputationGraphNode> _Assert_CastExpression(ComputationGraphNode node, Type type, MethodBlockAnalysis methodAnalysis)
        {
            var cast = (CastExpressionSyntax)node.Syntax;

            var typeActual = methodAnalysis.SemanticModel.GetTypeInfo(cast.Type);

            var typeActualFullName = $"{typeActual.Type.ContainingNamespace}.{typeActual.Type.Name}";

            Assert.AreEqual(type.FullName, typeActualFullName);
            return node.OtherAntedecents.Value.Single();
        }

        private Maybe<IReadOnlyList<Maybe<ComputationGraphNode>>> _Assert_StaticInvocation(ComputationGraphNode node, string className, string methodName)
        {

            var invocSyntax = (InvocationExpressionSyntax)node.Syntax;

            var expr = invocSyntax.Expression;

            return node.OtherAntedecents;

        }

        private Maybe<ComputationGraphNode> _Assert_ParenthesizedExpr(ComputationGraphNode node)
        {
            var syntax = (ParenthesizedExpressionSyntax)node.Syntax;

            return node.OtherAntedecents.Value.Single();
        }


        private (Maybe<ComputationGraphNode> Left, Maybe<ComputationGraphNode> Right) _Assert_BinaryExpression(ComputationGraphNode node, SyntaxKind operatorToken)
        {
            var syntax = (BinaryExpressionSyntax)node.Syntax;

            var actualToken = syntax.OperatorToken;

            Assert.AreEqual(operatorToken, actualToken.Kind());

            var (left, right, rest) = node.OtherAntedecents.Value;

            return (left, right);
        }


        private void _Assert_Literal(ComputationGraphNode node, int value)
        {
            var syntax = (LiteralExpressionSyntax)node.Syntax;
            Assert.AreEqual(value, int.Parse(syntax.Token.Text));

            Assert.IsFalse(node.OtherAntedecents.Exists);
        }


        private void _Assert_IdentifierMethodParam(ComputationGraphNode node, string paramName, Type paramType, MethodBlockAnalysis methodAnalysis)
        {
            var syntax = (ParameterSyntax)node.Syntax;

            var expectedType = $"{paramType.Namespace}.{paramType.Name}";

            var typeInfo = methodAnalysis.SemanticModel.GetTypeInfo(syntax.Type);

            var actualType = $"{typeInfo.Type.ContainingNamespace}.{typeInfo.Type.Name}";

            Assert.AreEqual(paramName, syntax.Identifier.ToString());

            Assert.AreEqual(expectedType, actualType);
        }

    }
}
