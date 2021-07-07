using System;
using System.Text;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ConsoleApp2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Console.WriteLine(isPalindrome("GoGo"));
            //Console.WriteLine(isPalindrome("anna"));
            //Console.WriteLine(isPalindrome("B"));
            //Console.WriteLine(isPalindrome("tattarrattat"));

            //Console.WriteLine(minSplit(500));
            //Console.WriteLine(minSplit(7));
            //Console.WriteLine(minSplit(25));
            //Console.WriteLine(minSplit(27));
            //Console.WriteLine(minSplit(356));


            //Console.WriteLine(notContains(new[] { 1,2,3,4,5}));
            //Console.WriteLine(notContains(new[] { -1, -32, -3333, -94, -5 }));
            //Console.WriteLine(notContains(new[] { 0, 0, 0, 0, 0 }));
            //Console.WriteLine(notContains(new[] { -1, 28, 3, -4, -5 }));
            //Console.WriteLine(notContains(new[] { 11, 23, 3, 4, 65 }));

            //Console.WriteLine(isProperly("(()()()()()))()()"));
            //Console.WriteLine(isProperly("(()())()"));
            //Console.WriteLine(isProperly("("));
            //Console.WriteLine(isProperly("()))("));

            //Console.WriteLine(countVariants(10));
            //Console.WriteLine(countVariants(3));
            //Console.WriteLine(countVariants(14));
            //Console.WriteLine(countVariants(34));
            //Console.WriteLine(countVariants(6));

            //var d = new DataStructure<string>();
            //d.Add("some");
            //d.Add("gama");
            //d.Add("som2");
            //d.Remove("gama");

            //Console.WriteLine( await exchangeRate("usd", "eur"));
            //Console.WriteLine(await exchangeRate("gel", "eur"));
            //Console.WriteLine(await exchangeRate("RUB", "eur"));
            //Console.WriteLine(await exchangeRate("eur", "gel"));
            //Console.WriteLine(await exchangeRate("gel", "rub"));
            //Console.WriteLine(await exchangeRate("aud", "eur"));



        }

        //this function isn't case sensitive
        static bool isPalindrome(string text)
        {

            var normalizedText = text.ToLower();

            var reverse = new string(normalizedText.Reverse().ToArray());
            return reverse == normalizedText;
        }

        static int minSplit(int amount)
        {
            var minCoinCount = 0;

            int calcCoinCount(int total, int coinValue) => total / coinValue;


            minCoinCount += calcCoinCount(amount, 50);
            amount %= 50;

            minCoinCount += calcCoinCount(amount, 20);
            amount %= 20;

            minCoinCount += calcCoinCount(amount, 10);
            amount %= 10;

            minCoinCount += calcCoinCount(amount, 5);
            amount %= 5;

            minCoinCount += calcCoinCount(amount, 1);


            return minCoinCount;

        }

        static int notContains(int[] array)
        {
            if (array.Length < 1)
                throw new ArgumentException("provide at least one element in the array");

            int maxElement = int.MinValue;
            for (int max = array[0], i= 0; i < array.Length; i++)
            {
                max = array[i] > max ? array[i] : max;

                //last iteration
                if(i == array.Length - 1)
                    maxElement = max;
            }

            if (maxElement < 1)
                return 1;
            else return maxElement + 1;
        }

        static bool isProperly(string sequence)
        {
            var stack = new Stack<char>();
            var properly = true;
            
            foreach(var @char in sequence)
            {
                if (@char == '(')
                    stack.Push(@char);
                else
                {
                    if (stack.Count == 0)
                        properly = false;
                    else
                        stack.Pop();
                }
            }

            if (properly && stack.Count == 0)
                return true;
            else return false;
        }

        static int countVariants(int stearsCount)
        {
            if (stearsCount == 1)
                return 1;
            else if (stearsCount == 2)
                return 2;
            else return countVariants(stearsCount - 1) + countVariants(stearsCount - 2);
        }

        static async Task<double> exchangeRate(string fromIdent, string toIdent)
        {
            fromIdent = fromIdent.ToUpper();
            toIdent = toIdent.ToUpper();

            const string url = "http://www.nbg.ge/rss.php";
            using var client = new HttpClient();


            var xml = "";
            try
            {
                xml = await client.GetStringAsync(url);
            }
            catch
            {
                xml = File.ReadAllText("mockRes.txt");
            } 

            //making xml content adaptable to parser
            xml = xml.Replace("<![CDATA[<table border=\"0\">", "").Replace("</table>]]>", "");
            xml = Regex.Replace(xml, @"<img.*?>", "");


            var doc = XDocument.Parse(xml);

           
            double firstCurrencyRate =double.NaN;
            double secondCurrencyRate = double.NaN;


            var firstQuery = (from tr in doc.Descendants("description").ToArray()[1].Descendants("tr")
                              let TDs = tr.Descendants("td").ToArray()
                              where TDs.First().Value == fromIdent
                            let quantity =  Regex.Match(TDs[1].Value, @"\d+").Value
                            select double.Parse(TDs[2].Value) / int.Parse(quantity) ).FirstOrDefault();

            var secondQuery = (from tr in doc.Descendants("description").ToArray()[1].Descendants("tr")
                               let TDs = tr.Descendants("td").ToArray()
                               where TDs.First().Value == toIdent
                               let quantity = Regex.Match(TDs[1].Value, @"\d+").Value
                               select double.Parse(TDs[2].Value) / int.Parse(quantity)).FirstOrDefault();

            firstCurrencyRate = fromIdent == "GEL" ? 1 : firstQuery;
            secondCurrencyRate = toIdent == "GEL" ? 1 : secondQuery;


            return firstCurrencyRate / secondCurrencyRate;

        }
    }

    class DataStructure<T>
    {
        //Dictionary<element, positionOfElementInArray>
        Dictionary<T, int> table = new();
        List<T> array = new List<T>();

        public void Add(T item)
        {
            array.Add(item);
            table[item] = array.Count - 1;
        }

        public void Remove(T item)
        {
            var last =   array[array.Count - 1];
            var itemIndex = table[item];
            array[itemIndex] = last;
            table[last] = itemIndex;

            array = array.GetRange(0, array.Count - 1);
            table.Remove(item);
        }


    }
}
