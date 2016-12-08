using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day8
{
    class Program
    {
        enum RotationAnchor
        {
            Row,
            Column
        }

        class LittleScreenDisplayByWtf
        {
            public int Width { get; private set; }
            public int Height { get; private set; }

            private bool[,] pixels;

            public int NumPixelsOn
            {
                get
                {
                    int count = 0;
                    for (int x = 0; x < Width; x++)
                    {
                        for (int y = 0; y < Height; y++)
                        {
                            count += pixels[y, x] ? 1 : 0;
                        }
                    }

                    return count;
                }
            }

            public LittleScreenDisplayByWtf(int width = 50, int height = 6)
            {
                pixels = new bool[height, width];

                Width = width;
                Height = height;
            }

            public void Rect(int rectWidth, int rectHeight)
            {
                for (int x = 0; x < rectWidth; x++)
                {
                    for (int y = 0; y < rectHeight; y++)
                    {
                        pixels[y, x] = true;
                    }
                }
            }

            public void Rotate(RotationAnchor anchor, int index, int amount)
            {
                if (anchor == RotationAnchor.Row)
                {
                    var newLine = new bool[Width];

                    for (int x = 0; x < Width; x++)
                    {
                        var newPosition = (x + amount) % Width;
                        newLine[newPosition] = pixels[index, x];
                    }

                    for (int i = 0; i < newLine.Length; i++)
                    {
                        pixels[index, i] = newLine[i];
                    }
                }
                else
                {
                    var newLine = new bool[Height];

                    for (int y = 0; y < Height; y++)
                    {
                        var newPosition = (y + amount) % Height;
                        newLine[newPosition] = pixels[y, index];
                    }

                    for (int i = 0; i < newLine.Length; i++)
                    {
                        pixels[i, index] = newLine[i];
                    }
                }
            }

            public void InterpreateCommand(string command)
            {
                var commandParts = command.ToLower()
                                          .Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)
                                          .Select(s => s.Trim())
                                          .ToArray();

                if (commandParts.Length == 0)
                {
                    throw new Exception("Bad command: " + command);
                }

                if (commandParts[0] == "rect" && commandParts.Length == 2)
                {
                    var rectParts = commandParts[1].Split(new string[] { "x" }, StringSplitOptions.RemoveEmptyEntries);

                    var rectWidth = Convert.ToInt32(rectParts[0]);
                    var rectHeight = Convert.ToInt32(rectParts[1]);

                    Rect(rectWidth, rectHeight);
                }
                else if (commandParts[0] == "rotate" && commandParts.Length == 5)
                {
                    var anchor = (RotationAnchor)Enum.Parse(typeof(RotationAnchor), commandParts[1], true);
                    var index = Convert.ToInt32(commandParts[2].Substring(2));
                    var amount = Convert.ToInt32(commandParts[4]);

                    Rotate(anchor, index, amount);
                }
                else
                {
                    throw new Exception("Unsupported command: " + command);
                }
            }

            public override string ToString()
            {
                var sb = new StringBuilder();
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        sb.Append(pixels[y, x] ? "█" : " ");
                    }
                    sb.Append(Environment.NewLine);
                }

                return sb.ToString();
            }
        }

        static void Main(string[] args)
        {
            var screen = new LittleScreenDisplayByWtf();
            foreach (var command in File.ReadAllLines("input.txt").Select(t => t.Trim()))
            {
                screen.InterpreateCommand(command);
            }

            Console.Write(screen.ToString());
            Console.WriteLine("=============");
            Console.WriteLine($"{screen.NumPixelsOn} pixels are currently on");
        }
    }
}
