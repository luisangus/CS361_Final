using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace UserInterfaceCS361
{
    class UserInterface
    {
        static void Main(string[] args)
        {
            //Saves Console background/foreground default colors
            ConsoleColor defaultBackground = Console.BackgroundColor;
            ConsoleColor defaultForeground = Console.ForegroundColor;

            string answer = "";
            while (answer != "EXIT")
            {
                backForegroundColors(defaultBackground, defaultForeground);
                Console.WriteLine("*******************************************************************************");
                Console.WriteLine("This program is used to track manufacturing defects");
                Console.WriteLine("You will be given a list of microservices to select from");
                Console.WriteLine("*******************************************************************************");
                Console.WriteLine("");
                Console.WriteLine("1. Run microservice that will match keywords to defects");
                Console.WriteLine("2. Run Dawn's microservice. It will provide the count of defects per employee");
                Console.WriteLine("3. Run microservice that will count the Total # of items/rows in a file");
                Console.WriteLine("4. Read CSV and display contents");
                Console.WriteLine("5. Exit Program ");
                Console.WriteLine("*******************************************************************************");
                Console.WriteLine("");
                backForegroundColors(ConsoleColor.DarkBlue, ConsoleColor.White);
                Console.WriteLine("Enter the number for the selection you want");
                backForegroundColors(ConsoleColor.DarkGreen, ConsoleColor.White);
                answer = Console.ReadLine();

                string dotnetPath = @"/usr/local/share/dotnet/dotnet";
                string mono = @"/Library/Frameworks/Mono.framework/Mono";
                if (answer == "1") {
                    answer = selectionY_N(answer);
                    //////////////////////////////////////////////////////////
                    if (answer == "Y") {
                        Process keywords = new Process();
                        //keywords.StartInfo.FileName = "/bin/bash";
                        keywords.StartInfo.FileName = @"/Applications/Utilities/Terminal.app/Contents/MacOS/Terminal";
                        //keywords.StartInfo.Arguments = "-c \" " + dotnetPath + " " + "/Users/luisangus/Desktop/Programming/keywordMatching/keywordMatching/bin/Debug/net6.0/keywordMatching.dll" + " \"";
                        keywords.StartInfo.UseShellExecute = false;
                        keywords.StartInfo.RedirectStandardOutput = true;
                        keywords.Start();

                        answer = gotoMenuOrExit();
                    } else {
                        continue;
                    }

                } else if (answer == "2") {
                    answer = selectionY_N(answer);
                    //////////////////////////////////////////////////////////
                    if (answer == "Y") {
                        Process DawnsMicroService = new Process();
                        DawnsMicroService.StartInfo.FileName = @"/usr/local/bin/node";
                        DawnsMicroService.StartInfo.Arguments = "/Users/luisangus/Desktop/Programming/countrowentries/service -i /Users/luisangus/Desktop/Programming/countrowentries/sampledata.csv -o /Users/luisangus/Desktop/Programming/countrowentries/employeecount.csv";
                        DawnsMicroService.Start();

                        Console.WriteLine("Dawn's Microservice has finished running");
                        answer = gotoMenuOrExit();
                    } else {
                        continue;
                    }
                }
                else if (answer == "3") {
                    answer = selectionY_N(answer);
                    //////////////////////////////////////////////////////////
                    if (answer == "Y") {
                        Process totalCount = new Process();
                        totalCount.StartInfo.FileName = dotnetPath;
                        totalCount.StartInfo.Arguments = "/Users/luisangus/Desktop/Programming/countTotalRows/countTotalRows/bin/Debug/net6.0/countTotalRows.dll";
                        //totalCount.StartInfo.Arguments = "/Users/luisangus/Desktop/Programming/countTotalRows/countTotalRows/countTotalRows.exe";
                        totalCount.StartInfo.CreateNoWindow = false;
                        totalCount.StartInfo.UseShellExecute = true;
                        totalCount.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                        totalCount.Start();
                        answer = gotoMenuOrExit();
                    } else {
                        continue;
                    }
                }
                else if (answer == "4") {
                    answer = selectionY_N(answer);
                    //////////////////////////////////////////////////////////
                    if (answer == "Y") {
                        Process readNdisplay = new Process();
                        readNdisplay.StartInfo.FileName = dotnetPath;
                        readNdisplay.StartInfo.Arguments = "/Users/luisangus/Desktop/Programming/readNdisplay/readNdisplay/bin/Debug/net6.0/readNdisplay.dll";
                        readNdisplay.Start();
                        answer = gotoMenuOrExit();
                    }
                    else {
                        continue;
                    }
                }
                else if (answer == "5") {
                    answer = selectionY_N(answer);
                    if (answer == "Y") {
                        backForegroundColors(ConsoleColor.DarkBlue, ConsoleColor.White);
                        Console.WriteLine("");
                        Console.WriteLine("Program is shutting down");
                        System.Threading.Thread.Sleep(2000);
                        Environment.Exit(0);
                    } else {
                        continue;
                    }

                } else {
                    backForegroundColors(ConsoleColor.Red, ConsoleColor.White);
                    Console.WriteLine("");
                    Console.WriteLine("*** You entered an invalid selection ***");
                    Console.WriteLine("");
                }
            }

            //
        }

        ////////////////////////////////////////////////////////////////////////////////////////////
        /// Function: gotoMenuOrExit
        /// Description: This function asks the user if they want to go back to the main menu or
        /// Exit the program. It reads the response and send its back to the main program.
        ////////////////////////////////////////////////////////////////////////////////////////////
        static string gotoMenuOrExit()
        {
            string answer = "";
            while (answer != "2")
            {
                //Console.WriteLine("The Microservice has finished running.");
                backForegroundColors(ConsoleColor.DarkBlue, ConsoleColor.White);
                Console.WriteLine("");
                Console.WriteLine("Do you want to go back to the (1)-Main Menu or (2)-Exit the program.");
                backForegroundColors(ConsoleColor.DarkGreen, ConsoleColor.White);
                answer = Console.ReadLine();

                if (answer == "1")
                {
                    return answer;
                }
                else if (answer == "2")
                {
                    backForegroundColors(ConsoleColor.DarkBlue, ConsoleColor.White);
                    Console.WriteLine("");
                    Console.WriteLine("Program is shutting down");
                    System.Threading.Thread.Sleep(2000);
                    Environment.Exit(0);
                }
                else
                {
                    backForegroundColors(ConsoleColor.Red, ConsoleColor.White);
                    Console.WriteLine("");
                    Console.WriteLine("******************************************************************");
                    Console.WriteLine("You entered an invalid selection. Please enter a valid selection:");
                    Console.WriteLine("******************************************************************");
                    Console.WriteLine("");
                }
            }
            return answer;
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
        /// Function: backForegroundColors
        /// Description: This function changes the curent background/foreground colors back to its
        /// default colors
        ////////////////////////////////////////////////////////////////////////////////////////////
        static string selectionY_N(string selection)
        {
            string Y_N = "";
            string answer = "";
            backForegroundColors(ConsoleColor.DarkBlue, ConsoleColor.White);
            Console.WriteLine("");
            Console.WriteLine("You selected #" + selection);
            Console.WriteLine("Is this correct? (Y/N)");
            backForegroundColors(ConsoleColor.DarkGreen, ConsoleColor.White);
            answer = Console.ReadLine();

            if (answer == "Y")
            {
                Y_N = "Y";
            }
            else if (answer == "N")
            {
                backForegroundColors(ConsoleColor.DarkRed, ConsoleColor.White);
                Console.WriteLine("");
                Console.WriteLine("*** You selected 'N'. Please enter the correct selection ***");
                Console.WriteLine("");
                Y_N = "N";
                return Y_N;
            }
            else
            {
                backForegroundColors(ConsoleColor.Red, ConsoleColor.White);
                Console.WriteLine("");
                Console.WriteLine("***You entered an invalid selection***");
                Console.WriteLine("");
                Y_N = "N";
                return Y_N;
            }

            return Y_N;
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

        //////////////////////////////////////////////////////////////////////////////////////
    }
}