using System;
using System.IO;
using IniParser;
using System.Net;
using System.Linq;

namespace offsetUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            WebClient wc = new WebClient(); //Initializing WebClient
            var parser = new FileIniDataParser(); //Initializing .ini parser

            //Defining the path's of files
            string localOffsetPath = "C:\\Mediocre\\Offsets.ini";
            string githubOffsetPath = "C:\\Mediocre\\GithubOffsets.ini";

            string textLocalOffsetPath = "C:\\Mediocre\\Offsets.txt";
            string textGithubOffsetPath = "C:\\Mediocre\\GithubOffsets.txt";

            try
            {
                //Downloading all of the up-to-date offsets from github and saving them to a .ini file for writing
                File.WriteAllText(githubOffsetPath, wc.DownloadString("https://raw.githubusercontent.com/frk1/hazedumper/master/csgo.toml"));
                File.WriteAllText(textLocalOffsetPath, parser.ReadFile(localOffsetPath).ToString()); //Simply saving Offsets.ini as a .txt for comparison
                File.WriteAllText(textGithubOffsetPath, parser.ReadFile(githubOffsetPath).ToString()); //Simply saving GithubOffsets.ini as a .txt for comparison            
            }
            catch(Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                Console.ReadLine();
                Environment.Exit(0);
            }

            Console.WriteLine("Checking for updated offsets...");

            if(!DiffCheck(textLocalOffsetPath, textGithubOffsetPath)) //If there's a difference (false)
            {
                Console.WriteLine("Looks like your offsets are outdated. Would you like to update them now?");
                Console.WriteLine("[Y] yes [N] no");
                string answer = Console.ReadLine(); //Storing user's input into a string called "answer"

                if(answer.ToUpper() == "Y") //If "answer" equals to 'Y'
                {
                    Console.WriteLine(""); //Extra line to seperate the user's answer from the program's messages
                    Console.WriteLine("Overwriting offsets...");

                    try
                    {
                        //Downloading all of the up-to-date offsets from github and saving them to a .ini file for writing
                        File.WriteAllText(localOffsetPath, wc.DownloadString("https://raw.githubusercontent.com/frk1/hazedumper/master/csgo.toml"));
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("Error: " + e);
                        Console.ReadLine();
                        Environment.Exit(0);
                    }

                    Console.WriteLine("Deleting temporary comparison files...");
                    FileDetetion();

                    Console.WriteLine("Everything is up-to-date!");
                    //Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Deleting temporary comparison files...");
                    FileDetetion();

                    Console.WriteLine("Offsets left unupdated.");
                    //Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("Deleting temporary comparison files...");
                FileDetetion();

                Console.WriteLine("Everything is up-to-date!");
                //Console.ReadLine();
            }

            //Console.ReadLine();
        }
        static bool DiffCheck(string file1, string file2)
        {
            bool equalOrNot = File.ReadAllLines(file1).SequenceEqual(File.ReadAllLines(file2)); //Compares the local file's offsets to github's offsets

            return equalOrNot;
        }

        static void FileDetetion()
        {
            string githubOffsetPath = "C:\\Mediocre\\GithubOffsets.ini";
            string textLocalOffsetPath = "C:\\Mediocre\\Offsets.txt";
            string textGithubOffsetPath = "C:\\Mediocre\\GithubOffsets.txt";

            try
            {
                File.Delete(textGithubOffsetPath);
                File.Delete(textLocalOffsetPath);
                File.Delete(githubOffsetPath);
            }
            catch(Exception e)
            {
                Console.WriteLine("Error: " + e);
                Console.ReadLine();
                Environment.Exit(0);
            }
        }
    }
}
