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

        Dictionary<SyntaxNode, Xor2ComputationNodeReference> _nodesBySyntax = new Dictionary<SyntaxNode, Xor2ComputationNodeReference>();

        public Maybe<Xor2ComputationNodeReference> Get(SyntaxNode syntaxNode)
        {
            if (_nodesBySyntax.TryGetValue(syntaxNode, out Xor2ComputationNodeReference node))
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
