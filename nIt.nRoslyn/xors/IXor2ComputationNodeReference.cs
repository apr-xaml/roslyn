using nIt.nCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;

namespace nIt.nRoslyn
{


    public interface IXor2ComputationNodeReference : IXor2<ComputationGraphNode, OutsideSyntaxReach>
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

        public static Xor2ComputationNodeReference OutOfScope(SyntaxNode returnSyntax)
         => new Xor2ComputationNodeReference(OutsideSyntaxReach.Instance);

        public ComputationGraphNode A => _innerXor2.A;

        public OutsideSyntaxReach B => _innerXor2.B;

        public Xor2Enum Kind => _innerXor2.Kind;


        public static implicit operator Xor2ComputationNodeReference(OutsideSyntaxReach outside) => new Xor2ComputationNodeReference(outside);

        public static implicit operator Xor2ComputationNodeReference(ComputationGraphNode node) => new Xor2ComputationNodeReference(node);



    
    }

    public struct OutsideSyntaxReach
    {
        static public OutsideSyntaxReach Instance { get; } = new OutsideSyntaxReach();
    }


}
