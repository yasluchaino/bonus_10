using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{

    class Program
    {
        static Queue<String> reachable = new Queue<string>();
        static Dictionary<String, HashSet<String>> productions = new Dictionary<String, HashSet<String>>();
        static void getAllProductions(String grammar)
        {
            var splitArr = grammar.Split();
            //добавляем стартовые символ в достижимые 
            for (int k = 0; k < splitArr.Length; k++)
            {
                for (int kk = 0; kk < splitArr[k].Length; kk++)
                    if (kk == 0 && k == 0)
                    {
                        reachable.Enqueue(splitArr[k][kk].ToString());
                    }
            }
            var result = new Dictionary<String, HashSet<String>>();
            foreach (var str in splitArr)
            {
                var rights = str.Substring(3);
                var left = str.Substring(0, 1);
                result.Add(left, new HashSet<String>());
                foreach (var right in rights.Split('|'))
                {
                    result[left].Add(right);
                    for (int i = 0; i < right.Length; i++)
                    {
                        //добавляем в очередь все достижимые из правых частей
                        if (char.IsUpper(right[i]) && !reachable.Contains(right[i].ToString()) && reachable.Contains(left))
                            reachable.Enqueue(right[i].ToString());

                    }
                }
            }

            productions = result;
        }
        static void removeUnreachFromDictionary()
        {

            var filteredDict = productions.Where(kvp => reachable.Contains(kvp.Key))
                            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            productions = filteredDict;
        }
        static String getResultString()
        {
            String result = "";
            foreach (var p in productions)
            {
                result += p.Key + "->" + String.Join("|", p.Value.ToArray()) + " ";
            }
            return result;
        }




        static String removeUnreachProductions(String grammar)
        {
            getAllProductions(grammar);
            removeUnreachFromDictionary();

            return getResultString();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Введите название файла с описанием гамматики:");
            var fileName = Console.ReadLine();
            if (File.Exists(fileName))
            {
                try
                {
                    var grammar = File.ReadAllText(fileName);
                    Console.WriteLine("Дано: " + grammar);
                    var getResultString = removeUnreachProductions(grammar.Trim());
                    Console.WriteLine("Грамматика без недостижимых: " + getResultString);
                }
                catch
                {
                    Console.WriteLine("Ошибка в ходе выполнения программы");
                }

            }
            else
            {
                Console.WriteLine("Файл не существует");
            }

            Console.ReadKey();

        }
    }
}
