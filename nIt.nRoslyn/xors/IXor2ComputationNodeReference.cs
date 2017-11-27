using nIt.nCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nIt.nRoslyn
{


    public interface IXor2ComputationNodeReference : IXor2<ComputationGraphNode, OutsideSyntaxReach>
    {
    }




    public struct OutsideSyntaxReach
    {

    }



    public class Xor2ComputationNodeReference : IXor2ComputationNodeReference
    {
        Xor2<ComputationGraphNode, OutsideSyntaxReach> _innerXor2;

        Xor2ComputationNodeReference(ComputationGraphNode node)
        {
            _innerXor2 = new Xor2<ComputationGraphNode, OutsideSyntaxReach>(node);
        }

        Xor2ComputationNodeReference(OutsideSyntaxReach outside)
        {
            _innerXor2 = new Xor2<ComputationGraphNode, OutsideSyntaxReach>(outside);
        }

        public bool IsA => _innerXor2.IsA;

        public bool IsB => _innerXor2.IsB;

        public ComputationGraphNode A => _innerXor2.A;

        public OutsideSyntaxReach B => _innerXor2.B;

        public Xor2Enum Kind => _innerXor2.Kind;


        public static implicit operator Xor2ComputationNodeReference(OutsideSyntaxReach outside) => new Xor2ComputationNodeReference(outside);

        public static implicit operator Xor2ComputationNodeReference(ComputationGraphNode node) => new Xor2ComputationNodeReference(node);
    }
}
