using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _nIt.nRoslyn.SyntaxAnalyserExamples
{
    public class PersonObject
    {
        public long Id { get; }
        public string Name { get; }
        public uint ChildrenCount { get; private set; }

        public PersonObject(long id, string name, uint childrenCount)
        {
            Id = id;
            Name = name;
            ChildrenCount = childrenCount;
        }



        public void AddChild(uint childrenDelta)
        {
            ChildrenCount = (ChildrenCount + childrenDelta);
        }




        static public bool AreEquivalent(PersonObject obj1, PersonObject obj2)       
            => (obj1.Id == obj2.Id) && (obj1.Name == obj2.Name) && (obj1.ChildrenCount == obj2.ChildrenCount);
        

        static public void BuildTypicalParent()
        {
            var p = new PersonObject(1, "Sting", 0);
            p.AddChild(+3);
        }

        static public void BuildAndForget()
        {
            new PersonObject(1, "Sting", 0).AddChild(+1);
            
        }


    }
}
