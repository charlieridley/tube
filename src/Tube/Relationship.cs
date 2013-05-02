using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tube
{
    public struct Relationship
    {
        public Relationship(string @from, string to) : this()
        {
            From = @from;
            To = to;
        }

        public string From { get; private set; }
        public string To { get; private set; }
    }
}
