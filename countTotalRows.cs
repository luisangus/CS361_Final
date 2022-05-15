using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace countTotalRows
{
    class Program
    {
        static void Main(string[] args)
        {
            //Saves Console background/foreground default colors
            ConsoleColor defaultBackground = Console.BackgroundColor;
            ConsoleColor defaultForeground = Console.ForegroundColor;

            // list of lists that will store the csv file
            List<List<string>> inputFileData = new List<List<string>>();
            string inputFilePathData = "";

            // Microservice description
            backForegroundColors(defaultBackground, defaultForeground);
            Console.WriteLine("**********************************************************************************");
            Console.WriteLine("This microservice counts the Total number of rows in the CSV given");
            Console.WriteLine("and it returns a csv file with the Total count");
            Console.WriteLine("**********************************************************************************");
            Console.WriteLine("");

            // Ask user for file path
            backForegroundColors(ConsoleColor.DarkBlue, ConsoleColor.White);
            Console.WriteLine("Enter the file path for the data file (.csv format only) that you want to process: ");
            backForegroundColors(ConsoleColor.DarkGreen, ConsoleColor.White);
            inputFilePathData = Console.ReadLine();

            string directoryName;
            directoryName = Path.GetDirectoryName(inputFilePathData);

            //read input File
            inputFileData = readCSV(inputFilePathData);

            //declare variables to hold csv data
            string totalCountFilePath = directoryName + "/TotalCount.csv";
            List<List<string>> TotalCountFile = new List<List<string>>();

            string totalcount = "";
            totalcount = inputFileData.Count().ToString();
            List<string> totalCountstring = new List<string>();
            totalCountstring.Add(totalcount);
            TotalCountFile.Add(totalCountstring);

            //Console.WriteLine(totalcount);

            //output the Keywords data into a csv file
            writeCSV(TotalCountFile, totalCountFilePath);
            backForegroundColors(ConsoleColor.DarkBlue, ConsoleColor.White);
            Console.WriteLine("");
            Console.WriteLine("The following file has been created:");
            Console.WriteLine(totalCountFilePath);
            Console.WriteLine("");

        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        // Function: readCSV
        // Description: The purpose of this function is to return a list of lists that contains
        // the file in memory.
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static List<List<string>> readCSV(string csvFile)
        {

            List<List<string>> csvList = new List<List<string>>();
            StreamReader logReader = new StreamReader(new FileStream(csvFile,
                                    FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            while (!logReader.EndOfStream)
            {
                csvList.Add(logReader.ReadLine().Split(',').ToList());
            }
            logReader.Close();

            return csvList;
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        // Function: writeCSV 
        // Description: The purpose of this function is to write a CSV file given the 
        // string list of lists
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static void writeCSV(List<List<string>> log, string csvFile)
        {
            StreamWriter csvWriter = new StreamWriter(csvFile);
            string newline;
            for (int i = 0; i < log.Count; i++)
            {
                newline = "";
                for (int j = 0; j < log[i].Count; j++)
                {
                    newline += log[i][j] + ",";
                }
                csvWriter.WriteLine(newline);
            }
            csvWriter.Close();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////
        /// Function: backForegroundColors
        /// Description: This function changes the curent background/foreground colors back to its
        /// default colors
        ////////////////////////////////////////////////////////////////////////////////////////////
        static void backForegroundColors(ConsoleColor background, ConsoleColor foreground)
        {
            Console.BackgroundColor = background;
            Console.ForegroundColor = foreground;
        }


        ////
    }
}
