using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using nIt.nCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nIt.nRoslyn
{
    static public class ChainOfConstructor
    {
        static  (ConstructorSyntaxLink, bool IsCompleted) _StartingFromRec(ConstructorDeclarationSyntax ctorDeclarationSyntax, ClassDeclarationSyntax owingType, SemanticModel semanticModel)
        {

            var syntaxTree = ctorDeclarationSyntax.SyntaxTree;

            var nextCtorPath = (ConstructorInitializerSyntax)ctorDeclarationSyntax
                .DescendantNodes()
                .Where(x => x.Kind() == SyntaxKind.BaseConstructorInitializer || x.Kind() == SyntaxKind.ThisConstructorInitializer)
                .SingleOrDefault();

            if (nextCtorPath == null)
            {
                return (_CreateFinalLink(owingType, ctorDeclarationSyntax), IsCompleted: true);
            }


            switch (nextCtorPath.Kind())
            {
                case SyntaxKind.ThisConstructorInitializer:
                    {
                        var nextCtor = _GetConstructor(owingType, nextCtorPath, semanticModel);

                        if(!nextCtor.Exists)
                        {
                            var incompleteLink = _CreateIncompleteIntermediateLink(owingClass: owingType, thisCtorDeclarationSyntax: ctorDeclarationSyntax, pathToNextCtor: nextCtorPath);
                            return (incompleteLink, IsCompleted: false);
                        }

                        var (nextLink, isComplete) = _StartingFromRec(nextCtor.Value, owingType, semanticModel: semanticModel);
                        var link = _CreateIntermediateLink(nextLink, owingClass: owingType, thisCtorDeclarationSyntax: ctorDeclarationSyntax, pathToNextCtor: nextCtorPath);
                        return (link, isComplete);
                    }

                case SyntaxKind.BaseConstructorInitializer:
                    {
                        var baseTypeSyntax = SyntaxOperations.GetBaseTypeOf(owingType, syntaxTree, semanticModel).Value;
                        var nextCtor = _GetConstructor(baseTypeSyntax, nextCtorPath, semanticModel);

                        if (!nextCtor.Exists)
                        {
                            var incompleteLink = _CreateIncompleteIntermediateLink(owingClass: owingType, thisCtorDeclarationSyntax: ctorDeclarationSyntax, pathToNextCtor: nextCtorPath);
                            return (incompleteLink, IsCompleted: false);
                        }   

                        var (nextLink, isCompleted) = _StartingFromRec(nextCtor.Value, owingType: baseTypeSyntax, semanticModel: semanticModel);
                        var link = _CreateIntermediateLink(nextLink, owingClass: owingType, thisCtorDeclarationSyntax: ctorDeclarationSyntax, pathToNextCtor: nextCtorPath);
                        return (link, isCompleted);
                    }


                default:
                    throw NotPreparedForThatCase.CannotHappenException;
            }

        }

        [DebuggerStepThrough]
        public static (ConstructorSyntaxLink, bool IsCompleted) StartingFrom(ConstructorDeclarationSyntax startingCtorSyntax, ClassDeclarationSyntax owingClass, SemanticModel semanticModel)
            => _StartingFromRec(startingCtorSyntax, owingClass, semanticModel);


        private static Maybe<ConstructorDeclarationSyntax> _GetConstructor(ClassDeclarationSyntax owingClassDeclarationSyntax, ConstructorInitializerSyntax nextCtorPathSyntax, SemanticModel semanticModel)
        {
            var nextCtorInfo = semanticModel.GetSymbolInfo(nextCtorPathSyntax);
            var nextCtorSymbol = (IMethodSymbol)nextCtorInfo.Symbol;

            if(nextCtorSymbol == null)
            {
                return Maybe.NoValue;
            }
            else
            {
                var nextCtorSyntax = (ConstructorDeclarationSyntax)nextCtorSymbol.DeclaringSyntaxReferences.Single().GetSyntax();
                return nextCtorSyntax;
            }
           

        }

        [DebuggerStepThrough]
        private static ConstructorSyntaxLink _CreateFinalLink(ClassDeclarationSyntax owingClass, ConstructorDeclarationSyntax ctorDeclarationSyntax)
            => new ConstructorSyntaxLink(owingClass, ctor: ctorDeclarationSyntax, pathToNextCtor: Maybe.NoValue, nextLink: Maybe.NoValue);

        [DebuggerStepThrough]
        private static ConstructorSyntaxLink _CreateIntermediateLink(ConstructorSyntaxLink nextLink, ClassDeclarationSyntax owingClass, ConstructorInitializerSyntax pathToNextCtor, ConstructorDeclarationSyntax thisCtorDeclarationSyntax)
            => new ConstructorSyntaxLink(owingClass, ctor: thisCtorDeclarationSyntax, pathToNextCtor: pathToNextCtor, nextLink: nextLink);

        [DebuggerStepThrough]
        private static ConstructorSyntaxLink _CreateIncompleteIntermediateLink(ClassDeclarationSyntax owingClass, ConstructorDeclarationSyntax thisCtorDeclarationSyntax, ConstructorInitializerSyntax pathToNextCtor)
            => new ConstructorSyntaxLink(owingClass, thisCtorDeclarationSyntax, pathToNextCtor, Maybe.NoValue);
    }



    public class ConstructorSyntaxLink
    {
        public ConstructorSyntaxLink(ClassDeclarationSyntax owingClass, ConstructorDeclarationSyntax ctor, Maybe<ConstructorInitializerSyntax> pathToNextCtor, Maybe<ConstructorSyntaxLink> nextLink)
        {
            OwingClassSyntax = owingClass;
            CtorSyntax = ctor;
            PathToNextCtorSyntax = pathToNextCtor;
            NextLink = nextLink;
        }

        public ClassDeclarationSyntax OwingClassSyntax { get; }

        public ConstructorDeclarationSyntax CtorSyntax { get; }
        public Maybe<ConstructorInitializerSyntax> PathToNextCtorSyntax { get; }

        public Maybe<ConstructorSyntaxLink> NextLink { get; }



    }


 
}
