using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace BlitzkriegSoftware.PCF.CFEnvParser.Test.Helpers
{
    public static class TestHelper
    {
        public static string UPS_Folder()
        {
            string folder = string.Empty;

            var cd = Environment.CurrentDirectory;
            var di = new DirectoryInfo(cd);

            recurseFolders(ref folder, di, "UPS");

            return folder;
        }

        private static void recurseFolders(ref string folder, DirectoryInfo di, string folderToFind)
        {
            foreach(var fl in di.GetDirectories())
            {
                if(fl.Name == folderToFind)
                {
                    folder = fl.FullName;
                    return;
                }
            }

            if (di.FullName == di.Root.FullName) return;

            recurseFolders(ref folder, di.Parent, folderToFind);
        }

    }
}
