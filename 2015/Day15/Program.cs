using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day15
{
    class Ingredient
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int Durability { get; set; }
        public int Flavor { get; set; }
        public int Texture { get; set; }
        public int Calories { get; set; }
    }

    class Recepie
    {
        public List<(int, Ingredient)> Ingredients { get; set; } = new List<(int, Ingredient)>();
        public int Score
        {
            get
            {
                int sumCapacity = Ingredients.Sum(x => x.Item1 * x.Item2.Capacity);
                int sumDurability = Ingredients.Sum(x => x.Item1 * x.Item2.Durability);
                int sumFlavor = Ingredients.Sum(x => x.Item1 * x.Item2.Flavor);
                int sumTexture = Ingredients.Sum(x => x.Item1 * x.Item2.Texture);

                return Math.Max(0, sumCapacity) * Math.Max(0, sumDurability) * Math.Max(0, sumFlavor) * Math.Max(0, sumTexture);
            }
        }

        public int Calories
        {
            get => Ingredients.Sum(x => x.Item1 * x.Item2.Calories);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var ingredients = new List<Ingredient>();

            foreach (var line in File.ReadAllLines("StuffOntheShelve.txt"))
            {
                var parts = line.Split(' ');

                ingredients.Add(new Ingredient
                {
                    Name = parts[0].TrimEnd(':'),
                    Capacity = Convert.ToInt32(parts[2].TrimEnd(',')),
                    Durability = Convert.ToInt32(parts[4].TrimEnd(',')),
                    Flavor = Convert.ToInt32(parts[6].TrimEnd(',')),
                    Texture = Convert.ToInt32(parts[8].TrimEnd(',')),
                    Calories = Convert.ToInt32(parts[10].TrimEnd(','))
                });
            }

            var recepies = new List<Recepie>();

            for (int ing1 = 0; ing1 <= 100; ing1++)
            {
                for (int ing2 = 0; ing2 <= 100; ing2++)
                {
                    for (int ing3 = 0; ing3 <= 100; ing3++)
                    {
                        int ing4 = 100 - ing1 - ing2 - ing3;

                        var recepie = new Recepie();
                        recepie.Ingredients.Add((ing1, ingredients[0]));
                        recepie.Ingredients.Add((ing2, ingredients[1]));
                        recepie.Ingredients.Add((ing3, ingredients[2]));
                        recepie.Ingredients.Add((ing4, ingredients[3]));

                        recepies.Add(recepie);
                    }
                }
            }

            var maxScore = recepies.Max(x => x.Score);
            var maxScoreWith500Calories = recepies.Where(x => x.Calories == 500).Max(x => x.Score);
            Console.WriteLine($"Maximum cookie score is {maxScore}");
            Console.WriteLine($"Or if you like only 500 calories the score is {maxScoreWith500Calories}");
        }
    }
}
