using StencilG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StencilGApp
{
    public class GridGenerator
    {
        private double cellWidth;
        private double cellHeight;
        private int widthMultiple;
        private int heightMultiple;
        private List<GCode> _commands = new List<GCode>();
        public List<GCode> Commands
        {
            get
            {
                return _commands;
            }
        }
        public double F;
        public Point BottomLeftOrigin = new Point(0,0);

        public GridGenerator(double cellWidth, double cellHeight, int widthMultiple, int heightMultiple)
        {
            this.cellWidth = cellWidth;
            this.cellHeight = cellHeight;
            this.widthMultiple = widthMultiple;
            this.heightMultiple = heightMultiple;
            F = 50000;
        }

        public void Compute()
        {
            _commands.Clear();
            for (int x = 0; x < (widthMultiple + 1); x++)
            {
                PenUp();
                _commands.Add(new GCode("G1", BottomLeftOrigin.X + (x * cellWidth), BottomLeftOrigin.Y, double.NaN, F));
                PenDown();
                _commands.Add(new GCode("G1", BottomLeftOrigin.X + (x * cellWidth), BottomLeftOrigin.Y + (heightMultiple * cellHeight), double.NaN, F));
                
            }

            for (int y = 0; y < (heightMultiple + 1); y++)
            {
                PenUp();
                _commands.Add(new GCode("G1", BottomLeftOrigin.X, BottomLeftOrigin.Y + (y * cellHeight), double.NaN, F));
                PenDown();
                _commands.Add(new GCode("G1", BottomLeftOrigin.X + (widthMultiple * cellWidth), BottomLeftOrigin.Y + (y * cellHeight), double.NaN, F));
                
            }
            PenUp();
        }

        public void PenUp()
        {
            _commands.Add(new GCode("G91"));
            _commands.Add(new GCode("G1", double.NaN, double.NaN, 10, F));
            _commands.Add(new GCode("G90"));
        }

        public void PenDown()
        {
            _commands.Add(new GCode("G91"));
            _commands.Add(new GCode("G1", double.NaN, double.NaN, -10, F));
            _commands.Add(new GCode("G90"));
        }

        public List<GCode> RepeatPattern(List<GCode> pattern)
        {
            List<GCode> patterns = new List<GCode>();

            foreach(GCode command in pattern){
                patterns.Add(new GCode(command, -cellWidth, 0));
            }

            foreach (GCode command in pattern)
            {
                patterns.Add(new GCode(command, +cellWidth, 0));
            }

            foreach (GCode command in pattern)
            {
                patterns.Add(new GCode(command, -cellWidth, cellHeight));
            }

            foreach (GCode command in pattern)
            {
                patterns.Add(new GCode(command, 0, cellHeight));
            }
            foreach (GCode command in pattern)
            {
                patterns.Add(new GCode(command, +cellWidth, cellHeight));
            }

            foreach (GCode command in pattern)
            {
                patterns.Add(new GCode(command, -cellWidth, -cellHeight));
            }

            foreach (GCode command in pattern)
            {
                patterns.Add(new GCode(command, 0, -cellHeight));
            }
            foreach (GCode command in pattern)
            {
                patterns.Add(new GCode(command, +cellWidth, -cellHeight));
            }

            return patterns;
        }
    }
}
