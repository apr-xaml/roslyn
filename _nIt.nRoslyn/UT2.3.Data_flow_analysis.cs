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

namespace _nIt.nRoslyn
{
    [TestClass]
    public partial class UT_Control_flow_analysis
    {


        [DeploymentItem(@"SyntaxAnalyserExamples/TestFunctions.cs", "SyntaxAnalyserExamples")]
        [TestMethod]
        public void Finds_variable_origins_to_be_local_declaration()
        {

            var codeAsText = File.ReadAllText(@"SyntaxAnalyserExamples/TestFunctions.cs");
            var syntaxTree = SyntaxOperations.Parse(codeAsText);
            var semanticModel = SemanticOperations.GetSemanticModel(syntaxTree);


            var methodSyntax = SyntaxOperations
                .FindMethodsOfClass(syntaxTree, nameof(TestFunctions), nameof(TestFunctions.DoublesParameter))
                .Value
                .Single();


            var methodAnalysis = MethodBlockAnalysis.FromSemanticModel(methodSyntax, semanticModel);

            var returnSyntax = methodAnalysis.BodyBlock.DescendantNodes().OfType<ReturnStatementSyntax>().Single();

            var identSyntax = SyntaxOperations.GetVariable(returnSyntax);


            var origin = SemanticOperations.GetIdentifierDeclarationSyntax(identSyntax, methodAnalysis).Value.A;

            var localDeclarSyntax = origin.A;

            var variableName = SyntaxOperations.GetVariableTokenSingle(localDeclarSyntax).ToString();

            Assert.AreEqual("res", variableName);
        }


        [DeploymentItem(@"SyntaxAnalyserExamples/TestFunctions.cs", "SyntaxAnalyserExamples")]
        [TestMethod]
        public void Finds_variable_origins_to_be_property_declaration()
        {

            var codeAsText = File.ReadAllText(@"SyntaxAnalyserExamples/TestFunctions.cs");
            var syntaxTree = SyntaxOperations.Parse(codeAsText);
            var semanticModel = SemanticOperations.GetSemanticModel(syntaxTree);


            var methodSyntax = SyntaxOperations
                .FindMethodsOfClass(syntaxTree, nameof(TestFunctions), nameof(TestFunctions.GetId))
                .Value
                .Single();


            var methodAnalysis = MethodBlockAnalysis.FromSemanticModel(methodSyntax, semanticModel);

            var returnSyntax = methodAnalysis.BodyBlock.DescendantNodes().OfType<ReturnStatementSyntax>().Single();

            var identSyntax = SyntaxOperations.GetVariable(returnSyntax);


            var origin = SemanticOperations.GetIdentifierDeclarationSyntax(identSyntax, methodAnalysis).Value.A;

            var propertyDeclarSyntax = origin.B;

            var propName = propertyDeclarSyntax.Identifier.ToString();

            Assert.AreEqual("Id", propName);
        }


        [DeploymentItem(@"SyntaxAnalyserExamples/TestFunctions.cs", "SyntaxAnalyserExamples")]
        [TestMethod]
        public void Finds_variable_origins_to_be_parameter_declaration()
        {

            var codeAsText = File.ReadAllText(@"SyntaxAnalyserExamples/TestFunctions.cs");
            var syntaxTree = SyntaxOperations.Parse(codeAsText);
            var semanticModel = SemanticOperations.GetSemanticModel(syntaxTree);


            var methodSyntax = SyntaxOperations
                .FindMethodsOfClass(syntaxTree, nameof(TestFunctions), nameof(TestFunctions.ReturnSameValue))
                .Value
                .Single();


            var methodAnalysis = MethodBlockAnalysis.FromSemanticModel(methodSyntax, semanticModel);

            var returnSyntax = methodAnalysis.BodyBlock.DescendantNodes().OfType<ReturnStatementSyntax>().Single();

            var identSyntax = SyntaxOperations.GetVariable(returnSyntax);


            var origin = SemanticOperations.GetIdentifierDeclarationSyntax(identSyntax, methodAnalysis).Value.A;

            var paramDeclarSyntax = origin.D;

            var paramName = paramDeclarSyntax.Identifier.ToString();

            Assert.AreEqual("x", paramName);
        }


        [DeploymentItem(@"SyntaxAnalyserExamples/TestFunctions.cs", "SyntaxAnalyserExamples")]
        [TestMethod]
        public void Throws_when_not_a_simple_data_flow()
        {

            var codeAsText = File.ReadAllText(@"SyntaxAnalyserExamples/TestFunctions.cs");
            var syntaxTree = SyntaxOperations.Parse(codeAsText);
            var semanticModel = SemanticOperations.GetSemanticModel(syntaxTree);


            var methodSyntax = SyntaxOperations
                .FindMethodsOfClass(syntaxTree, nameof(TestFunctions), nameof(TestFunctions.IncrementUntil10))
                .Value
                .Single();


            var methodAnalysis = MethodBlockAnalysis.FromSemanticModel(methodSyntax, semanticModel);

            var returnSyntax = methodAnalysis.BodyBlock.DescendantNodes().OfType<ReturnStatementSyntax>().Single();

            var identSyntax = SyntaxOperations.GetVariable(returnSyntax);


            var res = SemanticOperations.GetIdentifierDeclarationSyntax(identSyntax, methodAnalysis).Value;

            Assert.IsTrue(res.IsB);
        }




    }
}
