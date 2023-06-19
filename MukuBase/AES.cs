using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MukuBase
{
    public static class AES
    {
        public static byte[] Decrypt(byte[] data, byte[] password)
        {
            using (AesCryptoServiceProvider AES = new AesCryptoServiceProvider())
            {
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] aesKey = SHA256.Create().ComputeHash(password);
                byte[] aesIV = MD5.Create().ComputeHash(password);
                AES.Key = aesKey;
                AES.IV = aesIV;
                AES.Mode = CipherMode.CBC;
                AES.Padding = PaddingMode.PKCS7;
                byte[] output;
                using (MemoryStream ds = new MemoryStream(data, false))
                {
                    using(MemoryStream ds2 = new MemoryStream())
                    {
                        CryptoStream cs = new CryptoStream(ds, AES.CreateDecryptor(), CryptoStreamMode.Read);
                        cs.CopyTo(ds2);
                        ds2.Position = 0;
                        output = ds2.ToArray();
                    }
                }
                return output;
            }
        }
        public static byte[] Encrypt(byte[] data, byte[] password)
        {
            using (AesCryptoServiceProvider AES = new AesCryptoServiceProvider())
            {
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] aesKey = SHA256.Create().ComputeHash(password);
                byte[] aesIV = MD5.Create().ComputeHash(password);
                AES.Key = aesKey;
                AES.IV = aesIV;
                AES.Mode = CipherMode.CBC;
                AES.Padding = PaddingMode.PKCS7;
                byte[] output;
                using (MemoryStream ds = new MemoryStream(data, false))
                {
                    using (MemoryStream ds2 = new MemoryStream())
                    {
                        CryptoStream cs = new CryptoStream(ds, AES.CreateEncryptor(), CryptoStreamMode.Read);
                        cs.CopyTo(ds2);
                        ds2.Position = 0;
                        output = ds2.ToArray();
                    }
                }
                return output;
            }
        }
    }
}

