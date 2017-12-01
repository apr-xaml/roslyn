using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using nIt.nRoslyn.nAttributes;
using nIt.nCommon;
using nIt.nRoslyn.Analyzer.ThisConstructorMustBecalled;

namespace nIt.nRoslyn.nAnalyzer.nThisConstructorMustBecalled
{
    public class ThisConstructorMustBeCalledAnalyzer
    {
        public static IXor3IsUnderInterest IsUnderInterest(INamedTypeSymbol classSymbol)
        {
            var isClass = (classSymbol.TypeKind == TypeKind.Class);

            var ctors = classSymbol.Constructors.Where(SemanticOperations.HasAttribute<ThisConstructorMustBeAlwaysCalledAttribute>).ToList();

            if(!ctors.Any())
            {
                return Xor3IsUnderInterest.NoneCtor;
            }
            else
            {
                var (ctor, rest) = ctors;

                if(rest.Any())
                {
                    return Xor3IsUnderInterest.MoreThanOne(ctors);
                }
                else
                {
                    return new Xor3IsUnderInterest(ctor);
                }
            }

        }
    }
}
