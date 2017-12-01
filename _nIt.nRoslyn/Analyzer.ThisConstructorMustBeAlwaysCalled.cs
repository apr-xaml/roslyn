using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using nIt.nRoslyn;
using _nIt.nRoslyn.SyntaxAnalyserExamples;
using nIt.nRoslyn.nAnalyzer.nThisConstructorMustBecalled;
using Microsoft.CodeAnalysis;

namespace _nIt.nRoslyn
{
    [DeploymentItem(@"SyntaxAnalyserExamples/MustCallSpecialConstructor.cs", "SyntaxAnalyserExamples")]
    [TestClass]
    public class ThisConstructorMustBeAlwaysCalledAttribute_Tests
    {
        public string CodeAsText { get; }

        public ThisConstructorMustBeAlwaysCalledAttribute_Tests()
        {
            CodeAsText = File.ReadAllText(@"SyntaxAnalyserExamples/MustCallSpecialConstructor.cs");
        }



        [TestMethod]
        public void Recognises_if_class_contains_aformentioned_ctor_attribute()
        {

            var syntaxTree = SyntaxOperations.Parse(CodeAsText);
            var syntax = SyntaxOperations.FindClass<A>(syntaxTree).A;

            var semantics = SemanticOperations.GetSemanticModel(syntaxTree);

            var classSymbol = semantics.GetDeclaredSymbol(syntax);

            var isUnderInterest = ThisConstructorMustBeCalledAnalyzer.IsUnderInterest(classSymbol);

        }
    }
}
