using MukuBase;
using MukuBase.Windows.UWPHelpers;
using System.Text;

namespace AesTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Aes()
        {
            byte[] password = Encoding.Unicode.GetBytes("password");
            byte[] start = Encoding.Unicode.GetBytes("lompos");
            byte[] encrypted = AES.Encrypt(start, password);
            byte[] end = AES.Decrypt(encrypted, password);
            string a = Encoding.Unicode.GetString(start);
            string b = Encoding.Unicode.GetString(end);
            Console.WriteLine(a);
            Console.WriteLine(b);
            if (a != b)
            {
                throw new Exception();
            }
            
        }
        [TestMethod]
        public void WinHello_MatchingIds() => WinHello_MatchingIds_asnyc().Wait();       
        public async Task WinHello_MatchingIds_asnyc()
        {
            byte[] start = Encoding.Unicode.GetBytes("lompos");
            byte[] encrypted = await WindowsHello.Encrypt("asd",start);
            byte[] end = await WindowsHello.Decrypt("asd",encrypted);
            string a = Encoding.Unicode.GetString(start);
            string b = Encoding.Unicode.GetString(end);
            Console.WriteLine(a);
            Console.WriteLine(b);
            if (a != b)
            {
                throw new Exception();
            }
        }
        [TestMethod]
        public void WinHello_UnMatchingIds() => WinHello_UnMatchingIds_asnyc().Wait();      
        public async Task WinHello_UnMatchingIds_asnyc()
        {
            bool success = true;
            try
            {
                byte[] start = Encoding.Unicode.GetBytes("lompos");
                byte[] encrypted = await WindowsHello.Encrypt("asd", start);
                byte[] end = await WindowsHello.Decrypt("asd2", encrypted);
                success = false;
            }
            catch (Exception ex)
            {

            }
            finally 
            {
                if (!success)
                {
                    throw new Exception();
                }
            }
        }
    }
}