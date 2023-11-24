using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace tieba_sign
{
    class file_ctrl
    {
        private static file_ctrl instance = null;
        private string file_location = Environment.CurrentDirectory + @"\bduss.txt";
        private string user_bduss = "";
        private string serverchan_sendkey = "";
        public static file_ctrl Get()
        {
            if (instance == null) instance = new file_ctrl();
            return instance;
        }
        private file_ctrl() 
        {
            if (File.Exists(file_location))
            {
                string[] file_line = File.ReadAllLines(file_location);
                if (file_line.Length < 1 || file_line[0].Length < 1) { Console.WriteLine("bduss为空无法继续。"); }
                else 
                {
                    user_bduss = file_line[0];
                    if (file_line.Length >= 2) { serverchan_sendkey = file_line[1]; }
                }
            }
            else
            {
                Console.WriteLine("请在新创建的文件中首行粘贴bduss, 如需Server酱推送请将网页上的SendKey粘贴到文本中的第二行.");
                File.CreateText(file_location);
            }
        }
        public string bduss
        {
            get { return user_bduss; }
            set { user_bduss = value; }
        }
        public string sendkey
        {
            get { return serverchan_sendkey; }
            set { serverchan_sendkey = value; }
        }
    }
}
