using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace nIt.nRoslyn
{
    public class ComprehensionLinqSyntaxRewriter: CSharpSyntaxRewriter
    {

        public ComprehensionLinqSyntaxRewriter()
        {
            
        }    

        public override SyntaxNode VisitQueryExpression(QueryExpressionSyntax node)
        {
            var transformedQuery = _TransformQuery(node);
  

           return SyntaxFactory.ParenthesizedExpression(transformedQuery);
        }

        private ExpressionSyntax _TransformQuery(QueryExpressionSyntax node)
         => throw new Exception();
    }
}
