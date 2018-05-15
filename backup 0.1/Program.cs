using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace backup_0._1
{
    class Program
    {
        public int fileCount =0;
        public static string driveList()
        {
            //IList<String> DriveList = new List<String>();
            string dname = "";
            foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
            {
                if (driveInfo.DriveType == DriveType.Removable)
                {
                    dname = driveInfo.RootDirectory.FullName;
                    //DriveList.Add(driveInfo.RootDirectory.FullName);
                }
            }
            //foreach (string driveName in DriveList)
            //{
            //    Console.WriteLine(driveName);
            //}
            Console.WriteLine(dname);
            return dname;
        }

        public static void Copy(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {      
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0} --> {1}", fi.FullName, target.FullName);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }
            
            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
        static void Main(string[] args)
        {
            string sourceDirectory = driveList();
            string targetDirectory = @"C:\Users\SMile\Desktop\Auto backup";

            Copy(sourceDirectory, targetDirectory);
            
            
        }
    }
}
