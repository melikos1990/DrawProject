using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using SMARTII.Domain.Cache;

namespace SMARTII.Domain.Security
{
    public static class SecurityUtility
    {
        public static string AesEncryptBase64(this string sourceStr, string cryptoKey)
        {
            string encrypt = "";
            try
            {
                AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
                byte[] key = sha256.ComputeHash(Encoding.UTF8.GetBytes(cryptoKey));
                byte[] iv = md5.ComputeHash(Encoding.UTF8.GetBytes(cryptoKey));
                aes.Key = key;
                aes.IV = iv;

                if (sourceStr == null) return null;

                byte[] dataByteArray = Encoding.UTF8.GetBytes(sourceStr);
                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(dataByteArray, 0, dataByteArray.Length);
                    cs.FlushFinalBlock();
                    encrypt = Convert.ToBase64String(ms.ToArray());
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return encrypt;
        }

        public static string AesDecryptBase64(this string sourceStr, string cryptoKey)
        {
            string decrypt = "";
            try
            {
                AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
                byte[] key = sha256.ComputeHash(Encoding.UTF8.GetBytes(cryptoKey));
                byte[] iv = md5.ComputeHash(Encoding.UTF8.GetBytes(cryptoKey));
                aes.Key = key;
                aes.IV = iv;

                if (sourceStr == null) return null;

                byte[] dataByteArray = Convert.FromBase64String(sourceStr);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(dataByteArray, 0, dataByteArray.Length);
                        cs.FlushFinalBlock();
                        decrypt = Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return decrypt;
        }

        public static string Md5Hash(this string sourceStr)
        {
            using (MD5 md5 = MD5.Create()) { 
     
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(sourceStr));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
                return sBuilder.ToString();
            }
            
        }

        public static T Encrypt<T>(this T data)
        {
            data.GetType()
                .GetProperties()
                .ToList()
                .ForEach(pi =>
                {
                    var attrs = pi.GetCustomAttributes(typeof(SecurityAttribute), false);

                    if (attrs.Length > 0)
                    {
                        var value = pi.GetValue(data, null)?.ToString();

                        pi.SetValue(data, value.AesEncryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey));
                    }
                });

            return data;
        }

        public static T Decrypt<T>(this T data)
        {
            data.GetType()
                .GetProperties()
                .ToList()
                .ForEach(pi =>
                {
                    var attrs = pi.GetCustomAttributes(typeof(SecurityAttribute), false);

                    if (attrs.Length > 0)
                    {
                        var value = pi.GetValue(data, null)?.ToString();
                        var decryptValue = value.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey);
                        pi.SetValue(data, decryptValue);
                    }
                });

            return data;
        }

        private static RNGCryptoServiceProvider rngp = new RNGCryptoServiceProvider();
        private static byte[] rb = new byte[4];

        /// <summary>
        /// 產生一個非負數的亂數
        /// </summary>
        public static int Next()
        {
            rngp.GetBytes(rb);
            int value = BitConverter.ToInt32(rb, 0);
            if (value < 0) value = -value;
            return value;
        }
        /// <summary>
        /// 產生一個非負數且最大值 max 以下的亂數
        /// </summary>
        /// <param name="max">最大值</param>
        public static int Next(int max)
        {
            rngp.GetBytes(rb);
            int value = BitConverter.ToInt32(rb, 0);
            value = value % (max + 1);
            if (value < 0) value = -value;
            return value;
        }
        /// <summary>
        /// 產生一個非負數且最小值在 min 以上最大值在 max 以下的亂數
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public static int Next(int min, int max)
        {
            int value = Next(max - min) + min;
            return value;
        }
    }
}
