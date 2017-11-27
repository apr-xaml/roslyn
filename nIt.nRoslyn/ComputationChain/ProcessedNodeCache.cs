using Microsoft.CodeAnalysis;
using nIt.nCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nIt.nRoslyn
{
    public class AlreadyProcessedSyntaxNodes
    {

        Dictionary<SyntaxNode, ComputationGraphNode> _nodesBySyntax = new Dictionary<SyntaxNode, ComputationGraphNode>();

        public Maybe<ComputationGraphNode> Get(SyntaxNode syntaxNode)
        {
            if (_nodesBySyntax.TryGetValue(syntaxNode, out ComputationGraphNode node))
            {
                return node;
            }
            else
            {
                return Maybe.NoValue;
            }
        }

        public AlreadyProcessedSyntaxNodes WithAdded(ComputationGraphNode graphNode)
        {
            _nodesBySyntax.Add(graphNode.Syntax, graphNode);
            return this;
        }
    }
}
