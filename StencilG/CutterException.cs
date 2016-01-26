using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StencilG
{
    public class CutterException : Exception
    {
        public CutterException(string message) : base(message) { }
    }
}
