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
            WebClient wc = new WebClient();
            var parser = new FileIniDataParser();

            string localOffsetPath = "C:\\Mediocre\\Offsets.ini";
            string githubOffsetPath = "C:\\Mediocre\\GithubOffsets.txt";

            File.WriteAllText(githubOffsetPath, wc.DownloadString("https://raw.githubusercontent.com/frk1/hazedumper/master/csgo.toml"));
            File.WriteAllText("C:\\Mediocre\\Offsets.txt", parser.ReadFile(localOffsetPath).ToString());

            if(!DiffCheck("C:\\Mediocre\\Offsets.txt", githubOffsetPath))
            {
                Console.WriteLine("Looks like your offsets are outdated. Would you like to update them now?");
                Console.WriteLine("[Y] yes [N] no");
                string answer = Console.ReadLine();

                if(answer == "y")
                {

                }
            }
            else
            {
                Console.WriteLine("Everything is up-to-date!");
            }
            
            Console.ReadLine();
        }
        static bool DiffCheck(string file1, string file2)
        {
            bool equalOrNot = File.ReadAllLines(file1).SequenceEqual(File.ReadAllLines(file2));

            return equalOrNot;
        }
    }
}
