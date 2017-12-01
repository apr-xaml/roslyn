using nIt.nRoslyn.nAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _nIt.nRoslyn.SyntaxAnalyserExamples
{
    public class A
    {


  

        [ThisConstructorMustBeAlwaysCalled]
        public A(long id)
        {

        }

        public A(string a) : this(1)
        {

        }


        public A(string a, string b) 
        {

        }

        
    }
}


