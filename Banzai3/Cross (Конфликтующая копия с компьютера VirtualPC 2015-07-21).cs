using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banzai3
{
    class Cross
    {
        public readonly int Width;
        public readonly int Height;
        
        /// <summary>
        /// TODO IReadOnlyList interface
        /// </summary>
        public readonly List<int>[] WidthVal;
        public readonly List<int>[] HeightVal;

        static readonly int[] One = { 1 };

        /// <summary>
        /// Don't less 1
        /// </summary>
        public int HeaderWidth()
        {
            return
                WidthVal
                .Select(v => v.Count)
                .Concat(One)
                .Max();
        }

        /// <summary>
        /// Don't less 1
        /// </summary>
        public int HeaderHeight()
        {
            return
                WidthVal
                .Select(v => v.Count)
                .Concat(One)
                .Max();
        }

        public Cross(int width, int height)
        {
            Width = width;
            Height = height;
            WidthVal = new List<int>[width];
            HeightVal = new List<int>[height];
        }

        public Cross(string fileName)
        {
            using (var t = File.OpenText(fileName))
            {

                var s = t.ReadLine();
                if (s == null)
                {
                    throw new FormatException("Line 1 need Width and Height values");
                }
                try
                {
                    var ss = s.Split(' ');
                    Width = int.Parse(ss[0]);
                    Height = int.Parse(ss[1]);
                }
                catch (Exception)
                {
                    throw new FormatException("Line 1 need Width and Height space separated values");
                }
                WidthVal = new List<int>[Width];
                HeightVal = new List<int>[Height];
                if (!string.IsNullOrWhiteSpace(t.ReadLine()))
                    throw new FormatException("Line 2 is not empty");
                for (var i = 0; i < Width; i++)
                {
                    WidthVal[i] = ImportLine(t);
                }
                if (!string.IsNullOrWhiteSpace(t.ReadLine()))
                    throw new FormatException($"Line {(Width + 2)} is not empty");
                for (var i = 0; i < Height; i++)
                {
                    HeightVal[i] = ImportLine(t);
                }
            }
        }

        private static List<int> ImportLine(TextReader t)
        {
            var s = t.ReadLine();
            if (s == null)
                return new List<int>();
            var ss = s.Split(' ');
            return ss.Select(int.Parse)
                .ToList();
        }

        public void Export(string fileName)
        {
            using (var t = File.CreateText(fileName))
            {
                t.WriteLine("{0} {1}", Width, Height);
                t.WriteLine();
                foreach (var row in WidthVal)
                {
                    ExportLine(t, row);
                }
                t.WriteLine();
                foreach (var row in HeightVal)
                {
                    ExportLine(t, row);
                }
            }
        }

        private static void ExportLine(TextWriter t, List<int> row)
        {
            if (row != null && row.Count > 0)
                t.WriteLine(string.Join(" ", row));
            else
                t.WriteLine();
        }

        public static Cross Import(string fileName)
        {
            return new Cross(fileName);
        }
    }
}
