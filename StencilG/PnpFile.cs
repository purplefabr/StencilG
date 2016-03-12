using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StencilG
{
    public class PnpComponent
    {
        public string PartID;
        public string Value;
        public string Package;
        public string Layer;
        public double Rotation;
        public double X;
        public double Y;

        public static PnpComponent Load(string fileLine)
        {
            var parts = fileLine.Split(',');
            PnpComponent component = new PnpComponent();
            if (parts.Length == 7)
            {               
                component.PartID = parts[0];
                component.Value = parts[1];
                component.Package = parts[2];
                component.Layer = parts[3];
                component.Rotation = double.Parse(parts[4]);
                component.X = MathHelper.ToMillimeter(double.Parse(parts[5])/1000);
                component.Y = MathHelper.ToMillimeter(double.Parse(parts[6]) / 1000);
            }
            return component;
        }
    }

    public class PnpFile
    {
        private string filePath;
        private List<string> fileLines;
        public List<PnpComponent> Components
        {
            get; internal set;
        }

        public PnpFile(string filePath)
        {
            this.filePath = filePath;
            this.fileLines = File.ReadAllLines(this.filePath).ToList();
            this.Components = new List<PnpComponent>();

            foreach (string line in fileLines)
            {
                if (line.StartsWith("\""))
                {
                    Components.Add(PnpComponent.Load(line));
                }
            }
        }
    }
}
