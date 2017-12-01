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
    public class UnitTest1
    {
        [TestMethod, DeploymentItem(@"SyntaxAnalyserExamples/ClientDto.cs", "SyntaxAnalyserExamples")]
        public void Creates_semantic_model_of_a_class()
        {
            var classToAnalyze = typeof(ClientDto);

            var codeAsText = File.ReadAllText(@"SyntaxAnalyserExamples/ClientDto.cs");

            var syntaxTree = SyntaxOperations.Parse(codeAsText);
            var classDeclarationSyntax = SyntaxOperations.FindClass<ClientDto>(syntaxTree);

            var semanticModel = SemanticOperations.GetSemanticModel(syntaxTree);

            var typeSymbol = SemanticOperations.FindClassSingle<ClientDto>(syntaxTree, semanticModel).A;

            Assert.AreEqual(classToAnalyze.Name, typeSymbol.Name);

        }



        [TestMethod, DeploymentItem(@"SyntaxAnalyserExamples/PersonObject.cs", "SyntaxAnalyserExamples")]
        public void Finds_all_static_methods()
        {
            var classToAnalyze = typeof(PersonObject);

            var codeAsText = File.ReadAllText(@"SyntaxAnalyserExamples/PersonObject.cs");

            var syntaxTree = SyntaxOperations.Parse(codeAsText);
            var classDeclarationSyntax = SyntaxOperations.FindClass<PersonObject>(syntaxTree);

            var semanticModel = SemanticOperations.GetSemanticModel(syntaxTree);

            var typeSymbol = SemanticOperations.FindClassSingle<PersonObject>(syntaxTree, semanticModel).A;


            var members = typeSymbol.GetMembers();

            var methods = members.Where(x => x.Kind == SymbolKind.Method).Cast<IMethodSymbol>().ToList();

            var staticMethod = methods.Where(x => x.IsStatic).Where(x => x.Name == nameof(PersonObject.AreEquivalent)).Single();

            Assert.AreEqual(nameof(PersonObject.AreEquivalent), staticMethod.Name);

        }



        [TestMethod, DeploymentItem(@"SyntaxAnalyserExamples/PersonObject.cs", "SyntaxAnalyserExamples")]
        public void Finds_all_instance_methods()
        {
            var classToAnalyze = typeof(PersonObject);

            var codeAsText = File.ReadAllText(@"SyntaxAnalyserExamples/PersonObject.cs");

            var syntaxTree = SyntaxOperations.Parse(codeAsText);
            var classDeclarationSyntax = SyntaxOperations.FindClass<PersonObject>(syntaxTree);

            var semanticModel = SemanticOperations.GetSemanticModel(syntaxTree);

            var typeSymbol = SemanticOperations.FindClassSingle<PersonObject>(syntaxTree, semanticModel).A;


            var members = typeSymbol.GetMembers();

            var methods = members.Where(x => x.Kind == SymbolKind.Method).Cast<IMethodSymbol>().ToList();

            var staticMethod = methods.Where(x => !x.IsStatic && x.MethodKind == MethodKind.Ordinary).Single();

            Assert.AreEqual(nameof(PersonObject.AddChild), staticMethod.Name);

        }



        [TestMethod, DeploymentItem(@"SyntaxAnalyserExamples/PersonObject.cs", "SyntaxAnalyserExamples")]
        public void Finds_all_readonly_properties()
        {
            var classToAnalyze = typeof(PersonObject);

            var codeAsText = File.ReadAllText(@"SyntaxAnalyserExamples/PersonObject.cs");

            var syntaxTree = SyntaxOperations.Parse(codeAsText);

            var classDeclarationSyntax = SyntaxOperations.FindClass<PersonObject>(syntaxTree).A;

            var semanticModel = SemanticOperations.GetSemanticModel(syntaxTree);

            var typeSymbol = SemanticOperations.FindClassSingle<PersonObject>(syntaxTree, semanticModel).A;


            var members = typeSymbol.GetMembers();

            var props = members.Where(x => x.Kind == SymbolKind.Property).Cast<IPropertySymbol>().ToList();

            var readOnlyProps = props.Where(x => x.IsReadOnly).ToList();

            CollectionAssert.AreEquivalent(new[] { nameof(PersonObject.Id), nameof(PersonObject.Name) }, readOnlyProps.Select(x => x.Name).ToArray());

        }

        [TestMethod, DeploymentItem(@"SyntaxAnalyserExamples/PartialClass.part1.cs", "SyntaxAnalyserExamples"), DeploymentItem(@"SyntaxAnalyserExamples/PartialClass.part2.cs", "SyntaxAnalyserExamples")]
        public void Partial_class_symbol_has_many_syntax_references_depending_on_source_syntax_tree_it_was_obtained()
        {

            var codeAsText1 = File.ReadAllText(@"SyntaxAnalyserExamples/PartialClass.part1.cs");
            var syntaxTree1 = SyntaxOperations.Parse(codeAsText1);
            var classDeclarationSyntax1 = SyntaxOperations.FindClass<PartialClass>(syntaxTree1).A;


            var codeAsText2 = File.ReadAllText(@"SyntaxAnalyserExamples/PartialClass.part2.cs");
            var syntaxTree2 = SyntaxOperations.Parse(codeAsText2);
            var classDeclarationSyntax2 = SyntaxOperations.FindClass<PartialClass>(syntaxTree2).A;



            var semanticModel = SemanticOperations.GetSemanticModel(syntaxTree1, new[] { syntaxTree1, syntaxTree2 });

            var classSymbol1 = semanticModel.GetDeclaredSymbol(classDeclarationSyntax1);
            var classSymbol2 = semanticModel.GetDeclaredSymbol(classDeclarationSyntax1);

            Assert.AreEqual(classSymbol1, classSymbol2);

            var declaringSyntaxRefs1 = classSymbol1.DeclaringSyntaxReferences;
            var declaringSyntaxRefs2 = classSymbol2.DeclaringSyntaxReferences;

            CollectionAssert.AreEquivalent(declaringSyntaxRefs1, declaringSyntaxRefs2);

            Assert.AreEqual(2, declaringSyntaxRefs1.Length);

        }


        [TestMethod]
        public void Computes_value_of_int32_literal()
        {

            var codeAsText = @"var x = 1;";
            var syntaxTree = SyntaxOperations.Parse(codeAsText);
            var fieldSyntax = syntaxTree.GetRoot().ChildNodes().OfType<FieldDeclarationSyntax>().Single();

            var semantics = SemanticOperations.GetSemanticModel(syntaxTree);


            var literalSyntax = fieldSyntax.DescendantNodes().OfType<LiteralExpressionSyntax>().Single();

            var literalVal = (int)semantics.GetConstantValue(literalSyntax).Value;

            Assert.AreEqual(1, literalVal);
        }

        [TestMethod]
        public void Computes_value_of_string_literal()
        {

            var codeAsText = @"var x = ""konik"";";
            var syntaxTree = SyntaxOperations.Parse(codeAsText);
            var fieldSyntax = syntaxTree.GetRoot().ChildNodes().OfType<FieldDeclarationSyntax>().Single();

            var semantics = SemanticOperations.GetSemanticModel(syntaxTree);


            var literalSyntax = fieldSyntax.DescendantNodes().OfType<LiteralExpressionSyntax>().Single();

            var literalVal = (string)semantics.GetConstantValue(literalSyntax).Value;

            Assert.AreEqual("konik", literalVal);
        }


    }
}
