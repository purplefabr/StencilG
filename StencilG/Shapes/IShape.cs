using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StencilG
{
    public interface IShape
    {
        List<ISegment> Segments { get; }
        Point Origin { get; }
        double Rotation { get; }
        double Shrink { get; }

        IShape CreateInstance(Point origin);
    }
}
