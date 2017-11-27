using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using nIt.nCommon;
using nIt.nCommon.nExecutionResult;
using nItCIT.nCommon.nFSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace nIt.nRoslyn
{
    static public class ComputationGraph
    {
        
        static public (IXor2ComputationNodeReference, AlreadyProcessedSyntaxNodes) FromReturnStatement(ReturnStatementSyntax returnSyntax, MethodBlockAnalysis methodAnalysis, TextSpan span, AlreadyProcessedSyntaxNodes processedNodesOrNull = null)
        {
            Throw.IfNot(methodAnalysis.DataFlowAnalysis.Succeeded);


            var processedNodes = (processedNodesOrNull ?? new AlreadyProcessedSyntaxNodes());

            if (!span.Contains(returnSyntax.Span))
            {
                return _CreatePair(Maybe.NoValue, processedNodes);
            }

            var rhsSyntax = SyntaxOperations.GetRightHandSideExpression(returnSyntax);

            var previous = _DispatchXor7ExpressionSyntax(rhsSyntax, methodAnalysis, processedNodes ?? new AlreadyProcessedSyntaxNodes(), span);

            var node = ComputationGraphNode.FromReturnStatement(returnSyntax, previous.Item1);


            if (!previous.Item1.Exists)
            {
                return _CreatePair(node, previous.Item2);
            }


            return _CreatePair(node, previous.Item2);
        }




        private static (IXor2ComputationNodeReference, AlreadyProcessedSyntaxNodes) _DispatchXor7ExpressionSyntax(IXor7ExpressionSyntax expressionSyntax, MethodBlockAnalysis methodAnalysis, AlreadyProcessedSyntaxNodes processedNodes, TextSpan span)
                => expressionSyntax.Dispatch
                (
                   x => _FromParenthesizedExprSyntax(x, methodAnalysis, processedNodes, span),
                   x => _FromInvocationSyntaxRec(x, methodAnalysis, processedNodes, span),
                   x => _FromBinaryOperatorSyntax(x, methodAnalysis, processedNodes, span),
                   x => _FromCastSyntax(x, methodAnalysis, processedNodes, span),
                   x => _FromMemberAccessSyntaxRec(x, methodAnalysis, processedNodes, span),
                   x => _FromIdentifierSyntaxRec(x, methodAnalysis, processedNodes, span),
                   x => _FinalLinkFromLiteral(x, methodAnalysis, processedNodes, span)
                );



        static (IXor2ComputationNodeReference, AlreadyProcessedSyntaxNodes) _FromLocalDeclaration(LocalDeclarationStatementSyntax syntax, MethodBlockAnalysis methodAnalysis, AlreadyProcessedSyntaxNodes processedNodes, TextSpan span)
        {
            Throw.IfNot(methodAnalysis.DataFlowAnalysis.Succeeded);

            if (!span.Contains(syntax.Span))
            {
                return (Maybe.NoValue, processedNodes);
            }


            var valueExpressionSyntax = SyntaxOperations.GetRightHandSideExpression(syntax);

            var cached = processedNodes.Get(syntax);

            if (cached.Exists)
            {
                return (cached.Value, processedNodes);
            }

            var previous = _DispatchXor7ExpressionSyntax(valueExpressionSyntax, methodAnalysis, processedNodes, span);

            var node = ComputationGraphNode.FromLocalDeclarationSyntax(syntax, previous.Item1);
            return _CreatePair(node, previous);
        }



        private static (IXor2ComputationNodeReference, AlreadyProcessedSyntaxNodes) _FromCastSyntax(CastExpressionSyntax syntax, MethodBlockAnalysis methodAnalysis, AlreadyProcessedSyntaxNodes processedNodes, TextSpan span)
        {

            if (!span.Contains(syntax.Span))
            {
                return (Maybe.NoValue, processedNodes);
            }


            var cached = processedNodes.Get(syntax);


            if (cached.Exists)
            {
                return (cached.Value, processedNodes);
            }

            var rhsSyntax = Xor7ExpressionSyntax.FromExpression(syntax.Expression);
            var previous = _DispatchXor7ExpressionSyntax(rhsSyntax, methodAnalysis, processedNodes, span);
            var res = ComputationGraphNode.FromCastSyntax(syntax, previous.Item1);

            return _CreatePair(res, previous);
        }



        private static (IXor2ComputationNodeReference, AlreadyProcessedSyntaxNodes) _FinalLinkFromLiteral(LiteralExpressionSyntax syntax, MethodBlockAnalysis methodAnalysis, AlreadyProcessedSyntaxNodes processedNodes, TextSpan span)
        {

            if (!span.Contains(syntax.Span))
            {
                return (Maybe.NoValue, processedNodes);
            }

            var cached = processedNodes.Get(syntax);

            if (cached.Exists)
            {
                return (cached.Value, processedNodes);
            }

            var value = SemanticOperations.GetValueFromLiteral(syntax, methodAnalysis.SemanticModel);
            var node = ComputationGraphNode.FinalFromLiteral(syntax);

            return _CreatePair(node, processedNodes);
        }
        private static (IXor2ComputationNodeReference, AlreadyProcessedSyntaxNodes) _FinalFromParameter(ParameterSyntax syntax, MethodBlockAnalysis methodAnalysis, AlreadyProcessedSyntaxNodes processedNodes, TextSpan span)
        {

            if (!span.Contains(syntax.Span))
            {
                return (Maybe.NoValue, processedNodes);
            }

            var cached = processedNodes.Get(syntax);

            if (cached.Exists)
            {
                return (cached.Value, processedNodes);
            }


            var node = ComputationGraphNode.FinalFromParameter(syntax);

            return _CreatePair(node, processedNodes);
        }

        private static (IXor2ComputationNodeReference, AlreadyProcessedSyntaxNodes) _FinalFromFieldDeclaration(FieldDeclarationSyntax syntax, MethodBlockAnalysis methodAnalysis, AlreadyProcessedSyntaxNodes processedNodes, TextSpan span)
        {

            if (!span.Contains(syntax.Span))
            {
                return (Maybe.NoValue, processedNodes);
            }

            var cached = processedNodes.Get(syntax);

            if (cached.Exists)
            {
                return (cached.Value, processedNodes);
            }

            var node = ComputationGraphNode.FinalFromFieldDeclaration(syntax);

            return _CreatePair(node, processedNodes);
        }

        private static (IXor2ComputationNodeReference, AlreadyProcessedSyntaxNodes) _FinalFromPropDeclaration(PropertyDeclarationSyntax syntax, MethodBlockAnalysis methodAnalysis, AlreadyProcessedSyntaxNodes processedNodes, TextSpan span)
        {
            if (!span.Contains(syntax.Span))
            {
                return (Maybe.NoValue, processedNodes);
            }

            var cached = processedNodes.Get(syntax);

            if (cached.Exists)
            {
                return (cached.Value, processedNodes);
            }

            var node = ComputationGraphNode.FinalFromPropertyDeclaration(syntax);

            return _CreatePair(node, processedNodes);
        }


        private static (IXor2ComputationNodeReference, AlreadyProcessedSyntaxNodes) _FromIdentifierSyntaxRec(IdentifierNameSyntax syntax, MethodBlockAnalysis methodAnalysis, AlreadyProcessedSyntaxNodes processedNodes, TextSpan span)
        {

            if (!span.Contains(syntax.Span))
            {
                return (Maybe.NoValue, processedNodes);
            }

            var cached = processedNodes.Get(syntax);

            if (cached.Exists)
            {
                return (cached.Value, processedNodes);
            }

            var variableOrigin = SemanticOperations.GetIdentifierDeclarationSyntax(syntax, methodAnalysis);

            if(!variableOrigin.Exists)
            {
                var node = ComputationGraphNode.FromIdentifierSyntax(syntax, Maybe.NoValue);

                return _CreatePair(node, processedNodes);
            }
            else
            {
                var previous = variableOrigin.Value.A.Dispatch
                                (
                                    x => _FromLocalDeclaration(x, methodAnalysis, processedNodes, span),
                                    x => _FinalFromPropDeclaration(x, methodAnalysis, processedNodes, span),
                                    x => _FinalFromFieldDeclaration(x, methodAnalysis, processedNodes, span),
                                    x => _FinalFromParameter(x, methodAnalysis, processedNodes, span)
                                );

                var node = ComputationGraphNode.FromIdentifierSyntax(syntax, previous.Item1);

                return _CreatePair(node, previous);
            }

            
        }

        private static (IXor2ComputationNodeReference, AlreadyProcessedSyntaxNodes) _FromBinaryOperatorSyntax(BinaryExpressionSyntax syntax, MethodBlockAnalysis methodAnalysis, AlreadyProcessedSyntaxNodes processedNodes, TextSpan span)
        {

            if (!span.Contains(syntax.Span))
            {
                return (Maybe.NoValue, processedNodes);
            }

            var cached = processedNodes.Get(syntax);

            if (cached.Exists)
            {
                return (cached.Value, processedNodes);
            }

            var left = Xor7ExpressionSyntax.FromExpression(syntax.Left);
            var right = Xor7ExpressionSyntax.FromExpression(syntax.Right);

            var leftRes = _DispatchXor7ExpressionSyntax(left, methodAnalysis, processedNodes, span);
            var rightRes = _DispatchXor7ExpressionSyntax(right, methodAnalysis, leftRes.Item2, span);

            var node = ComputationGraphNode.Binary(syntax, leftRes.Item1, rightRes.Item1);

            return _CreatePair(node, rightRes);
        }

        private static (IXor2ComputationNodeReference, AlreadyProcessedSyntaxNodes) _FromMemberAccessSyntaxRec(MemberAccessExpressionSyntax syntax, MethodBlockAnalysis methodAnalysis, AlreadyProcessedSyntaxNodes processedNodes, TextSpan span)
        {

            if (!span.Contains(syntax.Span))
            {
                return (Maybe.NoValue, processedNodes);
            }

            var cached = processedNodes.Get(syntax);

            if (cached.Exists)
            {
                return (cached.Value, processedNodes);
            }



            var owner = SyntaxOperations.GetOwner(syntax);

            var previous = _DispatchXor7ExpressionSyntax(owner, methodAnalysis, processedNodes, span);

            var node = ComputationGraphNode.FromMemberAccess(syntax, previous.Item1);


            return _CreatePair(node, previous);
        }

        private static (IXor2ComputationNodeReference, AlreadyProcessedSyntaxNodes) _FromObjectCreation(ObjectCreationExpressionSyntax syntax, MethodBlockAnalysis methodAnalysis, AlreadyProcessedSyntaxNodes processedNodes, TextSpan span)
        {

            if (!span.Contains(syntax.Span))
            {
                return (Maybe.NoValue, processedNodes);
            }

            var cached = processedNodes.Get(syntax);

            if (cached.Exists)
            {
                return (cached.Value, processedNodes);
            }

            var args = _FormListOfArguments(methodAnalysis, processedNodes, syntax.ArgumentList, span);

            var previousNodes = args.Select(x => x.Item1).ToList();


            var node = ComputationGraphNode.FromObjectCreation(syntax, arguments: previousNodes);

            return _CreatePair(node, args.Last());
        }
        private static (IXor2ComputationNodeReference, AlreadyProcessedSyntaxNodes) _FromParenthesizedExprSyntax(ParenthesizedExpressionSyntax syntax, MethodBlockAnalysis methodAnalysis, AlreadyProcessedSyntaxNodes processedNodes, TextSpan span)
        {

            if (!span.Contains(syntax.Span))
            {
                return (Maybe.NoValue, processedNodes);
            }

            var cached = processedNodes.Get(syntax);

            if (cached.Exists)
            {
                return (cached.Value, processedNodes);
            }

            var previous = _DispatchXor7ExpressionSyntax(Xor7ExpressionSyntax.FromExpression(syntax.Expression), methodAnalysis, processedNodes, span);

            var node = ComputationGraphNode.FromParenthesizedExprSyntax(syntax, previous.Item1);

            return _CreatePair(node, previous);
        }


        private static (IXor2ComputationNodeReference, AlreadyProcessedSyntaxNodes) _FromInvocationSyntaxRec(InvocationExpressionSyntax syntax, MethodBlockAnalysis methodAnalysis, AlreadyProcessedSyntaxNodes processedNodes, TextSpan span)
        {

            if (!span.Contains(syntax.Span))
            {
                return (Maybe.NoValue, processedNodes);
            }

            var cached = processedNodes.Get(syntax);

            if (cached.Exists)
            {
                return (cached.Value, processedNodes);
            }


            var invocationTarget = SyntaxOperations.GetInvocationTarget(syntax);

            var argsProcessed = _FormListOfArguments(methodAnalysis, processedNodes, syntax.ArgumentList, span);

            var lastCache = argsProcessed.Any() ? (argsProcessed.Last().Item2) : (processedNodes);

            var target = _DispatchXor7ExpressionSyntax(invocationTarget, methodAnalysis, lastCache, span);


            if (!argsProcessed.Any())
            {
                var invocWithoutArgs = ComputationGraphNode.FromInvocation(syntax, target.Item1, Empty.List<Maybe<ComputationGraphNode>>());
                return _CreatePair(invocWithoutArgs, processedNodes);
            }
            else
            {
                var previousNodes = argsProcessed.Select(x => x.Item1).ToList();

                var invocWithArgs = ComputationGraphNode.FromInvocation(syntax, target.Item1, previousNodes);
                return _CreatePair(invocWithArgs, argsProcessed.Last());
            }

        }

        private static IReadOnlyList<(Maybe<ComputationGraphNode>, AlreadyProcessedSyntaxNodes)> _FormListOfArguments(MethodBlockAnalysis methodAnalysis, AlreadyProcessedSyntaxNodes processedNodes, ArgumentListSyntax syntax, TextSpan span)
        {

            if (!span.Contains(syntax.Span))
            {
                return (Maybe<ComputationGraphNode>.NoValue, processedNodes).IntoList();
            }

            var arguments = syntax.Arguments
                .Select(x => x.Expression)
                .ToList();

            var argumentsXors = arguments
                .Select(Xor7ExpressionSyntax.FromExpression)
                .ToList();


            var previous = argumentsXors
                            .SelectWithHistory
                            (
                                oxStart: x => _DispatchXor7ExpressionSyntax(x, methodAnalysis, processedNodes, span),
                                oxNext: (xSyntax, xPreviousPair) => _DispatchXor7ExpressionSyntax(xSyntax, methodAnalysis, xPreviousPair.Item2, span)
                            )
                            .ToList();

            return previous;
        }

        private static (IXor2ComputationNodeReference, AlreadyProcessedSyntaxNodes) _CreatePair(IXor2ComputationNodeReference node, (IXor2ComputationNodeReference, AlreadyProcessedSyntaxNodes) previous)
                => (node, !node.IsA ? (previous.Item2) : (previous.Item2.WithAdded(node.A)));

        private static (IXor2ComputationNodeReference, AlreadyProcessedSyntaxNodes) _CreatePair(IXor2ComputationNodeReference node, AlreadyProcessedSyntaxNodes cache)
                => (node, !node.IsA ? (cache) : (cache.WithAdded(node.A)));

    }







}
