using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace TowerMonitor.util
{
    public class FileUtil
    {
      
        public static bool WriteFile(String file, string content) {
            bool isSuccess = true;

            try {
                StreamWriter sw = new StreamWriter(file);

                sw.Write(content);
                sw.Close();
            } catch (Exception ex)
            {
                isSuccess = false;
            }           
            return isSuccess;
        }

        public static string ReadFile(string file)
        {

            String content = "";
            try {
                content = File.ReadAllText(file);
            } catch(Exception ex)
            {
               // do nothing
            }

            return content;
        }

        public static bool CopyFile(string sourceFile, string destinationFile) { 
            bool isSuccess = true;

            try
            {
                File.Copy(sourceFile, destinationFile, true);
            }
            catch (IOException iox)
            {
                isSuccess = false;
                Console.WriteLine(iox.Message);
            }

            return isSuccess;
        }
    }
}
