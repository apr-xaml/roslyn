using Microsoft.CodeAnalysis;
using nIt.nCommon;
using nItCIT.nCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nIt.nRoslyn.extensions
{
    static class ext_INamedTypeSymbols
    {
        public const string Dot = ".";

        static public bool IsTheType<T>(this INamedTypeSymbol _this)
        {
            var isType = _this.IsType;

            var fullNamespace = string.Join(Dot, _this.GetFullNamepace());
            var areNamespaceEq = (fullNamespace == typeof(T).Namespace);
            var areNamesEq = (_this.Name == typeof(T).Name);

            return (isType && areNamespaceEq && areNamesEq);

        }


        static  Stack<string> _GetFullNamepaceRec(this ISymbol _this)
        {
            if (_this.ContainingNamespace == null)
            {
                return Empty.MutableStack<string>();
            }
            else
            {
                var previous = _this.ContainingNamespace._GetFullNamepaceRec();
                var res = previous.WithPushed(_this.ContainingNamespace.Name);
                return res;
            }
        }

        static public IReadOnlyList<string> GetFullNamepace(this ISymbol _this)
        {
            if(_this.ContainingNamespace == null)
            {
                return Empty.List<string>();
            }
            else
            {
                var previous = _this._GetFullNamepaceRec();
                return previous.FromTopToBottom();
            }
        }
    }
}
