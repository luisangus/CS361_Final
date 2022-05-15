using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace keywordMatching
{
    class Program
    {
        static int Main(string[] args)
        {
            //Saves Console background/foreground default colors
            ConsoleColor defaultBackground = Console.BackgroundColor;
            ConsoleColor defaultForeground = Console.ForegroundColor;

            string inputFilePathData = "";
            string inputFilePathKeywords = "";
            string answer = "N";

            while (answer != "Y")
            {
                backForegroundColors(defaultBackground, defaultForeground);
                Console.WriteLine("**********************************************************************************");
                Console.WriteLine("This microservice matches keywords to the defects recorded.");
                Console.WriteLine("It requires the defects and keywords to be stored in CSV format");
                Console.WriteLine("**********************************************************************************");
                Console.WriteLine("");

                backForegroundColors(ConsoleColor.DarkBlue, ConsoleColor.White);
                Console.WriteLine("Enter the file path for the data file (.csv format only) that you want to process:  ");
                backForegroundColors(ConsoleColor.DarkGreen, ConsoleColor.White);
                inputFilePathData = Console.ReadLine();

                backForegroundColors(ConsoleColor.DarkBlue, ConsoleColor.White);
                Console.WriteLine("Enter the file path for the keyword file (.csv format only) that you want to process:  ");
                backForegroundColors(ConsoleColor.DarkGreen, ConsoleColor.White);
                inputFilePathKeywords = Console.ReadLine();

                backForegroundColors(ConsoleColor.DarkBlue, ConsoleColor.White);
                Console.WriteLine("You entered these 2 file paths:");
                Console.WriteLine(inputFilePathData);
                Console.WriteLine(inputFilePathKeywords);
                Console.WriteLine("Is this correct? (Y/N)");
                backForegroundColors(ConsoleColor.DarkGreen, ConsoleColor.White);
                answer = Console.ReadLine();

                if (answer == "Y") {

                } else if (answer == "N") {
                    backForegroundColors(ConsoleColor.DarkRed, ConsoleColor.White);
                    Console.WriteLine("");
                    Console.WriteLine("*** You selected 'N'. Please enter the correct file paths ***");
                    Console.WriteLine("");
                } else {
                    backForegroundColors(ConsoleColor.Red, ConsoleColor.White);
                    Console.WriteLine("");
                    Console.WriteLine("***You entered an invalid selection***");
                    Console.WriteLine("");
                }

            }

            List<List<string>> inputFileKeywords = new List<List<string>>();
            List<List<string>> inputFileData = new List<List<string>>();

            string outputFilePathData = inputFilePathData.Substring(0, inputFilePathData.Length - 4) + "_output.csv";
            string outputFilePathKeywords = inputFilePathKeywords.Substring(0, inputFilePathKeywords.Length - 4) + "_output.csv";

            //read inputfile
            inputFileKeywords = readCSV(inputFilePathKeywords);
            inputFileData = readCSV(inputFilePathData);

            inputFileKeywords = turnToUpper(inputFileKeywords);
            inputFileData = turnToUpper(inputFileData);

            //creating a list out of the input files to be able to modify them freely
            List<List<string>> outputFileKeywords = new List<List<string>>();
            List<List<string>> outputFileData = new List<List<string>>();
            //read file
            outputFileKeywords = readCSV(inputFilePathKeywords);
            outputFileData = readCSV(inputFilePathData);
            //turn to upper case
            outputFileKeywords = turnToUpper(outputFileKeywords);
            outputFileData = turnToUpper(outputFileData);

            //remove cells
            outputFileKeywords = removeEmptyCells(outputFileKeywords);
            outputFileData = removeEmptyCells(outputFileData);

            // this will contain the codes/keywords/count
            List<keywordCodesCount> listStruct = new List<keywordCodesCount>();

            string returnCode;
            string comments_F = "COMMENTS";
            string returnC_F = "RETURN_CODE";
            string kwords_K = "KEYWORDS";
            int indexComm_F = inputFileData[0].IndexOf(comments_F);
            int indexCause_F = inputFileData[0].IndexOf(returnC_F);

            int indexKeyw_K = inputFileKeywords[0].IndexOf(kwords_K);
            int indexCause_K = inputFileKeywords[0].IndexOf(returnC_F);

            outputFileData[0].Add(kwords_K);
            int indexKeyw_F = outputFileData[0].IndexOf(kwords_K);

            for (int i = 1; i < inputFileKeywords.Count; i++)
            {
                outputFileKeywords[i].Add("0");
                //go through all rows to get the keywords
                for (int r = 1; r < inputFileData.Count; r++)
                {
                    int count = 0;
                    returnCode = inputFileKeywords[i][0];
                    if (inputFileData[r][indexCause_F] == returnCode)
                    {
                        //some rows have multiple keywords that count the same, such as MULTIPLE AND SEVERAL
                        for (int j = 1; j < inputFileKeywords[i].Count; j++)
                        {
                            // the text NO_KEYWORD is added to all rows that match the returnCode given
                            if (indexKeyw_F >= outputFileData[r].Count) {
                                outputFileData[r].Add("NO_KEYWORD");
                            } else if (outputFileData[r][indexKeyw_F] == "")  {
                                outputFileData[r][indexKeyw_F] = "NO_KEYWORD";
                            }

                            if (inputFileData[r][indexComm_F].Contains(inputFileKeywords[i][j]) &&
                                    inputFileKeywords[i][j] != "")
                            {
                                //add keyword to DATA file
                                if (count == 0) {
                                    if (outputFileData[r][indexKeyw_F] == "NO_KEYWORD")
                                    {
                                        outputFileData[r][indexKeyw_F] = inputFileKeywords[i][indexKeyw_K];
                                    }
                                    else
                                    {
                                        outputFileData[r].Add(inputFileKeywords[i][indexKeyw_K]);
                                    }
                                }
                                count++;
                                //add count to the keyword in keywordFile
                                //if last item in row is a number it means there is a qty already
                                string lastItem = outputFileKeywords[i][outputFileKeywords[i].Count - 1];
                                if (int.TryParse(lastItem, out int value))
                                {
                                    int qty = int.Parse(lastItem) + 1;
                                    outputFileKeywords[i][outputFileKeywords[i].Count - 1] = qty.ToString();
                                }
                                else
                                {
                                    outputFileKeywords[i].Add("1");
                                }
                            }
                        }

                    }
                    else
                    {
                        outputFileData[r].Add("NO_KEYWORD");
                    }
                }

            }
            //Clean up outputFileKeywords to delete empty columns and combine keywords
            string qtys = "QTY";
            outputFileKeywords = removeEmptyCells(outputFileKeywords);
            outputFileKeywords[0].Add(qtys);
            int indexqty = outputFileKeywords[0].IndexOf(qtys);
            //remove unneccessary columns
            for (int i = 0; i < outputFileKeywords.Count; i++)
            {
                int rowColumns = outputFileKeywords[i].Count;
                outputFileKeywords[i].RemoveRange(2, rowColumns - indexqty - 1);
            }
            ////////////////////////////////////////////////////////////////////////////////////////////
            //If one row has multiple keywords matched to it, we will create more rows to have
            //each row have only one keywords, this will increase the number of hits to that single
            //item but it will allow the user to filter the data by keyword and date.
            //If we keep multiple keywords per work orders then it is not possible to filter by
            //both keyword and date, only by keyword, which limits your ability to look into
            //issues in the last X amount of days.
            ////////////////////////////////////////////////////////////////////////////////////////////
            List<string> tempList = new List<string>();
            for (int i = 1; i < outputFileData.Count; i++)
            {
                if (outputFileData[i].Count > indexKeyw_F + 1)
                {
                    int count = outputFileData[i].Count;
                    for (int j = indexKeyw_F; j < count - 1; j++)
                    {
                        string[] rowArray = outputFileData[i].ToArray();
                        outputFileData[i].RemoveRange(indexKeyw_F + 1, outputFileData[i].Count - indexKeyw_F - 1);
                        i++;
                        outputFileData.Insert(i, rowArray.ToList());
                        outputFileData[i].RemoveAt(indexKeyw_F);
                    }
                }
            }
            //output the FTQ data into a csv file
            writeCSV(outputFileData, outputFilePathData);
            backForegroundColors(ConsoleColor.DarkBlue, ConsoleColor.White);
            Console.WriteLine("");
            Console.WriteLine("Output file has been created at this location:");
            Console.WriteLine(outputFilePathData);

            //output the Keywords data into a csv file
            writeCSV(outputFileKeywords, outputFilePathKeywords);
            backForegroundColors(ConsoleColor.DarkBlue, ConsoleColor.White);
            Console.WriteLine("Output file has been created at this location:");
            Console.WriteLine(outputFilePathKeywords);
            Console.WriteLine("");

            Console.WriteLine("Program is shutting down");
            System.Threading.Thread.Sleep(5000);

            //End of Keywords Microservice

            return 0;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////
        /// Function: removeEmptyCells
        /// Description: This function removes all the cells that are empty, non-empty cells will
        /// shift to the left.
        ////////////////////////////////////////////////////////////////////////////////////////////
        static List<List<string>> removeEmptyCells(List<List<string>> cells)
        {
            for (int i = 0; i < cells.Count; i++)
            {
                //int cellCount = cells[i].Count;
                for (int j = 0; j < cells[i].Count; j++)
                {
                    if (cells[i][j] == "")
                    {
                        cells[i].RemoveAt(j);
                        j--;
                    }
                }
            }
            return cells;
        }

        //////////////////////////////////////////////////////////////////////////////////////
        // THIS STRUCTURE WILL ALLOW ME TO KEEP TRACK OF CAUSE CODES, KEYWORDS AND COUNT
        struct keywordCodesCount
        {
            public string code;
            public IDictionary<string, int> keyWcount;
            public keywordCodesCount(string code, IDictionary<string, int> keyWcount)
            {
                this.code = code;
                this.keyWcount = keyWcount;
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////


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
        // Function: turnToUpper
        // Description: The purpose of this function is to return a list of lists that contains
        // everything in uppercase.
        ///////////////////////////////////////////////////////////////////////////////////////////
        public static List<List<string>> turnToUpper(List<List<string>> csvFile)
        {
            for (int i = 0; i < csvFile.Count; i++)
            {
                for (int j = 0; j < csvFile[i].Count; j++)
                {
                    csvFile[i][j] = csvFile[i][j].ToUpper();
                }
            }

            return csvFile;
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


        ////////////////////////////////////////////////////////////////////////////////////////////
    }
}
