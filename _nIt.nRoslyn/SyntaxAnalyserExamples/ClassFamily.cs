using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _nIt.nRoslyn.SyntaxAnalyserExamples
{
    public class BaseUser
    {
        public BaseUser()
        {

        }

        public BaseUser(long id): this()
        {

        }
    }


    class NormalUser : BaseUser
    {
        public NormalUser(long id, string name) : base()
        {

        }


        public NormalUser(long id, string name, string nickName) : this(id, name)
        {

        }
    }


    class ExtendedUser: NormalUser
    {

        public ExtendedUser() : base(10, "user")
        {

        }


        public ExtendedUser( long id, string name, string nickName): base(id, "Joseph", nickName)
        {

        }
    }
}
