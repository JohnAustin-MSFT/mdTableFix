using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MDTroubleReport
{
    class PipeSymbols
    {
        FileInfo mFileInfo;
        public PipeSymbols(FileInfo fi) {
            if (!File.Exists(fi.FullName))
            {
                throw new ArgumentException("File is missing");
            }
            mFileInfo = fi;
        }

        public String findBrokenPipes() {
            String brokenPipes = "";
            String[] fileLines = File.ReadAllLines(mFileInfo.FullName);
            int pipeCount = 0;
            int lineCounter = 0;

            foreach (String fileLine in fileLines) {
                lineCounter++;

                if (fileLine.Contains("|:"))
                {
                    pipeCount = fileLine.Count(f => f == '|');
                }
                else if ( fileLine.Contains("|") && pipeCount > 0)
                {
                    if (fileLine.Count(f => f == '|') > pipeCount)
                    {
                        brokenPipes = mFileInfo.FullName;
                        Console.WriteLine("File " + mFileInfo.FullName + ", Broken table row at Line " + lineCounter.ToString());
                        //return brokenPipes;
                    }
                }
                else {
                    pipeCount = 0;
                }
            }
            return brokenPipes;
        }


    }
}
