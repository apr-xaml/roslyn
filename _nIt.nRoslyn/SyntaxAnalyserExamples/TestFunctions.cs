using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _nIt.nRoslyn.SyntaxAnalyserExamples
{
    public class TestFunctions
    {
        public int Id { get; }


        public TestFunctions()
        {
            Id = 1;
        }



        static public int DoublesParameter(int x)
        {
            var res = 2 * x;
            return res;
        }


        static public int GetMathSign(int x)
        {
            if (x > 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        static public int IncrementUntil10(int x)
        {
            while (true)
            {
                if (x > 10)
                {
                    return x;
                }
                else
                {
                    x++;
                }
            }
        }

        public int GetId()
        {
            return Id;
        }

        public int ReturnSameValue(int x)
        {
            return x;
        }



        public int ProcessInt32Parameter(int x)
        {
            var doubled = (2 * x);
            var doubledIncremented = (doubled + 1);
            var sqrtDoubledIncremented = System.Math.Sqrt(doubledIncremented);
            var sqrtDoubledIncrementedInt = (int)sqrtDoubledIncremented;

            return sqrtDoubledIncrementedInt;
        }

        public string ProcessStringParameter(string x)
        {
            return x
                .ToCharArray()
                .ElementAt(2)
                .GetType()
                .Name
                .ToLower();
        }
  



    }
}
