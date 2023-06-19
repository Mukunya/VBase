using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.Security.Cryptography;
using MukuBase;

namespace MukuBase.Windows.UWPHelpers
{
    public static class WindowsHello
    {
        private const string challenge = "sign this challenge using Windows Hello to get a secure string";

        public static async Task<byte[]> GetKeyAsync(string id)
        {
            if (!await KeyCredentialManager.IsSupportedAsync())
            {
                throw new NotSupportedException();
            }

            var openKeyResult = await KeyCredentialManager.OpenAsync(id);

            if (openKeyResult.Status != KeyCredentialStatus.Success)
            {
                openKeyResult = await KeyCredentialManager.RequestCreateAsync(id, KeyCredentialCreationOption.ReplaceExisting);
            }
            if(openKeyResult.Status == KeyCredentialStatus.Success) {
                // convert our string challenge to be able to sign it 
                var buffer = CryptographicBuffer.ConvertStringToBinary(
                    challenge, BinaryStringEncoding.Utf8
                );

                // request a sign from the user
                var signResult = await openKeyResult.Credential.RequestSignAsync(buffer);

                // if successful, we can use that signature as a key
                if (signResult.Status == KeyCredentialStatus.Success)
                {
                    return signResult.Result.ToArray();
                }
            }

            throw new Exception("Failed to obtain key");
        }
        public static async Task<bool> IsSupportedAsync()
        {
            return await KeyCredentialManager.IsSupportedAsync();
        }

        public static async Task<byte[]> Encrypt(string keyid, byte[] data)
        {
            return AES.Encrypt(data, await GetKeyAsync(keyid));
        }
        public static async Task<byte[]> Decrypt(string keyid, byte[] data)
        {
            return AES.Decrypt(data, await GetKeyAsync(keyid));
        }

    }
}
