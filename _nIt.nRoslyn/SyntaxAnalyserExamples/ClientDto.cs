using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _nIt.nRoslyn.SyntaxAnalyserExamples
{
    public class ClientDto
    {
        public ClientDto(long id, string firstName, string lastName)
        {
            Contract.Requires(id > 0);

            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        public long Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
    }
}
