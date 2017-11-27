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
    public partial class UT_ConstructorAnalysis
    {
        #region helpers

        private static void _Assert_IsLastLink(ConstructorSyntaxLink link, ClassDeclarationSyntax classDeclarationSyntax, ConstructorDeclarationSyntax ctorSyntax)
        {
            Assert.AreEqual(link.CtorSyntax, ctorSyntax);
            Assert.AreEqual(link.OwingClassSyntax, classDeclarationSyntax);
            Assert.IsFalse(link.PathToNextCtorSyntax.Exists);
            Assert.IsFalse(link.NextLink.Exists);
        }


        private static void _Assert_IsIntermadiateLink(ConstructorSyntaxLink link, ClassDeclarationSyntax classDeclarationSyntax, ConstructorDeclarationSyntax ctorDeclarationSyntax)
        {
            Assert.AreEqual(link.CtorSyntax, ctorDeclarationSyntax);
            Assert.AreEqual(link.OwingClassSyntax, classDeclarationSyntax);
            Assert.AreEqual(link.PathToNextCtorSyntax.Value, ctorDeclarationSyntax.DescendantNodes().OfType<ConstructorInitializerSyntax>().Single());
        }


        #endregion





    }
}
