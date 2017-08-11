using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace MDTroubleReport
{
    class Program
    {
        //TODO: These private const values are the argument. They need to become command line arguments

        //FOLDER - the location where to apply yaml metadata. the tag update is applie to all .md files in this folder, AND its subfolders.
        //private const string FOLDER = @"E:\projects\content-yaml-editor\";
        private const string FOLDER = @"E:\projects\VBA-content\VBA";

        // TAG - The name of the tag to update. It is the left side of the format such as "tag: value"
        private const string TAG = "ms.suite";

        //VALUE - The value to apply to the tag. If the value is an empty string, then the tag will be removed. If an existing tag is found, it will be updated with this new value
        //        If the tag does not exist, then it will be inserted
        private const string VALUE = "Office365";

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("");
                Console.WriteLine("Syntax: doc-yaml-editor <FOLDER> <TAG> <VALUE>");
                Console.WriteLine("------------------------------------------");
                Console.WriteLine("FOLDER - The location where yaml metadata is applied. The tag update is applie to all .md files in this folder, AND its subfolders.");
                Console.WriteLine("TAG - OPTIONAL. The name of the tag to update. It is the left side of the format such as 'tag: value'");
                Console.WriteLine("VALUE - REQUIRED if TAG supplied. The value to apply to the tag. The argument is treated as a string. Do not quote the string value. If the value is an empty string, then the tag will be removed. If an existing tag is found, it will be updated with this new value");
                return;
            }

            try
            {
                String folder = args[0];

                if (args.Length == 2)
                    ProcessMDFiles(args[0], args[1], false);
                if (args.Length == 3)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Parameter values: " + args[0] + " " + args[1] + " " + args[2]);

                }
            }
            catch (DirectoryNotFoundException dnf)
            {
                Console.WriteLine(dnf.Message);
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

        }

        /// <summary>
        /// Create a Yaml section in all files missing one in the identified folder and subfolders. .md files only. This will also add the title tag
        /// using the value of the H1 # header in the content.
        /// </summary>
        /// <param name="folderName">The folder to update files in (plus all subfolders received yaml sections in .md files</param>
        /// <param name="overWrite">true if an existing title should be overwritten with the header 1 value; otherwise false</param>
        static void ProcessMDFiles(string folderName, String job, bool overWrite)
        {

            //loop through each .md file found in this and subfolders.
            DirectoryInfo di = new DirectoryInfo(folderName);
            FileInfo[] fileList = di.GetFiles("*.md", System.IO.SearchOption.AllDirectories);
            foreach (FileInfo fi in fileList)
            {
                try
                {
                    // Determine if entry is really a directory
                    if ((fi.Attributes & FileAttributes.Directory) != FileAttributes.Directory)
                    {

                        switch (job)
                        {
                            case "pipe":
                                PipeSymbols pipeSymbols = new PipeSymbols(fi);
                                if (pipeSymbols.findBrokenPipes().Length > 0)
                                {
                                    Console.WriteLine("Broken table found in " + fi.FullName);
                                }
                                break;
                            case "alt-text":
                                ArtJobs artJobs = new ArtJobs(fi);
                                if (artJobs.findMissingAltText().Length > 0) {
                                    Console.WriteLine("Missing alt-text in " + fi.FullName);
                                }
                                break;
                            case "alt-text-report":
                                ArtJobs altTextReport = new ArtJobs(fi);
                                altTextReport.ReportAltText();
                                break;
                            default:
                                return;
                        }
                    }
                    else {
                        Console.WriteLine("Processing directory " + fi.DirectoryName);
                    }
                }
                catch (ArgumentException e) {
                    Console.WriteLine("Folder: " + fi.DirectoryName);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: Bad Yaml Found in File: " + fi.FullName);
                }
            }
        }
    }
}
