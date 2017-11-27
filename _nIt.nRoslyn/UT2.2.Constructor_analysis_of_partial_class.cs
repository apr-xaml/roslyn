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

        const string TEST_CATEGORY_PARTIAL_CLASS_ANALYSIS = "Partial class analysis";


        [DeploymentItem(@"SyntaxAnalyserExamples/PartialClass.part2.cs", "SyntaxAnalyserExamples")]
        [TestMethod, TestCategory(TEST_CATEGORY_PARTIAL_CLASS_ANALYSIS)]
        public void Semantic_model_can_be_obtained_from_partial_definition_of_a_class_1()
        {

            var codeAsText2 = File.ReadAllText(@"SyntaxAnalyserExamples/PartialClass.part2.cs");
            var syntaxTree2 = SyntaxOperations.Parse(codeAsText2);
            var classDeclarationSyntax1 = SyntaxOperations.FindClass<PartialClass>(syntaxTree2).A;
            var semanticModel1 = SemanticOperations.GetSemanticModel(syntaxTree2);


            var twoArgsCtor = SyntaxOperations
                .FindConstructorsOf(classDeclarationSyntax1, semanticModel1, argsCount: 2)
                .Single();



            Assert.IsNotNull(twoArgsCtor);

        }




        [TestMethod, TestCategory(TEST_CATEGORY_PARTIAL_CLASS_ANALYSIS), DeploymentItem(@"SyntaxAnalyserExamples/PartialClass.part2.cs", "SyntaxAnalyserExamples")]
        public void Semantic_model_can_be_obtained_from_partial_definition_of_a_class_2()
        {



            var codeAsText2 = File.ReadAllText(@"SyntaxAnalyserExamples/PartialClass.part2.cs");
            var syntaxTree2 = SyntaxOperations.Parse(codeAsText2);
            var classDeclarationSyntax2 = SyntaxOperations.FindClass<PartialClass>(syntaxTree2).A;



            var semanticModel = SemanticOperations.GetSemanticModel(syntaxTree2, new[] { syntaxTree2 });


            var twoArgsCtor = SyntaxOperations
                    .FindConstructorsOf(classDeclarationSyntax2, semanticModel, argsCount: 2)
                    .Single();


            var (firstLink, isCompleted) = ChainOfConstructor.StartingFrom(twoArgsCtor, classDeclarationSyntax2, semanticModel);

            Assert.IsFalse(isCompleted);

            _Assert_IsIntermadiateLink(firstLink, classDeclarationSyntax2, twoArgsCtor);

        }
    }
}
