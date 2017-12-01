using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using nIt.nCommon;
using nItCIT.nCommon.ExecutionResult.classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace nIt.nRoslyn
{
    static public class SyntaxOperations
    {



        static public SingleItemResult<ClassDeclarationSyntax> FindClass<TClass>(CSharpSyntaxTree syntaxTree) where TClass : class
        {
            var classToAnalyze = typeof(TClass);
            return FindClass(syntaxTree, classToAnalyze.Name);
        }



        static public SingleItemResult<ClassDeclarationSyntax> FindClass(CSharpSyntaxTree syntaxTree, string className)
        {

            var results = syntaxTree
                .GetRoot()
                .DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .Where(x => (x.Identifier.ToString() == className))
                .Take(2)
                .ToList()
                ;

            switch (results)
            {
                case var noRes when (results.Count == 0):
                    {

                        return SingleItemResult<ClassDeclarationSyntax>.NoneElementsMatching;
                    }

                case var singleton when (results.Count == 1):
                    {
                        return singleton.Single();
                    }

                case var twoItems when (results.Count >= 2):
                    {
                        return SingleItemResult<ClassDeclarationSyntax>.MoreThanTwoElementsMatching(twoItems[0], twoItems[1]);
                    }

                default:
                    throw NotPreparedForThatCase.CannotHappenException;
            }

        }

        static public IdentifierNameSyntax GetVariable(ReturnStatementSyntax returnSyntax)
        {
            var identSyntax = (IdentifierNameSyntax)returnSyntax.Expression;
            return identSyntax;
        }


        static public IXor7ExpressionSyntax GetRightHandSideExpression(ReturnStatementSyntax returnSyntax)
           => Xor7ExpressionSyntax.FromExpression(returnSyntax.Expression);


        static public IXor7ExpressionSyntax GetRightHandSideExpression(LocalDeclarationStatementSyntax variableDeclarationSyntax)
        {

            var equalsSyntax = variableDeclarationSyntax.Declaration.Variables.Single().ChildNodes().OfType<EqualsValueClauseSyntax>().Single();
            var valueSyntax = equalsSyntax.Value;

            return Xor7ExpressionSyntax.FromExpression(valueSyntax);
        }

        static public SyntaxToken GetVariableTokenSingle(LocalDeclarationStatementSyntax declarationSyntax)
             => declarationSyntax.Declaration.Variables.Single().Identifier;



        static public CSharpSyntaxTree Parse(string codeAsText)
              => (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(codeAsText);

        static public Maybe<ClassDeclarationSyntax> GetBaseTypeOf(ClassDeclarationSyntax classSyntax, SyntaxTree syntaxTree, SemanticModel semanticModel)
        {
            var classSymbol = semanticModel.GetDeclaredSymbol(classSyntax);
            var baseSymbol = classSymbol.BaseType;

            if (baseSymbol == null)
            {
                return Maybe.NoValue;
            }
            else
            {
                var baseClassSyntax = baseSymbol.DeclaringSyntaxReferences.Single().GetSyntax();

                return (ClassDeclarationSyntax)baseClassSyntax;
            }
        }

        static public bool IsLoopLike(SyntaxNode syntax)
        {
            switch (syntax)
            {
                case IfStatementSyntax ifSyntax:
                case WhileStatementSyntax whileSyntax:
                    {

                        return true;
                    }

                default:
                    {
                        return false;
                    }
            }
        }

        public static IReadOnlyList<ConstructorDeclarationSyntax> FindConstructorsOf(ClassDeclarationSyntax classDeclarationSyntax, SemanticModel semanticModel, int? argsCount = null)
        {
            var classSymbol = semanticModel.GetDeclaredSymbol(classDeclarationSyntax);

            var ctorSymbols = classSymbol.Constructors
                .Where(x => (argsCount == null) || (x.Parameters.Length == argsCount.Value))
                .ToList();

            var ctorSyntaxes = ctorSymbols
                .Select(x => x.DeclaringSyntaxReferences.Single().GetSyntax())
                .Cast<ConstructorDeclarationSyntax>()
                .ToList();

            return ctorSyntaxes;

        }

        public static LocalDeclarationStatementSyntax GetDelarationOfVariable(BlockSyntax bodyBlock, string variableName)
        {
            var declarator = bodyBlock.DescendantNodes()
                .OfType<VariableDeclaratorSyntax>()
                .Where(x => x.Identifier.ToString() == variableName)
                .Single();

            var declaration = declarator.Ancestors().OfType<LocalDeclarationStatementSyntax>().First();

            return declaration;
        }

        public static IXor2BodyOfMethod GetBodyOfMethod(MethodDeclarationSyntax methodOfClass)
        {

            if (methodOfClass.Body != null)
            {
                return new Xor2BodyOfMethod(methodOfClass.Body);
            }
            else
            {
                var expression = methodOfClass.ExpressionBody.Expression;

                var xor5RhsExpression = Xor7ExpressionSyntax.FromExpression(expression);
                return new Xor2BodyOfMethod(xor5RhsExpression);
            }

        }


        public static Maybe<IReadOnlyList<MethodDeclarationSyntax>> FindMethodsOfClass(CSharpSyntaxTree syntaxTree, string className, string methodName)
        {
            var res = FindClass(syntaxTree, className);

            if (!res.IsA)
            {
                return Maybe.NoValue;
            }
            else
            {
                var classDeclSyntax = res.A;
                return FindMethodsOfClass(syntaxTree, res.A, methodName);
            }
        }


        public static Maybe<IReadOnlyList<MethodDeclarationSyntax>> FindMethodsOfClass(CSharpSyntaxTree syntaxTree, ClassDeclarationSyntax classDeclSyntax, string methodName)
        {

            var methodSyntaxes = classDeclSyntax
                    .DescendantNodes().OfType<MethodDeclarationSyntax>().Cast<MethodDeclarationSyntax>()
                    .Where(x => x.Identifier.ToString() == methodName)
                    .ToArray();

            return methodSyntaxes;
        }

        public static Maybe<IReadOnlyList<MethodDeclarationSyntax>> FindMethodsOfClass<TClass>(Expression<Func<TClass, string>> exMethodName, CSharpSyntaxTree syntaxTree) where TClass : class
        {
            var methodName = ValueOf.Property(exMethodName, null);
            var className = typeof(TClass).Name;
            return FindMethodsOfClass(syntaxTree, className, methodName);
        }

        static public IXor7ExpressionSyntax GetOwner(MemberAccessExpressionSyntax syntax)
            => Xor7ExpressionSyntax.FromExpression(syntax.Expression); 

        static public IXor7ExpressionSyntax GetInvocationTarget(InvocationExpressionSyntax syntax)
            => Xor7ExpressionSyntax.FromExpression(syntax.Expression);


    }




}
