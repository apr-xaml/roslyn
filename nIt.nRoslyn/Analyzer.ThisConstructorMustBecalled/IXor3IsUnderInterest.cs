using Microsoft.CodeAnalysis;
using nIt.nCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nIt.nRoslyn.Analyzer.ThisConstructorMustBecalled
{
    public interface IXor3IsUnderInterest : IXor3<ExactlyOneCtorWithAttribute, MoreThanOneCtorWithAtribute, NoneCtorWithAttribute>
    {
    }

    public class Xor3IsUnderInterest : IXor3IsUnderInterest
    {
        Xor3<ExactlyOneCtorWithAttribute, MoreThanOneCtorWithAtribute, NoneCtorWithAttribute> _innerXor;


        public Xor3IsUnderInterest(ISymbol ctorSymbol)
        {
            _innerXor = new ExactlyOneCtorWithAttribute(ctorSymbol);
        }




        public bool IsA => _innerXor.IsA;

        public bool IsB => _innerXor.IsB;

        public bool IsC => _innerXor.IsC;

        public ExactlyOneCtorWithAttribute A => _innerXor.A;

        public MoreThanOneCtorWithAtribute B => _innerXor.B;

        public NoneCtorWithAttribute C => _innerXor.C;

        public Xor3Enum Kind => _innerXor.Kind;
    }

    public struct ExactlyOneCtorWithAttribute
    {
        public ExactlyOneCtorWithAttribute(ISymbol ctorSymbol)
        {
            Symbol = ctorSymbol;
        }
        ISymbol Symbol { get; }
    }

    public struct MoreThanOneCtorWithAtribute
    {

    }


    public struct NoneCtorWithAttribute
    {

    }


}
