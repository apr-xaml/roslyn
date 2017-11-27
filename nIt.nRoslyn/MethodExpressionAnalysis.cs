using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nIt.nRoslyn
{
    public class MethodExpressionAnalysis
    {
        public MethodExpressionAnalysis(MethodDeclarationSyntax methodDeclarationSyntax, ExpressionSyntax bodyExpression, SemanticModel semanticModel, DataFlowAnalysis dataFlow)
        {
            BodyExpression = bodyExpression;
            MethodDeclarationSyntax = methodDeclarationSyntax;
            SemanticModel = semanticModel;
            DataFlowAnalysis = dataFlow;
        }

        public ExpressionSyntax BodyExpression { get; }
        public MethodDeclarationSyntax MethodDeclarationSyntax { get; }
        public SemanticModel SemanticModel { get; }
        public DataFlowAnalysis DataFlowAnalysis { get; }

        static public MethodExpressionAnalysis FromSemanticModel(MethodDeclarationSyntax methodSyntax, SemanticModel semanticModel)
        {
            var expression = SyntaxOperations.GetBodyOfMethod(methodSyntax).B.Common;
            var dataFlow = semanticModel.AnalyzeDataFlow(expression);

            return new MethodExpressionAnalysis(methodSyntax, expression, semanticModel, dataFlow);
        }
    }
}
