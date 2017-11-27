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


    public partial class UT_ConstructorAnalysis
    {

        const string TEST_CATEGORY_CONSTRUCTOR_ANALYSIS = "Constructor chain analysis";

        [TestMethod, TestCategory(TEST_CATEGORY_CONSTRUCTOR_ANALYSIS), DeploymentItem(@"SyntaxAnalyserExamples/MyEmptyClass.cs", "SyntaxAnalyserExamples")]
        public void Finds_implicit_constructor()
        {

            var codeAsText = File.ReadAllText(@"SyntaxAnalyserExamples/MyEmptyClass.cs");

            var syntaxTree = SyntaxOperations.Parse(codeAsText);

            var classDeclarationSyntax = SyntaxOperations.FindClass<MyEmptyClass>(syntaxTree).A;

            var semanticModel = SemanticOperations.GetSemanticModel(syntaxTree);

            var typeSymbol = SemanticOperations.FindClassSingle<MyEmptyClass>(syntaxTree, semanticModel).A;


            var anonymousConstructor = typeSymbol.Constructors.Single();

            Assert.AreEqual(actual: anonymousConstructor.MethodKind, expected: MethodKind.Constructor);
            Assert.IsTrue(anonymousConstructor.IsImplicitlyDeclared);

        }




        [TestMethod, TestCategory(TEST_CATEGORY_CONSTRUCTOR_ANALYSIS), DeploymentItem(@"SyntaxAnalyserExamples/ClassFamily.cs", "SyntaxAnalyserExamples")]
        public void List_chain_of_constructors_1()
        {

            var codeAsText = File.ReadAllText(@"SyntaxAnalyserExamples/ClassFamily.cs");

            var syntaxTree = SyntaxOperations.Parse(codeAsText);
            var classDeclarationSyntax = SyntaxOperations.FindClass<BaseUser>(syntaxTree).A;

            var semanticModel = SemanticOperations.GetSemanticModel(syntaxTree);

            var zeroArgsCtor = SyntaxOperations
                .FindConstructorsOf(classDeclarationSyntax, semanticModel, argsCount: 0)
                .Single();

            var (firstAndOnlyLink, isCompleted) = ChainOfConstructor.StartingFrom(zeroArgsCtor, classDeclarationSyntax, semanticModel);


            Assert.IsTrue(isCompleted);
            _Assert_IsLastLink(firstAndOnlyLink, classDeclarationSyntax, zeroArgsCtor);

        }

        [TestMethod, TestCategory(TEST_CATEGORY_CONSTRUCTOR_ANALYSIS), DeploymentItem(@"SyntaxAnalyserExamples/ClassFamily.cs", "SyntaxAnalyserExamples")]
        public void List_chain_of_constructors_2()
        {

            var codeAsText = File.ReadAllText(@"SyntaxAnalyserExamples/ClassFamily.cs");

            var syntaxTree = SyntaxOperations.Parse(codeAsText);
            var classDeclarationSyntax = SyntaxOperations.FindClass<BaseUser>(syntaxTree).A;

            var semanticModel = SemanticOperations.GetSemanticModel(syntaxTree);


            var zeroArgsCtor = SyntaxOperations
                .FindConstructorsOf(classDeclarationSyntax, semanticModel, argsCount: 0)
                .Single();


            var oneArgCtor = SyntaxOperations
                .FindConstructorsOf(classDeclarationSyntax, semanticModel, argsCount: 1)
                .Single();

            var (firstLink, isCompleted) = ChainOfConstructor.StartingFrom(oneArgCtor, classDeclarationSyntax, semanticModel);

            Assert.IsTrue(isCompleted);

            _Assert_IsIntermadiateLink(firstLink, classDeclarationSyntax, oneArgCtor);

            var secondLink = firstLink.NextLink.Value;
            _Assert_IsLastLink(secondLink, classDeclarationSyntax, zeroArgsCtor);

        }




        [TestMethod, TestCategory(TEST_CATEGORY_CONSTRUCTOR_ANALYSIS), DeploymentItem(@"SyntaxAnalyserExamples/ClassFamily.cs", "SyntaxAnalyserExamples")]
        public void List_chain_of_constructors_3()
        {

            var codeAsText = File.ReadAllText(@"SyntaxAnalyserExamples/ClassFamily.cs");

            var syntaxTree = SyntaxOperations.Parse(codeAsText);

            var classExtUserDeclarationSyntax = SyntaxOperations.FindClass<ExtendedUser>(syntaxTree).A;
            var classNormalUserDeclarationSyntax = SyntaxOperations.FindClass<NormalUser>(syntaxTree).A;
            var classBaseUserDeclarationSyntax = SyntaxOperations.FindClass<BaseUser>(syntaxTree).A;

            var semanticModel = SemanticOperations.GetSemanticModel(syntaxTree);




            var threeArgsCtor = SyntaxOperations
                .FindConstructorsOf(classExtUserDeclarationSyntax, semanticModel, argsCount: 3)
                .Single();

            var (firstLink, isCompleted) = ChainOfConstructor.StartingFrom(threeArgsCtor, classExtUserDeclarationSyntax, semanticModel);

            var secondLink = firstLink.NextLink.Value;
            var thirdLink = secondLink.NextLink.Value;
            var lastlink = thirdLink.NextLink.Value;

            Assert.IsTrue(isCompleted);

            _Assert_IsIntermadiateLink(firstLink, classExtUserDeclarationSyntax, threeArgsCtor);

            _Assert_IsIntermadiateLink(secondLink, classNormalUserDeclarationSyntax, SyntaxOperations.FindConstructorsOf(classNormalUserDeclarationSyntax, semanticModel, 3).Single());

            _Assert_IsIntermadiateLink(thirdLink, classNormalUserDeclarationSyntax, SyntaxOperations.FindConstructorsOf(classNormalUserDeclarationSyntax, semanticModel, 2).Single());

            _Assert_IsLastLink(lastlink, classBaseUserDeclarationSyntax, SyntaxOperations.FindConstructorsOf(classBaseUserDeclarationSyntax, semanticModel, 0).Single());


        }



    }
}
