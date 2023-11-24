using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace tieba_sign
{
    class tieba_func
    {
        public static string get_follow_url = "https://tieba.baidu.com/mo/q/newmoindex";
        public static string set_sign_url = "http://c.tieba.baidu.com/c/c/forum/sign";

        public static string[] get_follow()
        {
            string[] result = new string[] { "", "" };
            try
            {
                JObject jobj = JObject.Parse(http.Get(get_follow_url, true));
                result[0] = jobj["data"]["itb_tbs"].ToString();
                result[1] = jobj["data"]["like_forum"].ToString();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return result;
        }

        public static bool set_sign(string tbs, string name)
        {
            string post_data = "kw=" + name.Replace("+", "%2B") + "&tbs=" + tbs + "&sign=" + EncryptandDecipher.md5jm("kw=" + name + "tbs=" + tbs + "tiebaclient!!!");

            JObject jobj = new JObject();
            try { jobj = JObject.Parse(http.Post(set_sign_url, post_data)); }
            catch (Exception ex) { jobj = JObject.FromObject(new { error_code = "Exception", error_msg = "解码签到状态时出现错误：" + ex.Message }); }

            if ("0".Equals(jobj["error_code"].ToString()))
            {
                Console.WriteLine("(" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + ") [" + name + "] 签到成功。服务器返回：" + jobj["forum"][0]["window_conf"]["text"]);
                return true;
            }
            else 
            {
                Console.WriteLine("(" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + ") [" + name + "] 签到失败。服务器返回：" + jobj["error_code"] + ", " + jobj["error_msg"]);
                return false;
            }
        }


    }
}
