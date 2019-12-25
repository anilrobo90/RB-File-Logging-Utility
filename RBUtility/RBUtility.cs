using System;
using System.IO;
using System.Text;


namespace RBUtility
{
    public static class RBUtility
    {
        public static void WriteLog(string strMessage, string LogFileName = "RBUtility")
        {
            string Dt4 = DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy").Substring(0, 2);
            StringBuilder fileFormat = new StringBuilder();

            fileFormat.Append(AppDomain.CurrentDomain.BaseDirectory);
            fileFormat.Append("\\" + LogFileName + "Log");
            fileFormat.Append(DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy").Substring(0, 2));
            fileFormat.Append(DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy").Substring(3, 2));
            fileFormat.Append(DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy").Substring(6, 4));
            fileFormat.Append(".log");

            if (File.Exists(fileFormat.ToString()))
            {
                File.Delete(fileFormat.ToString());
            }

            fileFormat.Clear();

            fileFormat.Append(AppDomain.CurrentDomain.BaseDirectory);
            fileFormat.Append("\\" + LogFileName + "Log");
            fileFormat.Append(DateTime.Now.Date.ToString("dd/MM/yyyy").Substring(0, 2));
            fileFormat.Append(DateTime.Now.Date.ToString("dd/MM/yyyy").Substring(3, 2));
            fileFormat.Append(DateTime.Now.Date.ToString("dd/MM/yyyy").Substring(6, 4));
            fileFormat.Append(".log");

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileFormat.ToString(), true))
            {
                file.AutoFlush = false;
                file.WriteLine(System.DateTime.Now.TimeOfDay.ToString().Substring(0, 5) + " " + strMessage);
            }
        }

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

        public static string ReadLine(string file_name, char separator = '=', int no_of_lines = 1)
        {
            using (StreamReader file = new StreamReader(file_name))
            {
                if (no_of_lines == 1)
                {
                    return file.ReadLine().Split(separator)[1].ToString();
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
    }

}

