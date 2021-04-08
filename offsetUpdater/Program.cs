using System;
using IniParser;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace offsetUpdater
{
    class Program
    {


        static void Main(string[] args)
        {
            Console.WriteLine("Checking for updated offsets...");

            //Defining the path's of files
            string localOffsetPath = "C:\\Mediocre\\Offsets.ini";
            string githubOffsetPath = "C:\\Mediocre\\GithubOffsets.ini";

            string textLocalOffsetPath = "C:\\Mediocre\\Offsets.txt";
            string textGithubOffsetPath = "C:\\Mediocre\\GithubOffsets.txt";

            WebClient wc = new WebClient(); //Initializing WebClient
            var parser = new FileIniDataParser(); //Initializing .ini parser

            File.WriteAllText(githubOffsetPath, wc.DownloadString("https://raw.githubusercontent.com/frk1/hazedumper/master/csgo.toml")); //Downloading all of the up-to-date offsets from github and saving them to a .ini file for writing
            
            File.WriteAllText(textLocalOffsetPath, parser.ReadFile(localOffsetPath).ToString()); //Simply saving Offsets.ini as a .txt for comparison
            File.WriteAllText(textGithubOffsetPath, parser.ReadFile(githubOffsetPath).ToString()); //Simply saving GithubOffsets.ini as a .txt for comparison

            //TODO: Add failsafes and proper exception handling
            //TODO: Add failsafes and proper exception handling
            //TODO: Add failsafes and proper exception handling

            if(!DiffCheck(textLocalOffsetPath, textGithubOffsetPath)) //If there's a difference (false)
            {
                Console.WriteLine("Looks like your offsets are outdated. Would you like to update them now?");
                Console.WriteLine("[Y] yes [N] no");
                string answer = Console.ReadLine(); //Storing user's input into a string called "answer"

                if(answer.ToUpper() == "Y") //If "answer" equals to 'Y'
                {
                    Console.WriteLine(""); //Extra line to seperate the user's answer from the program's messages
                    Console.WriteLine("Overwriting offsets...");
                    File.WriteAllText(localOffsetPath, wc.DownloadString("https://raw.githubusercontent.com/frk1/hazedumper/master/csgo.toml")); //Downloading all of the up-to-date offsets from github and saving them to the original .ini file for use

                    Console.WriteLine("Deleting temporary comparison files...");
                    File.Delete(textGithubOffsetPath);
                    File.Delete(textLocalOffsetPath);
                    File.Delete(githubOffsetPath);

                    Console.WriteLine("Everything is up-to-date!");
                    //Console.ReadLine(); for debugging purposes later
                }
                else
                {
                    Console.WriteLine("Deleting temporary comparison files...");
                    File.Delete(textGithubOffsetPath);
                    File.Delete(textLocalOffsetPath);
                    File.Delete(githubOffsetPath);

                    Console.WriteLine("Offsets left unupdated.");
                    //Console.ReadLine(); for debugging purposes later
                }
            }
            else
            {
                Console.WriteLine("Deleting temporary comparison files...");
                File.Delete(textGithubOffsetPath);
                File.Delete(textLocalOffsetPath);
                File.Delete(githubOffsetPath);

                Console.WriteLine("Everything is up-to-date!");
                //Console.ReadLine(); for debugging purposes later
            }

            //Console.ReadLine(); for debugging purposes later
        }
        static bool DiffCheck(string file1, string file2)
        {
            bool equalOrNot = File.ReadAllLines(file1).SequenceEqual(File.ReadAllLines(file2)); //Compares the local file's offsets to github's offsets

            return equalOrNot;
        }
    }
}
