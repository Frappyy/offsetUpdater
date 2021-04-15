using System;
using System.IO;
using IniParser;
using System.Net;
using System.Linq;
using IniParser.Model;
using System.Threading;
using System.Diagnostics;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace offsetUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            //Creating objects
            WebClient wc = new WebClient();
            var parser = new FileIniDataParser();
            Definitions def = new Definitions();

            Console.WriteLine("Checking for version file...");
            CSGOVersionPathGen();

            try
            {
                if(!parser.ReadFile(def.steamInfPath).ToString().Contains(File.ReadAllText(def.csgoVersionPath)))
                {
                    Console.WriteLine("Updating offsets...\n");
                    //Downloads the raw hazedumper offsets from github
                    File.WriteAllText(def.githubOffsetPath, wc.DownloadString(def.hazeDumperLink));
                    //Simply saving Offsets.ini as a .txt for comparison
                    File.WriteAllText(def.textLocalOffsetPath, parser.ReadFile(def.localOffsetPath).ToString());
                    //Simply saving GithubOffsets.ini as a .txt for comparison
                    File.WriteAllText(def.textGithubOffsetPath, parser.ReadFile(def.githubOffsetPath).ToString()); 

                    Console.WriteLine("Looks like CS:GO has updated and your offsets are outdated. Would you like to update them now?");
                    Console.WriteLine("[Y] yes [N] no");

                    //If "answer" equals to 'Y' or "YE" or "YES"
                    if (Console.ReadLine().ToUpper() == "Y" || Console.ReadLine().ToUpper() == "YE" || Console.ReadLine().ToUpper() == "YES" && !DiffCheck(def.textLocalOffsetPath, def.textGithubOffsetPath))
                    {
                        //Downloading all of the up-to-date offsets from github and saving them to a .ini file
                        File.WriteAllText(def.localOffsetPath, wc.DownloadString(def.hazeDumperLink));
                        TerminationSequence("up to date!", true, false);
                    }
                    //If "answer" equals to 'Y' or "YE" or "YES"
                    else if (Console.ReadLine().ToUpper() == "Y" || Console.ReadLine().ToUpper() == "YE" || Console.ReadLine().ToUpper() == "YES" && DiffCheck(def.textLocalOffsetPath, def.textGithubOffsetPath))
                    {
                        Console.WriteLine("CS:GO has been updated but the offsets retrieved from hazedumper haven't been. This requires manual offset dumping.");
                        TerminationSequence("left outdated.", false, true);
                    }
                    else
                    {
                        TerminationSequence("left outdated.", false, true);
                    }

                    TerminationSequence("up to date!", true, false);
                }
                TerminationSequence("up to date!", false, false);
            }
            catch(Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        static bool DiffCheck(string file1, string file2)
        {
            //Compares the local file's offsets to github's offsets
            bool equalOrNot = File.ReadAllLines(file1).SequenceEqual(File.ReadAllLines(file2)); 

            return equalOrNot;
        }

        static void TerminationSequence(string message, bool versionChange, bool pause)
        {
            Definitions def = new Definitions();
            var parser = new FileIniDataParser();

            if (versionChange)
            {
                //Regex pattern that takes version numbers like x.x.xx.x
                Regex pattern = new Regex("\\d+(\\.\\d+)+");
                //Applying the pattern to a steam .inf file containing the patch build of CS:GO to exctract it
                Match m = pattern.Match(parser.ReadFile(def.steamInfPath).ToString()); 

                //Writing the retrieved version value from "Match m" to the .txt file in your docuements
                File.WriteAllText(def.csgoVersionPath, m.Value);
            }

            Console.WriteLine("Deleting temporary comparison files...");
            try
            {
                //Deleting the .txt files saved previously for comparison since we don't need them anymore
                File.Delete(def.textGithubOffsetPath);
                File.Delete(def.textLocalOffsetPath);
                File.Delete(def.githubOffsetPath);

            }
            catch(Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                Console.ReadLine();
                Environment.Exit(0);
            }
            Console.WriteLine("Offsets are " + message);

            if (pause)
            {
                Console.ReadLine();
            }
        }

        static void CSGOVersionPathGen()
        {
            Definitions def = new Definitions();

            if (!Directory.Exists(def.csgoVersionFolderPath)) //If directory doesn't exist
            {
                //Creating the directory for holding the patch version number of CS:GO
                Directory.CreateDirectory(def.csgoVersionFolderPath); 

                if (!File.Exists(def.csgoVersionPath)) //If file doesn't exist
                {
                    File.WriteAllText(def.csgoVersionPath, "1.1.1.1"); //Creating the .txt file containing the patch version number of CS:GO
                }
            }
        }
    }
}
