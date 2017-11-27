using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nIt.nRoslyn
{
    public class MethodBlockAnalysis
    {
        MethodBlockAnalysis(MethodDeclarationSyntax methodDeclarationSyntax, BlockSyntax methodBodySyntax, SemanticModel semanticModel, ControlFlowAnalysis controlFlow, DataFlowAnalysis dataFlow)
        {
            MethodDeclarationSyntax = methodDeclarationSyntax;
            SemanticModel = semanticModel;
            ControlFlowAnalysis = controlFlow;
            DataFlowAnalysis = dataFlow;
            BodyBlock = methodBodySyntax;
        }

        public MethodDeclarationSyntax MethodDeclarationSyntax { get; }
        public SemanticModel SemanticModel { get; }
        public ControlFlowAnalysis ControlFlowAnalysis { get; }
        public DataFlowAnalysis DataFlowAnalysis { get; }
        public BlockSyntax BodyBlock { get; }

        static public MethodBlockAnalysis FromSemanticModel(MethodDeclarationSyntax methodSyntax, SemanticModel semanticModel)
        {
            var block = SyntaxOperations.GetBodyOfMethod(methodSyntax).A;
            var controlFlow = semanticModel.AnalyzeControlFlow(block);
            var dataFlow = semanticModel.AnalyzeDataFlow(block);

            return new MethodBlockAnalysis(methodSyntax, block, semanticModel, controlFlow, dataFlow);
        }
    }
}
