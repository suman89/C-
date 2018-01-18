using DBKernel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBKernel.Collections
{
    public class Displays:Entitys<Display>
    {
        public Domain Domain { get; set; }

        public Displays(Domain Parent)
        {
            this.Domain = Parent;
        }

        public Displays() { }
    }
}
