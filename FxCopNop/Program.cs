using System;
using System.IO;

namespace FxCopNop
{
    class Program
    {
        static void Main(string[] args)
        {
            var myLocation = System.Reflection.Assembly.GetEntryAssembly().Location;            
            
            var fileName = Path.GetFileName(myLocation);
            if (string.IsNullOrEmpty(myLocation) || string.IsNullOrEmpty(fileName))
            {
                Console.WriteLine("Unable to determine our path");
                return;
            }

            // Running as fxcopcmd.exe return 0;
            if (fileName.Equals("FxCopCmd.exe", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            // Help
            if (args.Length != 1)
            {
                Console.WriteLine("-e Enable FXCop");
                Console.WriteLine("-d Disable FXCop");
                return;
            }

            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                                            "Microsoft Visual Studio 11.0", "Team Tools", "Static Analysis Tools",
                                            "FxCop");
            
            string fxCopExe = Path.Combine(path, "FxCopCmd.exe");
            string fxCopExeBak = Path.Combine(path, "FxCopCmd.exe.bak");

            if (!File.Exists(fxCopExe))
            {
                Console.WriteLine("FxCopCmd.Exe not found");
                return;
            }

            // Backup
            if (!File.Exists(fxCopExeBak))
            {
                File.Copy(fxCopExe, fxCopExeBak, true);
            }

            if (args[0].Equals("-d", StringComparison.OrdinalIgnoreCase))
            {
                File.Copy(myLocation, fxCopExe, true);
            }
            else if (args[0].Equals("-e", StringComparison.OrdinalIgnoreCase))
            {
                File.Copy(fxCopExeBak, fxCopExe, true);
            }
            else
            {
                Console.WriteLine("Invalid option");
            }
        }
    }
}
