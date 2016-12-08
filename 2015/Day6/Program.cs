using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day6
{
    class Program
    {
        static void Main(string[] args)
        {
            var lights = new int[1000, 1000];
            lights.Initialize();

            foreach(var line in File.ReadAllLines("lienus.txt"))
            {
                var coordMatches = Regex.Matches(line, @"(\d*,\d*)");
                var from = coordMatches[0].Groups[1].Value.Split(',').Select(s => Convert.ToInt32(s)).ToArray();
                var to = coordMatches[1].Groups[1].Value.Split(',').Select(s => Convert.ToInt32(s)).ToArray();

                for(int x = from[0]; x <= to[0]; x++)
                {
                    for(int y = from[1]; y <= to[1]; y++)
                    {
                        if (line.Contains("on"))
                        {
                            lights[x, y]++;
                            //lights[x, y] = 1;
                        }else if (line.Contains("toggle"))
                        {
                            lights[x, y] += 2;
                            //if (lights[x, y] > 0)
                            //    lights[x, y] = 0;
                            //else
                            //    lights[x, y] = 1;
                        }
                        else
                        {
                            lights[x, y]--;

                            if (lights[x, y] < 0)
                                lights[x, y] = 0;
                        }
                    }
                }
            }

            var count = 0;
            var max = 0;
            for (int x = 0; x < 1000; x++)
                for (int y = 0; y < 1000; y++)
                {
                    if(lights[x,y] > max)
                    {
                        max = lights[x, y];
                    }
                    //count += lights[x, y] > 0 ? 1 : 0;
                    count += lights[x, y];
                }

            Console.WriteLine("Light count is: " + count.ToString());
            Console.WriteLine("Max brightness is: " + max.ToString());

            SaveImage(lights);
        }

        static void SaveImage(int[,] lights)
        {
            var baseColor = Color.Ivory;
            var rnd = new Random();

            using (var bitmap = new Bitmap(lights.GetLength(0), lights.GetLength(1)))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    for(int y = 0;y < lights.GetLength(0); y++)
                    {
                        for (int x = 0; x < lights.GetLength(1); x++)
                        {
                            Color color;
                            
                            if (lights[y, x] > 0)
                            {
                                color = ColorFromAhsb(255, baseColor.GetHue(), baseColor.GetSaturation(), lights[y, x] / 50.0f);
                            }
                            else
                            {
                                color = Color.Black;
                            }

                            graphics.FillRectangle(new SolidBrush(color), new Rectangle(y,x,1,1));
                        }
                    }
                }

                bitmap.Save("lights.png");
                Process.Start("lights.png");
            }
        }

        public static Color ColorFromAhsb(int a, float h, float s, float b)
        {

            //if (0 > a || 255 < a)
            //{
            //    throw new ArgumentOutOfRangeException("a", a,
            //      Properties.Resources.InvalidAlpha);
            //}
            //if (0f > h || 360f < h)
            //{
            //    throw new ArgumentOutOfRangeException("h", h,
            //      Properties.Resources.InvalidHue);
            //}
            //if (0f > s || 1f < s)
            //{
            //    throw new ArgumentOutOfRangeException("s", s,
            //      Properties.Resources.InvalidSaturation);
            //}
            //if (0f > b || 1f < b)
            //{
            //    throw new ArgumentOutOfRangeException("b", b,
            //      Properties.Resources.InvalidBrightness);
            //}

            if (0 == s)
            {
                return Color.FromArgb(a, Convert.ToInt32(b * 255),
                  Convert.ToInt32(b * 255), Convert.ToInt32(b * 255));
            }

            float fMax, fMid, fMin;
            int iSextant, iMax, iMid, iMin;

            if (0.5 < b)
            {
                fMax = b - (b * s) + s;
                fMin = b + (b * s) - s;
            }
            else
            {
                fMax = b + (b * s);
                fMin = b - (b * s);
            }

            iSextant = (int)Math.Floor(h / 60f);
            if (300f <= h)
            {
                h -= 360f;
            }
            h /= 60f;
            h -= 2f * (float)Math.Floor(((iSextant + 1f) % 6f) / 2f);
            if (0 == iSextant % 2)
            {
                fMid = h * (fMax - fMin) + fMin;
            }
            else
            {
                fMid = fMin - h * (fMax - fMin);
            }

            iMax = Convert.ToInt32(fMax * 255);
            iMid = Convert.ToInt32(fMid * 255);
            iMin = Convert.ToInt32(fMin * 255);

            switch (iSextant)
            {
                case 1:
                    return Color.FromArgb(a, iMid, iMax, iMin);
                case 2:
                    return Color.FromArgb(a, iMin, iMax, iMid);
                case 3:
                    return Color.FromArgb(a, iMin, iMid, iMax);
                case 4:
                    return Color.FromArgb(a, iMid, iMin, iMax);
                case 5:
                    return Color.FromArgb(a, iMax, iMin, iMid);
                default:
                    return Color.FromArgb(a, iMax, iMid, iMin);
            }
        }
    }
}
