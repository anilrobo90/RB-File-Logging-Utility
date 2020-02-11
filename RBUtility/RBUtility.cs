using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace RBUtility
{
    public static class Utils
    {
        /// <summary>
        /// Writes Message/Data to the Log File.
        /// </summary>
        /// <param name="strMessage"> Message to be written to the log File.</param>
        /// <param name="LogFileName">Name and location of the log file. Default name is "RBUtility"</param>
        public static void WriteLog(string strMessage, string LogFileName = "RBUtility")
        {
            string Dt4 = DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy",CultureInfo.InvariantCulture).Substring(0, 2);
            StringBuilder fileFormat = new StringBuilder();

            fileFormat.Append(AppDomain.CurrentDomain.BaseDirectory);
            fileFormat.Append("\\" + LogFileName + "Log");
            fileFormat.Append(DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Substring(0, 2));
            fileFormat.Append(DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Substring(3, 2));
            fileFormat.Append(DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Substring(6, 4));
            fileFormat.Append(".log");

            if (File.Exists(fileFormat.ToString()))
            {
                File.Delete(fileFormat.ToString());
            }

            fileFormat.Clear();

            fileFormat.Append(AppDomain.CurrentDomain.BaseDirectory);
            fileFormat.Append("\\" + LogFileName + "Log");
            fileFormat.Append(DateTime.Now.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Substring(0, 2));
            fileFormat.Append(DateTime.Now.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Substring(3, 2));
            fileFormat.Append(DateTime.Now.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Substring(6, 4));
            fileFormat.Append(".log");

            /// <exception cref="IOException"></exception>
            using (StreamWriter file = new StreamWriter(fileFormat.ToString(), true))
            {
                file.AutoFlush = false;
                file.WriteLine(System.DateTime.Now.TimeOfDay.ToString().Substring(0, 5) + " " + strMessage);
            }
        }

        /// <summary>
        /// Creates a Connection string for MSSQL Server connection.
        /// This function exepects the configuration file to be written in this order
        /// Server Name
        /// Database Name
        /// Username
        /// Password
        /// 
        /// The username and password are to be written as plaintext, in future it will be encrypted.
        /// </summary>
        /// <param name="IniFileName"> Configuration file where all the configurtaion parameters are mentioned in prescribed format.</param>
        /// <returns>Configration file in MSSQL format.</returns>
        public static string CreateDBconnectionString(string IniFileName)
        {
            string ConnectionString = string.Empty;

            string[] readlines = ReadLine(IniFileName, '=', 4).Split('=');

            string C_ServerName = readlines[0];

            string C_DbName = readlines[1];

            string userName = readlines[2];

            string password = readlines[3];

            ConnectionString = "Data Source=" + C_ServerName + ";initial catalog=" + C_DbName + ";user Id=" + userName + ";password=" + password + "";

            return ConnectionString;
        }

        /// <summary>
        /// This function is used to read configuration from a File. 
        /// The function assumes that each configuration is followed by new line "\r\n"
        /// </summary>
        /// <param name="file_name"> Name Of The File along with the location of the file. </param>
        /// <param name="separator"> Separator to used to separate config files.  Default separator is "="</param>
        /// <param name="no_of_lines"> No of configuration Lines present in the file. Default value is 1 </param>
        /// <returns>Contents from the configuration filealong with added separator to distinguish betwwen configuration parameters.</returns>
        public static string ReadLine(string fileName, char separator = '=', int noOfLines = 1)
        {
            /// <exception cref="FileNotFoundException"></exception>
            using (StreamReader file = new StreamReader(fileName))
            {
                if (noOfLines == 1)
                {
                    return file.ReadLine().Split(separator)[1].ToString(CultureInfo.InvariantCulture);
                }

                else
                {
                    string data = file.ReadToEnd();
                    string[] lines = data.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    StringBuilder ret_values = new StringBuilder();
                    for (int i = 0; i < lines.Length; i++)
                    {
                        ret_values.Append(lines[i].Split(separator)[1]).Append(separator);
                    }
                    return ret_values.ToString();
                }
            }
        }

        /// <summary>
        ///  Creates a SHA2 512 bit hash
        /// </summary>
        /// <param name="Phrase"> string to be encrypted</param>
        /// <returns>SHA2 Hash without equals</returns>
        public static string CreateSHA2Hash(string Phrase)
        {
            Byte[] PhraseAsByte;
            Byte[] EncryptedBytes;
            using (SHA512Managed HashTool = new SHA512Managed())
            {
                PhraseAsByte = System.Text.Encoding.UTF8.GetBytes(string.Concat(Phrase));
                EncryptedBytes = HashTool.ComputeHash(PhraseAsByte);
                HashTool.Clear();
            }

            return Convert.ToBase64String(EncryptedBytes).Replace("==","");
        }
    }

}

