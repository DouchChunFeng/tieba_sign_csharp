using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace tieba_sign
{
    class EncryptandDecipher
    {
        public static string md5jm(string str)
        {
            MD5 md5 = MD5.Create();
            byte[] byteArray = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            string resultString = "";
            foreach (byte b in byteArray)
            {
                resultString += b.ToString("x2"); //将字节数组转成16进制的字符串。X表示16进制，2表示每个16字符占2位
            }
            return resultString;
        }

    }
}
