using log4net;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace NoahExpressEmailSender
{
    internal class Program
    {
        private static readonly byte[] CryptoKey = Encoding.ASCII.GetBytes("@noahexpress.com");
        private static readonly byte[] CryptoIv = Convert.FromBase64String("wm193+oZDyA=");
        private static readonly Random Random = new Random();
        private static readonly ILog Log = LogManager.GetLogger("NoahExpressEmailSender");

        private static int Main(string[] args)
        {
            if (args.Length == 1)
            {
                var bytes = EncryptStringToBytes(args[0], CryptoKey, CryptoIv);
                Console.WriteLine(Convert.ToBase64String(bytes));

                return 0;
            }

            ServicePointManager.ServerCertificateValidationCallback = (s, certificate, chain, sslPolicyErrors) => true;

            var option = new Option()
            {
                Host = ConfigurationManager.AppSettings["Host"],
                Port = int.Parse(ConfigurationManager.AppSettings["Port"]),
                SenderAddress = ConfigurationManager.AppSettings["SenderAddress"],
                SenderPassword = ConfigurationManager.AppSettings["SenderPassword"],
                Subject = ConfigurationManager.AppSettings["Subject"],
                BodyFile = ConfigurationManager.AppSettings["BodyFile"],
                EmailAddressFile = ConfigurationManager.AppSettings["EmailAddressFile"],
                Attachments = ConfigurationManager.AppSettings["Attachments"]
            };
            option.SenderPassword = DecryptStringFromBytes(Convert.FromBase64String(option.SenderPassword), CryptoKey,
                CryptoIv);

            var recipients = (from recipient in File.ReadAllLines(option.EmailAddressFile)
                              where !string.IsNullOrWhiteSpace(recipient)
                              select recipient.Trim()).Distinct();
            foreach (var recipient in recipients)
            {
                Log.InfoFormat("Send mail to: {0}", recipient);
                try
                {
                    SendMail(option, recipient);
                }
                catch (Exception e)
                {
                    Log.Fatal(string.Format("Encounter fatal error when sending mail to {0}", recipient), e);
                }

                var sleepSeconds = 30 + Random.NextDouble() * 10;
                Log.InfoFormat("Sleep {0:N0} seconds", sleepSeconds);
                Thread.Sleep(TimeSpan.FromSeconds(sleepSeconds));
            }

            return 0;
        }

        private static void SendMail(Option option, string toAddress)
        {
            var body = File.ReadAllText(option.BodyFile);

            if (!File.Exists(option.EmailAddressFile))
                throw new FileNotFoundException("Email address file is not found.");

            var mailMessage = new MailMessage(option.SenderAddress, toAddress, option.Subject, body);
            foreach (var attachmentfileName in option.Attachments.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!File.Exists(attachmentfileName))
                {
                    throw new FileNotFoundException(string.Format("Attachment {0} is not found.", attachmentfileName));
                }
                mailMessage.Attachments.Add(new Attachment(attachmentfileName));
            }

            var smtpClient = new SmtpClient(option.Host, option.Port)
            {
                Credentials = new NetworkCredential(option.SenderAddress, option.SenderPassword),
                EnableSsl = true,
            };
            smtpClient.Send(mailMessage);
        }

        #region Encrypt/Decrypt

        private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");

            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");

            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("key");

            byte[] encrypted;
            // Create an TripleDESCryptoServiceProvider object
            // with the specified key and IV.
            using (var cryptoServiceProvider = new TripleDESCryptoServiceProvider())
            {
                cryptoServiceProvider.Key = key;
                cryptoServiceProvider.IV = iv;

                // Create a decrytor to perform the stream transform.
                var encryptor = cryptoServiceProvider.CreateEncryptor(cryptoServiceProvider.Key, cryptoServiceProvider.IV);

                // Create the streams used for encryption.
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (var streamWriter = new StreamWriter(cryptoStream))
                        {
                            //Write all data to the stream.
                            streamWriter.Write(plainText);
                        }
                        encrypted = memoryStream.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");

            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");

            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("key");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext;

            // Create an TripleDESCryptoServiceProvider object
            // with the specified key and IV.
            using (var cryptoServiceProvider = new TripleDESCryptoServiceProvider())
            {
                cryptoServiceProvider.Key = key;
                cryptoServiceProvider.IV = iv;

                // Create a decrytor to perform the stream transform.
                var decryptor = cryptoServiceProvider.CreateDecryptor(cryptoServiceProvider.Key, cryptoServiceProvider.IV);

                // Create the streams used for decryption.
                using (var memoryStream = new MemoryStream(cipherText))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var streamReader = new StreamReader(cryptoStream))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = streamReader.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        #endregion Encrypt/Decrypt
    }
}