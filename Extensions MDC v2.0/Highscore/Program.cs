using System;
using System.Collections.Generic;
using System.IO;

namespace Highscore
{
    class Program
    {
        public static List<GameResult> WinnerList = new List<GameResult>();
        public static List<GameResult> HighscoreList = new List<GameResult>();

        static void Main(string[] args)
        {
            AddGameResult(new GameResult("klaus", 1111));
            AddGameResult(new GameResult("theo", 111));
            AddGameResult(new GameResult("max", 11));
            FillWinnerList();
            HeapSort(WinnerList);
        }

        /// <summary>
        /// Fills the winner list
        /// </summary>
        public static void FillWinnerList()
        {
            WinnerList.Clear();
            string line; 
            System.IO.StreamReader file = new System.IO.StreamReader(@"Highscore.txt");
            while ((line = file.ReadLine()) != null)
            {
                string[] words = line.Split(' ');
                WinnerList.Add(new GameResult(words[0], Int32.Parse(words[1])));
            }
        }

        /// <summary>
        /// Adds the game result to the text file
        /// </summary>
        /// <param name="newHighscore">New highscore</param>
        public static void AddGameResult(GameResult newHighscore)
        {
            File.AppendAllText("Highscore.txt", newHighscore.ToString() + Environment.NewLine);
        }

        /// <summary>
        /// Gets the index of the father
        /// </summary>
        /// <returns>The index of the father</returns>
        /// <param name="sonIndex">Son index</param>
        public static int GetFather(int sonIndex)
        {
            int fatherIndex;
            fatherIndex = ((sonIndex + 1) / 2) - 1;
            return fatherIndex;
        }

        /// <summary>
        /// Gets the index of the left son
        /// </summary>
        /// <returns>The index of the left son</returns>
        /// <param name="fatherIndex">Father index.</param>
        public static int GetLeftSon(int fatherIndex)
        {
            int leftSonIndex;
            leftSonIndex = fatherIndex * 2 + 1;
            return leftSonIndex;
        }

        /// <summary>
        /// Checks if it is againsts the heap condition
        /// </summary>
        /// <returns><c>true</c>, if heap condition was againsted, <c>false</c> otherwise.</returns>
        /// <param name="list">List.</param>
        public static Boolean AgainstHeapCondition(List<GameResult> list)
        {

            Boolean result = true;

            for (int i = list.Count - 1; i > 0; i--)
            {
                if (list[i].Score > list[(GetFather(i))].Score)
                {
                    result = false;
                    return result;
                }
            }
            return result;
        }

         /// <summary>
         /// Heapify the specified list
         /// </summary>
         /// <param name="list">List.</param>
         public static void Heapify(List<GameResult> list)
         {
             GameResult temp = new GameResult(); 

             while (AgainstHeapCondition(list) == false)
             {
                 for (int i = list.Count - 1; i > 0; i--)
                 {
                     if (list[i].Score > list[(GetFather(i))].Score & list[i].Score >= list[(GetLeftSon((GetFather(i))))].Score)
                     {
                        temp = list[(GetFather(i))];
                        list[(GetFather(i))] = list[i];
                        list[i] = temp; 
                     }
                 }
             }
         }

        /// <summary>
        /// Heap sort
        /// Uses the previous methods
        /// </summary>
        /// <param name="list">List.</param>
         public static void HeapSort(List<GameResult> list)
         {
            HighscoreList.Clear();

             Console.WriteLine("input row:");

             for (int i = 0; i < list.Count; i++)
             {
                 Console.WriteLine(list[i].ToString() + " ");
             }

             while (list.Count > 0)
             {
                 Heapify(list);
                 HighscoreList.Add(list[0]);
                 list.RemoveAt(0); 
             }

             Console.WriteLine();
             Console.WriteLine("output row:");

             for (int i = 0; i < HighscoreList.Count; i++)
             {
                //TODO: Fill frontend list here
                 Console.WriteLine(HighscoreList[i].ToString() + " ");
             }
         }
    }
}
