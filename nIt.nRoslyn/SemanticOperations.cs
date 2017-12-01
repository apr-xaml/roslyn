using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using nIt.nCommon;
using nIt.nCommon.nExecutionResult;
using nItCIT.nCommon.ExecutionResult.classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nIt.nRoslyn
{
    static public class SemanticOperations
    {

        static public SingleItemResult<INamedTypeSymbol> FindClassSingle<TClass>(SyntaxTree syntaxTree, SemanticModel semanticModel) where TClass : class
        {
            var classToAnalyze = typeof(TClass);


            var classDeclarationSyntax = syntaxTree
                .GetRoot()
                .DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .Single(x => (x.Identifier.ToString() == classToAnalyze.Name))
                ;


            var typeSymbol = semanticModel.GetDeclaredSymbol(classDeclarationSyntax);
            
            if (typeSymbol == null)
            {
                return SingleItemResult<INamedTypeSymbol>.NoneElementsMatching;
            }
            else
            {
                return new SingleItemResult<INamedTypeSymbol>(typeSymbol);
            }

        }

        internal static bool HasAttribute<T>(IMethodSymbol x) where T: Attribute
        {
            throw new NotImplementedException();
        }

        static public IReadOnlyList<IMethodSymbol> FindAllMethodsOfClass(INamedTypeSymbol typeSymbol, string methodNameOf)
        {
            var members = typeSymbol.GetMembers();

            var methods = members
                .Where(x => x.Kind == SymbolKind.Method && x.Name == methodNameOf)
                .Cast<IMethodSymbol>()
                .ToList();

            return methods;
        }



        static public SemanticModel GetSemanticModel(SyntaxTree syntaxTree)
        {


            var dlls = _GetAssemblies(WellKnownAssembliesEnum.MsCoreLib);

            var compilation = CSharpCompilation
                .Create
                (
                    "nIt.nMyCompilation",
                    new[] { syntaxTree },
                    references: dlls
                );

            var semanticModel = compilation.GetSemanticModel(syntaxTree);

            return semanticModel;
        }

        private static IReadOnlyList<MetadataReference> _GetAssemblies(params WellKnownAssembliesEnum[] assemblies)
         => assemblies.Select
            (

             x =>
             {
                 switch (x)
                 {
                     case WellKnownAssembliesEnum.MsCoreLib:
                         {
                             return @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.2\mscorlib.dll";
                         }
                     default:
                         throw NotPreparedForThatCase.UnexpectedEnumException(x);
                 }
             }
            )
            .Select(x => MetadataReference.CreateFromFile(x))
            .ToArray();
        

        private static IReadOnlyList<MetadataReference> _AssembliesContaining(params Type[] types)
            => types
            .Select(x => Path.GetDirectoryName(x.Assembly.Location))
            .Select(x => MetadataReference.CreateFromFile(x))
            .Distinct()
            .ToArray();

        static public SemanticModel GetSemanticModel(SyntaxTree syntaxTree, IReadOnlyList<SyntaxTree> syntaxTrees)
        {
            var compilation = CSharpCompilation.Create("nIt.nMyCompilation", syntaxTrees);
            var semanticModel = compilation.GetSemanticModel(syntaxTree);

            return semanticModel;
        }

        static public object GetValueFromLiteral(LiteralExpressionSyntax literalSyntax, SemanticModel semanticModel)
        {
            var value = semanticModel.GetConstantValue(literalSyntax);
            if (!value.HasValue)
            {
                throw NotPreparedForThatCase.CannotHappenException;
            }
            else
            {
                return value.Value;
            }
        }

        static public Maybe<FuncResult<IXor4IdentifierOriginSyntax, SimpleControlFlowAnalysisError>> GetIdentifierDeclarationSyntax(IdentifierNameSyntax syntax, MethodBlockAnalysis methodAnalysis)
        {

            Maybe<FuncResult<IXor4IdentifierOriginSyntax, SimpleControlFlowAnalysisError>> __Ok(IXor4IdentifierOriginSyntax ok) => new FuncResult<IXor4IdentifierOriginSyntax, SimpleControlFlowAnalysisError>(ok);
            Maybe<FuncResult<IXor4IdentifierOriginSyntax, SimpleControlFlowAnalysisError>> __Error(SimpleControlFlowAnalysisError error) => new FuncResult<IXor4IdentifierOriginSyntax, SimpleControlFlowAnalysisError>(error);


            var loopLikeSyntax = syntax.Ancestors().FirstOrDefault(x => SyntaxOperations.IsLoopLike(x));

            if (loopLikeSyntax != null)
            {
                return __Error(SimpleControlFlowAnalysisError.NotALinearControlFlow(loopLikeSyntax));
            }
            else
            {
                var symbolInfo = methodAnalysis.SemanticModel.GetSymbolInfo(syntax);


                switch (symbolInfo.Symbol.Kind)
                {
                    case SymbolKind.NamedType:
                    case SymbolKind.Namespace:
                        {
                            return Maybe.NoValue;
                        }
                    case SymbolKind.Local:
                        {
                            var declaratorSyntax = (VariableDeclaratorSyntax)symbolInfo.Symbol.DeclaringSyntaxReferences.Single().GetSyntax();
                            var delarationSyntax = declaratorSyntax.Ancestors().OfType<LocalDeclarationStatementSyntax>().First();
                            return __Ok(new Xor4IdentifierOriginSyntax(delarationSyntax));
                        }
                    case SymbolKind.Field:
                        {
                            var declaringSyntax = (FieldDeclarationSyntax)symbolInfo.Symbol.DeclaringSyntaxReferences.Single().GetSyntax();
                            return __Ok(new Xor4IdentifierOriginSyntax(declaringSyntax));
                        }
                    case SymbolKind.Property:
                        {
                            var declaringSyntax = (PropertyDeclarationSyntax)symbolInfo.Symbol.DeclaringSyntaxReferences.Single().GetSyntax();
                            return __Ok(new Xor4IdentifierOriginSyntax(declaringSyntax));
                        }
                    case SymbolKind.Parameter:
                        {
                            var declaringSyntax = (ParameterSyntax)symbolInfo.Symbol.DeclaringSyntaxReferences.Single().GetSyntax();
                            return __Ok(new Xor4IdentifierOriginSyntax(declaringSyntax));
                        }
                    default:
                        throw NotPreparedForThatCase.UnexpectedEnumException(symbolInfo.Symbol.Kind);
                }




            }


        }






    }






}
