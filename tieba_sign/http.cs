using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace tieba_sign
{
    class http
    {
        public static string Get(string url, bool baidu)
        {
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Timeout = 15000;
                webRequest.Method = "GET";
                webRequest.KeepAlive = false;
                if (baidu)
                {
                    webRequest.Host = "tieba.baidu.com";
                    webRequest.KeepAlive = true;
                    webRequest.UserAgent = "Mozilla/5.0 (Linux; Android 10; EVR-AN00) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Mobile Safari/537.36";
                    webRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                    webRequest.Headers["Cookie"] = "BDUSS=" + file_ctrl.Get().bduss;
                }
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                StreamReader sr = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8);
                return sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }

        public static string Post(string url, string post_data)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(post_data);
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Timeout = 15000;
                webRequest.Method = "Post";
                webRequest.Host = "tieba.baidu.com";
                webRequest.KeepAlive = true;
                webRequest.UserAgent = "Mozilla/5.0 (Linux; Android 10; EVR-AN00) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Mobile Safari/537.36";
                webRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                webRequest.Headers["Cookie"] = "BDUSS=" + file_ctrl.Get().bduss;
                webRequest.ContentLength = data.Length;

                using (Stream stream = webRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                using (Stream responseStream = webResponse.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

    }
}
