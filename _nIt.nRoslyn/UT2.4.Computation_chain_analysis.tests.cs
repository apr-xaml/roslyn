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


            var chainHead = ComputationGraph.FromReturnStatement(returnSyntax, methodAnalysis, methodAnalysis.MethodDeclarationSyntax.Span).Item1.A;

            var sqrtDoubledIncrementedInt = _Assert_ReturnStatement(chainHead).A;
            var localDecl_sqrtDoubledIncrementedInt = _Assert_Identifier(sqrtDoubledIncrementedInt, "sqrtDoubledIncrementedInt").A;

            var castToInt = _Assert_LocalDeclaration(localDecl_sqrtDoubledIncrementedInt, "sqrtDoubledIncrementedInt").A;
            var sqrtDoubledIncremented = _Assert_CastExpression(castToInt, typeof(int), methodAnalysis).A;
            var localDecl_sqrtDoubledIncremented = _Assert_Identifier(sqrtDoubledIncremented, "sqrtDoubledIncremented").A;
            var sqrt = _Assert_LocalDeclaration(localDecl_sqrtDoubledIncremented, "sqrtDoubledIncremented").A;

            var (mathClassSqrt, args_doubledIncremented) = _Assert_Invocation(sqrt, nameof(Math.Sqrt));



            var localDecl_doubledIncremented = _Assert_Identifier(args_doubledIncremented.Single().A, "doubledIncremented").A;
            var parenthesizedExprAdd = _Assert_LocalDeclaration(localDecl_doubledIncremented, "doubledIncremented").A;

            var binary_add = _Assert_ParenthesizedExpr(parenthesizedExprAdd).A;

            var (doubled, oneLiteral) = _Assert_BinaryExpression(binary_add, operatorToken: SyntaxKind.PlusToken);

            _Assert_Literal(oneLiteral.A, 1);

            var localDecl_doubled = _Assert_Identifier(doubled.A, "doubled").A;


            var parenthesizedExprMult = _Assert_LocalDeclaration(localDecl_doubled, "doubled").A;

            var binary_mult = _Assert_ParenthesizedExpr(parenthesizedExprMult).A;

            var (twoLiteral, x) = _Assert_BinaryExpression(binary_mult, operatorToken: SyntaxKind.AsteriskToken);

            _Assert_Literal(twoLiteral.A, 2);

            var methodParam = _Assert_Identifier(x.A, "x").A;

            _Assert_MethodParam(methodParam, "x", typeof(int), methodAnalysis);

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

            var chainHead = ComputationGraph.FromReturnStatement(returnSyntax, methodAnalysis, searchSpan).Item1.A;

            var sqrtDoubledIncrementedInt = _Assert_ReturnStatement(chainHead).A;
            var localDecl_sqrtDoubledIncrementedInt = _Assert_Identifier(sqrtDoubledIncrementedInt, "sqrtDoubledIncrementedInt").A;

            var castToInt = _Assert_LocalDeclaration(localDecl_sqrtDoubledIncrementedInt, "sqrtDoubledIncrementedInt").A;
            var sqrtDoubledIncremented = _Assert_CastExpression(castToInt, typeof(int), methodAnalysis).A;
            var localDecl_sqrtDoubledIncremented = _Assert_Identifier(sqrtDoubledIncremented, "sqrtDoubledIncremented").A;
            var sqrt = _Assert_LocalDeclaration(localDecl_sqrtDoubledIncremented, "sqrtDoubledIncremented").A;

            var (mathSqrt, args_doubledIncremented) = _Assert_Invocation(sqrt, nameof(Math.Sqrt));

            var localDecl_doubledIncremented = _Assert_Identifier(args_doubledIncremented.Single().A, "doubledIncremented").A;
            var parenthesizedExprAdd = _Assert_LocalDeclaration(localDecl_doubledIncremented, "doubledIncremented").A;

            var binary_add = _Assert_ParenthesizedExpr(parenthesizedExprAdd).A;

            var (doubled, oneLiteral) = _Assert_BinaryExpression(binary_add, operatorToken: SyntaxKind.PlusToken);

            _Assert_Literal(oneLiteral.A, 1);

            var localDecl_doubled = _Assert_Identifier(doubled.A, "doubled");


            Assert.IsFalse(localDecl_doubled.IsA);

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

            var chainHead = ComputationGraph.FromReturnStatement(returnSyntax, methodAnalysis, methodAnalysis.MethodDeclarationSyntax.Span).Item1.A;


            var propAccess_Name = _Assert_ReturnStatement(chainHead);
            var invoc_GetType = _Assert_MemberAccess(propAccess_Name, nameof(Type.Name)).A;
            var (memberAccess_GetType, args) = _Assert_Invocation(invoc_GetType, nameof(object.GetType));

            CollectionAssert.That.IsEmpty(args);

            var invoc_ElementAt =  _Assert_MemberAccess(memberAccess_GetType, nameof(object.GetType)).A;

            var (memberAccess_ElementAt, elementAt_Args) = _Assert_Invocation(invoc_ElementAt, "ElementAt");

            _Assert_Literal(elementAt_Args.Single().A, 2);


            var ident_x =  _Assert_MemberAccess(memberAccess_ElementAt, "ElementAt").A;

            var methodParam_x = _Assert_Identifier(ident_x, "x").A;

            _Assert_MethodParam(methodParam_x, "x", typeof(string), methodAnalysis);

        }




        [DeploymentItem(@"SyntaxAnalyserExamples/TestFunctions.cs", "SyntaxAnalyserExamples")]
        [TestMethod, TestCategory(TEST_CATEGORY_COMPUTATION_GRAPH)]
        public void Creates_computation_chain_ramifying_code()
        {
            var codeAsText = File.ReadAllText(@"SyntaxAnalyserExamples/TestFunctions.cs");
            var syntaxTree = SyntaxOperations.Parse(codeAsText);
            var semanticModel = SemanticOperations.GetSemanticModel(syntaxTree);


            var methodSyntax = SyntaxOperations
                .FindMethodsOfClass(syntaxTree, nameof(TestFunctions), nameof(TestFunctions.ProcessInt32arameterWithRamification))
                .Value
                .Single();



            var methodAnalysis = MethodBlockAnalysis.FromSemanticModel(methodSyntax, semanticModel);

            var returnSyntax = methodAnalysis.BodyBlock.DescendantNodes().OfType<ReturnStatementSyntax>().Single();

            var chainHead = ComputationGraph.FromReturnStatement(returnSyntax, methodAnalysis, methodAnalysis.MethodDeclarationSyntax.Span).Item1.A;


         

        }


    }
}
