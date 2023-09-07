using System.Drawing;
using System.Collections;

namespace ASignInSpace
{

    internal class Draw
    {
        public int Width
        {
            get;
            set;
        } = 256;

        public int Height
        {
            get;
            set;
        } = 256;

        public bool WriteBinFile
        {
            get;
            set;
        } = false;

        public int FileNumber
        {
            get;
            set;
        } = 0;

        public string FilenamePrefix
        {
            get;
            set;
        } = @$"Default";

        public Point[] PlotAddressList
        {
            get;
            set;
        } = Array.Empty<Point>();

        public Point[] DrawLinesPoints
        {
            get;
            set;
        } = Array.Empty<Point>();

        public void DrawStarMap()
        {
            System.Reflection.Assembly assem = System.Reflection.Assembly.GetExecutingAssembly();
            Stream starmapStream = assem.GetManifestResourceStream("ASignInSpace.data17square.bin");
            byte[] starmap = new byte[starmapStream.Length];
            starmapStream.Read(starmap, 0, (int)starmapStream.Length);
            var dataBitsIn = BitArrayHelper.CreateBitArray(starmap);
            DrawBitArray(dataBitsIn);
        }

        public void DrawFile(string filename)
        {
            var dataBytesIn = File.ReadAllBytes(filename);
            var dataBitsIn = BitArrayHelper.CreateBitArray(dataBytesIn);
            DrawBitArray(dataBitsIn);
        }

        public void DrawBitArray(BitArray data)
        {
            Bitmap dialImage = new Bitmap(Width, Height);
            int x = 0, y = 0;
            Graphics g = Graphics.FromImage(dialImage);
            g.Clear(Color.Black);
            int onCount = 0;

            foreach (var p in PlotAddressList)
            {
                g.FillRectangle(Brushes.Blue, p.X, p.Y, 1, 1);
                onCount++;
            }
            for (int i = 0; i < DrawLinesPoints.Length; i += 2)
            {
                g.DrawLine(Pens.Blue, DrawLinesPoints[i], DrawLinesPoints[i + 1]);
                onCount += 2;
            }

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i])
                {
                    g.FillRectangle(Brushes.White, x, y, 1, 1);
                    onCount++;
                }

                x++;
                if (x == dialImage.Width)
                {
                    x = 0;
                    y++;
                }

            }
            if (WriteBinFile)
            {
                File.WriteAllBytes($"{FilenamePrefix}-{FileNumber}.bin", BitArrayHelper.CreateByteArray(data));
            }

            dialImage.Save($"{FilenamePrefix}-{FileNumber}.png", System.Drawing.Imaging.ImageFormat.Png);

            FileNumber++;
            Console.WriteLine($"On count = {onCount}");
        }
    }
}
