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
using nIt.nCommon;
using _nIt.nTestingFramework;

namespace _nIt.nRoslyn
{

    public partial class UT_Computation_chain_analysis
    {

        const string TEST_CATEGORY_COMPUTATION_GRAPH = "Computation graph";


        [DeploymentItem(@"SyntaxAnalyserExamples/TestFunctions.cs", "SyntaxAnalyserExamples")]
        [TestMethod, TestCategory(TEST_CATEGORY_COMPUTATION_GRAPH)]
        public void Creates_computation_chain()
        {

            var codeAsText = File.ReadAllText(@"SyntaxAnalyserExamples/TestFunctions.cs");
            var syntaxTree = SyntaxOperations.Parse(codeAsText);
            var semanticModel = SemanticOperations.GetSemanticModel(syntaxTree);


            var methodSyntax = SyntaxOperations
                .FindMethodsOfClass(syntaxTree, nameof(TestFunctions), nameof(TestFunctions.ProcessInt32Parameter))
                .Value
                .Single();

            var methodAnalysis = MethodBlockAnalysis.FromSemanticModel(methodSyntax, semanticModel);

            var returnSyntax = methodAnalysis.BodyBlock.DescendantNodes().OfType<ReturnStatementSyntax>().Single();

            var identSyntax = SyntaxOperations.GetVariable(returnSyntax);


            var chainHead = ComputationGraph.FromReturnStatement(returnSyntax, methodAnalysis, methodAnalysis.MethodDeclarationSyntax.Span).Item1.Value;

            var sqrtDoubledIncrementedInt = _Assert_ReturnStatement(chainHead).Value;
            var localDecl_sqrtDoubledIncrementedInt = _Assert_Identifier(sqrtDoubledIncrementedInt, "sqrtDoubledIncrementedInt").Value;

            var castToInt = _Assert_LocalDeclaration(localDecl_sqrtDoubledIncrementedInt, "sqrtDoubledIncrementedInt").Value;
            var sqrtDoubledIncremented = _Assert_CastExpression(castToInt, typeof(int), methodAnalysis).Value;
            var localDecl_sqrtDoubledIncremented = _Assert_Identifier(sqrtDoubledIncremented, "sqrtDoubledIncremented").Value;
            var sqrt = _Assert_LocalDeclaration(localDecl_sqrtDoubledIncremented, "sqrtDoubledIncremented").Value;
        
            var (mathClassSqrt , doubledIncremented, emptyRest) = _Assert_StaticInvocation(sqrt, nameof(Math), nameof(Math.Sqrt)).Value;
  
            CollectionAssert.That.IsEmpty(emptyRest);

            var localDecl_doubledIncremented = _Assert_Identifier(doubledIncremented.Value, "doubledIncremented").Value;
            var parenthesizedExprAdd = _Assert_LocalDeclaration(localDecl_doubledIncremented, "doubledIncremented").Value;

            var binary_add = _Assert_ParenthesizedExpr(parenthesizedExprAdd).Value;

            var (doubled, oneLiteral) = _Assert_BinaryExpression(binary_add, operatorToken: SyntaxKind.PlusToken);

            _Assert_Literal(oneLiteral.Value, 1);

            var localDecl_doubled = _Assert_Identifier(doubled.Value, "doubled").Value;


            var parenthesizedExprMult = _Assert_LocalDeclaration(localDecl_doubled, "doubled").Value;

            var binary_mult = _Assert_ParenthesizedExpr(parenthesizedExprMult).Value;

            var (twoLiteral, x) = _Assert_BinaryExpression(binary_mult, operatorToken: SyntaxKind.AsteriskToken);

            _Assert_Literal(twoLiteral.Value, 2);

            var methodParam = _Assert_Identifier(x.Value, "x").Value;

            _Assert_IdentifierMethodParam(methodParam, "x", typeof(int), methodAnalysis);

        }


        [DeploymentItem(@"SyntaxAnalyserExamples/TestFunctions.cs", "SyntaxAnalyserExamples")]
        [TestMethod, TestCategory(TEST_CATEGORY_COMPUTATION_GRAPH)]
        public void Creates_computation_chain_with_limited_scope()
        {
            var codeAsText = File.ReadAllText(@"SyntaxAnalyserExamples/TestFunctions.cs");
            var syntaxTree = SyntaxOperations.Parse(codeAsText);
            var semanticModel = SemanticOperations.GetSemanticModel(syntaxTree);


            var methodSyntax = SyntaxOperations
                .FindMethodsOfClass(syntaxTree, nameof(TestFunctions), nameof(TestFunctions.ProcessInt32Parameter))
                .Value
                .Single();



            var methodAnalysis = MethodBlockAnalysis.FromSemanticModel(methodSyntax, semanticModel);

            var returnSyntax = methodAnalysis.BodyBlock.DescendantNodes().OfType<ReturnStatementSyntax>().Single();

            var identSyntax = SyntaxOperations.GetVariable(returnSyntax);

            var doubleIncrementedIdentSyntax = SyntaxOperations.GetDelarationOfVariable(methodAnalysis.BodyBlock, "doubledIncremented");
            var searchSpan = TextSpan.FromBounds(doubleIncrementedIdentSyntax.SpanStart, returnSyntax.Span.End);

            var chainHead = ComputationGraph.FromReturnStatement(returnSyntax, methodAnalysis, searchSpan).Item1.Value;

            var sqrtDoubledIncrementedInt = _Assert_ReturnStatement(chainHead).Value;
            var localDecl_sqrtDoubledIncrementedInt = _Assert_Identifier(sqrtDoubledIncrementedInt, "sqrtDoubledIncrementedInt").Value;

            var castToInt = _Assert_LocalDeclaration(localDecl_sqrtDoubledIncrementedInt, "sqrtDoubledIncrementedInt").Value;
            var sqrtDoubledIncremented = _Assert_CastExpression(castToInt, typeof(int), methodAnalysis).Value;
            var localDecl_sqrtDoubledIncremented = _Assert_Identifier(sqrtDoubledIncremented, "sqrtDoubledIncremented").Value;
            var sqrt = _Assert_LocalDeclaration(localDecl_sqrtDoubledIncremented, "sqrtDoubledIncremented").Value;

            var doubledIncremented = _Assert_StaticInvocation(sqrt, nameof(Math), nameof(Math.Sqrt)).Value.Single().Value;

            var localDecl_doubledIncremented = _Assert_Identifier(doubledIncremented, "doubledIncremented").Value;
            var parenthesizedExprAdd = _Assert_LocalDeclaration(localDecl_doubledIncremented, "doubledIncremented").Value;

            var binary_add = _Assert_ParenthesizedExpr(parenthesizedExprAdd).Value;

            var (doubled, oneLiteral) = _Assert_BinaryExpression(binary_add, operatorToken: SyntaxKind.PlusToken);

            _Assert_Literal(oneLiteral.Value, 1);

            var localDecl_doubled = _Assert_Identifier(doubled.Value, "doubled");


            Assert.IsFalse(localDecl_doubled.Exists);

        }



        [DeploymentItem(@"SyntaxAnalyserExamples/TestFunctions.cs", "SyntaxAnalyserExamples")]
        [TestMethod, TestCategory(TEST_CATEGORY_COMPUTATION_GRAPH)]
        public void Creates_computation_chain_from_member_access_expr()
        {
            var codeAsText = File.ReadAllText(@"SyntaxAnalyserExamples/TestFunctions.cs");
            var syntaxTree = SyntaxOperations.Parse(codeAsText);
            var semanticModel = SemanticOperations.GetSemanticModel(syntaxTree);


            var methodSyntax = SyntaxOperations
                .FindMethodsOfClass(syntaxTree, nameof(TestFunctions), nameof(TestFunctions.ProcessStringParameter))
                .Value
                .Single();



            var methodAnalysis = MethodBlockAnalysis.FromSemanticModel(methodSyntax, semanticModel);

            var returnSyntax = methodAnalysis.BodyBlock.DescendantNodes().OfType<ReturnStatementSyntax>().Single();

            var chainHead = ComputationGraph.FromReturnStatement(returnSyntax, methodAnalysis, methodAnalysis.MethodDeclarationSyntax.Span).Item1.Value;

        }

    }
}
