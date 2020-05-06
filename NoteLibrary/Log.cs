using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NoteLibrary
{
    class Log
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="file_name"></param>
        /// <returns></returns>
        private static bool writetofile(string txt, string file_name)
        {
            FileInfo fi = new FileInfo(file_name);
            if (!Directory.Exists(fi.DirectoryName))
            {
                Directory.CreateDirectory(fi.DirectoryName);
            }
            txt = DateTime.Now.ToString("HH:mm:ss") + txt;
            try
            {
                using (FileStream sw = new FileStream(file_name, FileMode.Append, FileAccess.Write))
                    if (File.Exists(file_name))
                    {
                        StreamWriter fs = new StreamWriter(sw);
                        // 为文件添加一些文本内容
                        fs.WriteLine(txt);
                        fs.Close();
                        return true;
                    }
                    else
                    {
                        using (StreamWriter fs = new StreamWriter(sw))
                        {
                            fs.WriteLine(txt);
                            fs.Close();
                            return true;
                        }
                    }
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="x"></param>
        public static void ErrorLog(string x)
        {
            string err_name = "Syslog\\Error_log" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".txt";
            writetofile(x, err_name);
        }
        /// <summary>
        /// 日常操作日志
        /// </summary>
        /// <param name="x"></param>
        public static void ActionLog(string x)
        {
            string act_name = "Syslog\\Action_log" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".txt";
            writetofile(x, act_name);
        }
    }
}


