using System;
using Microsoft.CodeAnalysis;

namespace nIt.nRoslyn
{
    public class SimpleControlFlowAnalysisError
    {
        public SyntaxNode LoopLikeSyntax { get; }

        public SimpleControlFlowAnalysisError(SyntaxNode loopLikeSyntax)
        {
            this.LoopLikeSyntax = loopLikeSyntax;
        }

        static public SimpleControlFlowAnalysisError NotALinearControlFlow(SyntaxNode loopLikeSyntax)
            => new SimpleControlFlowAnalysisError(loopLikeSyntax);
    }
}