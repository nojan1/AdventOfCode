using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day12
{
    class Program
    {
        static void CountIntegers(JToken token, ref int sum)
        {
            switch (token.Type)
            {
                case JTokenType.Array:
                    foreach (var t in token.Values<JToken>())
                    {
                        CountIntegers(t, ref sum);
                    }

                    break;
                case JTokenType.Object:
                    if (ContainsRed((JObject)token))
                        return;

                    foreach (var t in token.Children())
                    {
                        CountIntegers(t, ref sum);
                    }

                    break;
                case JTokenType.Property:
                    var property = (JProperty)token;

                    if (int.TryParse(property.Name, out int propNameInt))
                    {
                        sum += propNameInt;
                    }

                    CountIntegers(property.Value, ref sum);

                    break;
                case JTokenType.Integer:
                    sum += token.Value<int>();
                    break;
                case JTokenType.String:
                    if (int.TryParse(token.Value<string>(), out int val))
                    {
                        sum += val;
                    }

                    break;
                default:
                    Console.WriteLine($"Warning. Ignoring {token}");
                    break;
            }
        }

        private static bool ContainsRed(JObject obj)
        {
            foreach(var child in obj.Children())
            {
                if(child.Type == JTokenType.Property)
                {
                    var property = (JProperty)child;
                    if(property.Value.Type == JTokenType.String && property.Value.Value<string>() == "red")
                        return true;
                }
            }

            return false;
        }

        static void Main(string[] args)
        {
            var rootObject = JObject.Parse(File.ReadAllText("catlaman.json"));

            int sum = 0;
            CountIntegers(rootObject, ref sum);

            Console.WriteLine($"The count is {sum}");
        }
    }
}

//1510 to low