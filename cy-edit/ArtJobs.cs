using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MDTroubleReport
{
    class ArtJobs
    {
        FileInfo mFileInfo;
        public ArtJobs(FileInfo fi)
        {
            if (!File.Exists(fi.FullName))
            {
                throw new ArgumentException("File is missing");
            }
            mFileInfo = fi;
        }

        public String findMissingAltText() {
            String noAltText = "";
            String[] fileLines = File.ReadAllLines(mFileInfo.FullName);
            int lineCounter = 0;

            foreach (String fileLine in fileLines)
            {
                lineCounter++;

                if (fileLine.Contains("![]"))
                {
                    noAltText = mFileInfo.FullName;
                    Console.WriteLine("File " + mFileInfo.FullName + ", Missing alt-text at Line " + lineCounter.ToString());
                }
            }
            return noAltText;
        }
        public void ReportAltText() {
            String[] fileLines = File.ReadAllLines(mFileInfo.FullName);
            int lineCounter = 0;

            Regex rgx = new Regex(@" !\x5B \w+ \s",  RegexOptions.IgnorePatternWhitespace);
            foreach (String fileLine in fileLines)
            {
                lineCounter++;
                MatchCollection matches = rgx.Matches(fileLine);

                if (matches.Count > 0)
                {
                    foreach (Match match in matches) {
                        Console.WriteLine("File " + mFileInfo.FullName + ", alt-text " + match.Value + " at Line " + lineCounter.ToString());
                    }
                }
            }
            return;

        }

    }
}
