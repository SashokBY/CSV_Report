using System;
using System.IO;
using System.Linq;

namespace CSV_Report
{
    class Program
    {
        static int[,] ReadAndOuptutDBFromFile()
        {
            string filePath = System.IO.Path.GetFullPath(@"..\..\..\..\") + @"DB.csv";
            Console.WriteLine("Чтение данных с файла: " + filePath);
            string input = "";
            int[,] profitTable = new int[2, 12];
            try
            {
                StreamReader sr = new StreamReader(filePath);
                input = sr.ReadLine();
                int monthCounter = 0, positiveProfit = 0;
                Console.WriteLine($"\nОтчет сформирован:\n");
                Console.WriteLine($"| Месяц |     Доход\t|    Расход\t|    Прибыль\t| ");
                Console.WriteLine("=========================================================");
                while (input != null)
                {
                    int[] profit = input.Split(';').Select(int.Parse).ToArray();
                    profitTable[0, monthCounter] = monthCounter + 1;
                    profitTable[1, monthCounter] = profit[1] - profit[2];
                    if (profitTable[1, monthCounter] > 0) positiveProfit++;
                    monthCounter++;
                    Console.WriteLine($"|{profit[0],4}\t|{profit[1],10}\t|{profit[2],10}\t|{profit[1] - profit[2],10}\t|");
                    input = sr.ReadLine();
                }
                Console.WriteLine($"\nКоличество месяцев с положительной прибылью: {positiveProfit}");
            }
            catch
            {
                Console.WriteLine("Error: Input Data Error");
                Console.ReadKey();
                Environment.Exit(0);
            }
            return profitTable;
        }
        static void findAndOutputWorstMonth(int[,] profitTable, int outputNumberOfWorstMonth)
        {
            #region Sort Array With Worst Month...
            bool changeFlag = true;
            while (changeFlag == true)
            {
                changeFlag = false;
                for (int counter = 0; counter < (int)(profitTable.Length / 2) - 1; counter++)
                {
                    if (profitTable[1, counter] > profitTable[1, counter + 1])
                    {
                        profitTable[1, counter] += profitTable[1, counter + 1];
                        profitTable[1, counter + 1] = profitTable[1, counter] - profitTable[1, counter + 1];
                        profitTable[1, counter] -= profitTable[1, counter + 1];
                        profitTable[0, counter] += profitTable[0, counter + 1];
                        profitTable[0, counter + 1] = profitTable[0, counter] - profitTable[0, counter + 1];
                        profitTable[0, counter] -= profitTable[0, counter + 1];
                        changeFlag = true;
                    }
                }
            }
            #endregion
            int numberChange = 0;
            Console.Write($"\nХудшая прибыль в месяцах:");
            for (int count = 1; count < (int)(profitTable.Length / 2) - 1; count++)
            {
                if (profitTable[1, count] != profitTable[1, count - 1]) numberChange++;
                if (numberChange <= outputNumberOfWorstMonth) Console.Write($" {profitTable[0, count - 1]}");
            }
        }
        static void Main(string[] args)
        {
            int[,] profitTable = ReadAndOuptutDBFromFile();
            findAndOutputWorstMonth(profitTable, 4);
            Console.WriteLine("\n\nНажмите Enter для выхода...");
            Console.ReadKey();
        }
    }
}
