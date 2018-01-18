using DBKernel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBKernel.Collections
{
    public class DigitalTexts : Entitys<DigitalText>
    {
        public Display Display { get; set; }

        public DigitalTexts() { }
        public DigitalTexts(Display Parent)
        {
            this.Display = Parent;
        }
    }
}
