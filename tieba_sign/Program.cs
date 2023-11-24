using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace tieba_sign
{
    class Program
    {
        static void Main(string[] args)
        {
            //支持TLS1.2
            System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)192 | (System.Net.SecurityProtocolType)768 | (System.Net.SecurityProtocolType)3072;

            //签到方法
            if (file_ctrl.Get().bduss.Length < 1) { Console.WriteLine("按一个键退出。。"); Console.ReadKey(); return; }
            if (file_ctrl.Get().sendkey.Length < 34) { Console.WriteLine("由于您未在配置文件中的第二行配置Server酱的SendKey或长度异常，每日首次签到状态不会推送到微信。\n----------"); }

            Console.WriteLine("贴吧自动签到EXE已启动。");
            Auto_Sign(false);
            while (true)
            {
                //Console.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                switch (DateTime.Now.ToString("HH:mm"))
                {
                    case "00:01":
                        Auto_Sign(true);
                        break;
                    case "01:00":
                    case "05:00":
                    case "09:00":
                    case "12:00":
                    case "22:00":
                    case "23:40":
                        Auto_Sign(false);
                        break;
                    default:
                        break;
                }
                Thread.Sleep(50000);
            }
        }
        static void Auto_Sign(bool send_wechat)
        {
            //取信息
            string[] data = tieba_func.get_follow();
            int success_count = 0; int faild_count = 0;
            JArray jarray = new JArray();
            try { jarray = JArray.Parse(data[1]); }
            catch (Exception ex) { Console.WriteLine("解码关注列表时出现错误：" + ex.Message + "\n" + data[1]); }

            foreach (JToken jtoken in jarray)
            {
                if (jtoken["is_sign"].ToString().Equals("1"))
                {
                    Console.WriteLine(string.Format("({0}) [{1}](LV{2}|{3}EXP)，已经签到了，不再重复执行。", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), jtoken["forum_name"], jtoken["user_level"], jtoken["user_exp"]));
                }
                else
                {
                    Console.WriteLine(string.Format("({0}) [{1}](LV{2}|{3}EXP)，现在未签到，现将立即执行。", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), jtoken["forum_name"], jtoken["user_level"], jtoken["user_exp"]));
                    if (tieba_func.set_sign(data[0], jtoken["forum_name"].ToString())) { success_count++; }
                    else { faild_count++; }
                    Thread.Sleep(1500);
                }
            }
            if (send_wechat)
            {
                string send_key = file_ctrl.Get().sendkey;
                if (send_key.Length >= 34)
                {
                    string send_msg = string.Format("贴吧签到反馈: 成功{0}, 失败{1}", success_count, faild_count);
                    Console.WriteLine("推送信息到微信：[" + send_msg + "]");
                    http.Get(@"http://sctapi.ftqq.com/" + send_key + ".send?title=" + send_msg, false);
                }
            }
            Console.WriteLine(string.Format("一个周期已完成。成功签到数：{0}，失败签到数：{1}\n----------", success_count, faild_count));
        }
    }
}
