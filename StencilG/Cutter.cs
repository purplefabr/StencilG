using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StencilG
{
    public class Cutter
    {
        private bool _headingKnown = false;   // False when blade direction hasn't been initialised
        private double _heading = 0;          // Blade direction in degrees

        public double Diameter { get; set; }    // Blade body diameter in mm
        public double Angle { get; set; }       // Blade cutting angle in degrees

        public Cutter()
        {
            Diameter = 0.9;
            Angle = 45;
        }
    }
}
