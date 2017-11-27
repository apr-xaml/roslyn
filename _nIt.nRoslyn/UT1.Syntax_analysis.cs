using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace _nIt.nRoslyn
{
    [TestClass]
    public class Syntax_analysis
    {
        [TestMethod]
        public void Can_parse_empty_class_definition()
        {
            var myClassDef = "class MyClass {}";

            var tree = CSharpSyntaxTree.ParseText(myClassDef);

            var text = tree.GetText();

            Assert.AreEqual(myClassDef, text.ToString());
        }



        [TestMethod]
        public void Cannot_parse_faulty_code_1()
        {
            var myClassDef = "pasta";

            var tree = CSharpSyntaxTree.ParseText(myClassDef);

            var diagnostics = tree.GetDiagnostics().ToList();

            var namespaceCannotContainDirect = diagnostics.Single();

            Assert.AreEqual(namespaceCannotContainDirect.Severity, DiagnosticSeverity.Error);

        }


        [TestMethod]
        public void Can_find_class_declaration()
        {
            var myClassDef = "class MyClass {}";

            var tree = CSharpSyntaxTree.ParseText(myClassDef);

            var root = tree.GetRoot();

            var node = root
                .ChildNodes().Where(x => x.Kind() == SyntaxKind.ClassDeclaration)        
                .Single();

            var classDeclaration = (ClassDeclarationSyntax)node;

            var name = classDeclaration.Identifier.ToString();
            var nameWithTrivia = classDeclaration.Identifier.ToFullString();

            Assert.AreEqual("MyClass", name);
            Assert.AreEqual("MyClass ", nameWithTrivia);
        }




        [TestMethod]
        public void Can_change_name_of_empty_class()
        {
            var myClassDef = "class MyClass {}";

            var tree = CSharpSyntaxTree.ParseText(myClassDef);

            var root = tree.GetRoot();

            var classDeclaration = root
                .ChildNodes()
                .Where(x => x.Kind() == SyntaxKind.ClassDeclaration)
                .Cast<ClassDeclarationSyntax>()
                .Single();

   
            var oldIdentifier = classDeclaration.Identifier;

            var newClassNameToken = SyntaxFactory.Identifier("MyClassChanged");

            var newSyntaxDeclaration = classDeclaration
                    .WithIdentifier(newClassNameToken.WithTriviaFrom(oldIdentifier))
                .WithTriviaFrom(classDeclaration);

            var newRoot = root.ReplaceNode(classDeclaration, newSyntaxDeclaration);

            var newRootStr = newRoot.ToString();

            Assert.AreEqual("class MyClassChanged {}", newRootStr);
           
        }
    }
}
