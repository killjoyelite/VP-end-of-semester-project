using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;


namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }
        public void OnDebug()
        {
            OnStart(null);
        }

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
            //Console.WriteLine(dname);
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

        protected override void OnStart(string[] args)
        {
            DateTime dt = DateTime.Now;
            
            string startPath = @"C:\Users\SMile\Documents\Visual Studio 2010\Projects\WindowsService1\WindowsService1\bin\Release\OnStart.txt";
            string errorPath = @"C:\Users\SMile\Documents\Visual Studio 2010\Projects\WindowsService1\WindowsService1\bin\Release\ErrorLog.txt";
            
            string sourceDirectory = driveList();
            string targetDirectory = @"C:\Users\SMile\Desktop\Auto backup";

            System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + "OnStart.txt");
            using (StreamWriter sw = File.AppendText(startPath))
            {
                sw.WriteLine(dt + " - Backup Service Started");
                sw.WriteLine("\n");
            }

            //File.AppendAllText(@"C:\Users\SMile\Documents\Visual Studio 2010\Projects\WindowsService1\WindowsService1\bin\Release\OnStart.txt", "The service has started");
            
            try
            {
                Copy(sourceDirectory, targetDirectory);
            }
            catch (Exception e)
            {
                
                using (StreamWriter sw = File.AppendText(errorPath))
                {
                    sw.WriteLine(dt + " - " + e.Message);
                    sw.WriteLine("\n");
                }
                
                //File.AppendAllText(@"C:\Users\SMile\Documents\Visual Studio 2010\Projects\WindowsService1\WindowsService1\bin\Release\ErrorLog.txt", "Device not found");
            }
            
            Console.WriteLine("Total files copied: \n");
            Console.WriteLine();
        }

        protected override void OnStop()
        {
            string stopPath = @"C:\Users\SMile\Documents\Visual Studio 2010\Projects\WindowsService1\WindowsService1\bin\Release\OnStop.txt";
            DateTime dt = DateTime.Now;

            System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + "OnStop.txt");
            using (StreamWriter sw = File.AppendText(stopPath))
            {
                sw.WriteLine(dt + " - Backup Service Stopped");
                sw.WriteLine("\n");
            }
        }
    }
}
