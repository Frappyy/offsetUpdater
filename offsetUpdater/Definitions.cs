using System;
using System.IO;

namespace offsetUpdater
{
    class Definitions
    {
        //Seperate class file for cleaner code & future proofing
        public string localOffsetPath = "C:\\Mediocre\\Offsets.ini";
        public string githubOffsetPath = "C:\\Mediocre\\GithubOffsets.ini";
        public string steamInfPath = "G:\\Games\\Steam Library\\steamapps\\common\\Counter-Strike Global Offensive\\csgo\\steam.inf";
        public string csgoVersionPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\OffsetUpdater\\csgoVersion.txt";
        public string csgoVersionFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\OffsetUpdater";

        public string hazeDumperLink = "https://raw.githubusercontent.com/frk1/hazedumper/master/csgo.toml";

        public string textLocalOffsetPath = "C:\\Mediocre\\Offsets.txt";
        public string textGithubOffsetPath = "C:\\Mediocre\\GithubOffsets.txt";
    }
}
