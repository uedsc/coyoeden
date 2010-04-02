/// Author:             Joe Audette
/// Created:            2006-02-06
/// Last Modified:      2009-06-21

using System;
using System.Configuration;
using System.Configuration.Provider;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using log4net;


namespace Cynthia.Web.Framework
{
    
    public static class CryptoHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CryptoHelper));

        public static string Encrypt(string clearText)
        {
           
            StringBuilder stringBuilder = new System.Text.StringBuilder();

            RSACryptoServiceProvider.UseMachineKeyStore = true;

            using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider())
            {
                CEncryptionConfiguration config = CEncryptionConfiguration.GetConfig();

                if (config.RsaKey.Length == 0)
                {
                    log.Error("CryptoHelper.LoadRsaKey failed to load key from config, key was an empty string.");
                    throw new ArgumentException("CryptoHelper.LoadRsaKey failed to load key from config, key was an empty string.");
                }

                rsaProvider.FromXmlString(config.RsaKey);
                  
                byte[] encryptedStr;
                encryptedStr = rsaProvider.Encrypt(Encoding.ASCII.GetBytes(clearText), false);
                
                for (int i = 0; i <= encryptedStr.Length - 1; i++)
                {
                    if (i != encryptedStr.Length - 1)
                    {
                        stringBuilder.Append(encryptedStr[i] + "~");
                    }
                    else
                    {
                        stringBuilder.Append(encryptedStr[i]);
                    }
                }
            }

            return stringBuilder.ToString();

        }

        public static string Decrypt(string encryptedText)
        {
  
            StringBuilder stringBuilder = new StringBuilder();

            RSACryptoServiceProvider.UseMachineKeyStore = true;

            using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider())
            {
                CEncryptionConfiguration config = CEncryptionConfiguration.GetConfig();
                if (config.RsaKey.Length == 0)
                {
                    log.Error("CryptoHelper.LoadRsaKey failed to load key from config, key was an empty string.");
                    throw new ArgumentException("CryptoHelper.LoadRsaKey failed to load key from config, key was an empty string.");
                }

                rsaProvider.FromXmlString(config.RsaKey);
    
                byte[] decryptedStr = rsaProvider.Decrypt(StringToByteArray(encryptedText.Trim()), false);
                
                for (int i = 0; i <= decryptedStr.Length - 1; i++)
                {
                    stringBuilder.Append(Convert.ToChar(decryptedStr[i]));
                }
            }

            return stringBuilder.ToString();

        }

        public static byte[] StringToByteArray(string inputText)
        {
            string[] s;
            s = inputText.Trim().Split('~');

            byte[] b = new byte[s.Length];

            for (int i = 0; i <= s.Length - 1; i++)
            {
                b[i] = Convert.ToByte(s[i],CultureInfo.InvariantCulture);
            }
            return b;
        }


        public static string Hash(string cleanText)
        {
            if (cleanText != null)
            {
                Byte[] clearBytes = new UnicodeEncoding().GetBytes(cleanText);

                Byte[] hashedBytes 
                    = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);

                return BitConverter.ToString(hashedBytes);
            }
            else
            {
                return String.Empty;
            }

        }

        //

        // TODO: move to config, should not be hard coded
        private static byte[] key_192 = new byte[] 
			{10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10,
				10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10};

        private static byte[] iv_128 = new byte[]
			{10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10,
				10, 10, 10, 10};

        public static string EncryptRijndaelManaged(string value)
        {
            if (value == string.Empty) return string.Empty;

            RijndaelManaged crypto = new RijndaelManaged();
            MemoryStream memoryStream = new MemoryStream();

            CryptoStream cryptoStream = new CryptoStream(
                memoryStream, 
                crypto.CreateEncryptor(key_192, iv_128),
                CryptoStreamMode.Write);

            StreamWriter streamWriter = new StreamWriter(cryptoStream);

            streamWriter.Write(value);
            streamWriter.Flush();
            cryptoStream.FlushFinalBlock();
            memoryStream.Flush();

            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }

        public static string DecryptRijndaelManaged(string value)
        {
            if (value == string.Empty) return string.Empty;

            RijndaelManaged crypto = new RijndaelManaged();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(value));

            CryptoStream cryptoStream = new CryptoStream(
                memoryStream, 
                crypto.CreateDecryptor(key_192, iv_128),
                CryptoStreamMode.Read);

            StreamReader streamReader = new StreamReader(cryptoStream);

            return streamReader.ReadToEnd();
        }

        public static string SignAndSecureData(string value)
        {
            return SignAndSecureData(new string[] { value });
        }

        

        public static string SignAndSecureData(string[] values)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<x></x>");

            for (int i = 0; i < values.Length; i++)
            {
                XmlHelper.AddNode(xmlDoc, "v" + i.ToString(), values[i]);
            }

            RSACryptoServiceProvider.UseMachineKeyStore = true;

            using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider())
            {
                CEncryptionConfiguration config = CEncryptionConfiguration.GetConfig();

                if (config.RsaKey.Length == 0)
                {
                    log.Error("CryptoHelper.LoadRsaKey failed to load key from config, key was an empty string.");
                    throw new ArgumentException("CryptoHelper.LoadRsaKey failed to load key from config, key was an empty string.");
                }

                rsaProvider.FromXmlString(config.RsaKey);

                byte[] signature = rsaProvider.SignData(Encoding.ASCII.GetBytes(xmlDoc.InnerXml),
                    "SHA1");

                XmlHelper.AddNode(xmlDoc, "s", Convert.ToBase64String(signature, 0, signature.Length));
            }

            
            return EncryptRijndaelManaged(xmlDoc.InnerXml);
        }

        
        public static bool DecryptAndVerifyData(string input, out string[] values)
        {
            string xml = DecryptRijndaelManaged(input);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            values = null;

            XmlNode node = xmlDoc.GetElementsByTagName("s")[0];
            node.ParentNode.RemoveChild(node);

            byte[] signature = Convert.FromBase64String(node.InnerText);

            byte[] data = Encoding.ASCII.GetBytes(xmlDoc.InnerXml);

            RSACryptoServiceProvider.UseMachineKeyStore = true;

            using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider())
            {
                CEncryptionConfiguration config = CEncryptionConfiguration.GetConfig();

                if (config.RsaKey.Length == 0)
                {
                    log.Error("CryptoHelper.LoadRsaKey failed to load key from config, key was an empty string.");
                    throw new ArgumentException("CryptoHelper.LoadRsaKey failed to load key from config, key was an empty string.");
                }

                rsaProvider.FromXmlString(config.RsaKey);
                if (!rsaProvider.VerifyData(data, "SHA1", signature))
                    return false;
            }

            int count;
            for (count = 0; count < 100; count++)
            {
                if (xmlDoc.GetElementsByTagName("v" + count.ToString())[0] == null)
                    break;
            }

            values = new string[count];

            for (int i = 0; i < count; i++)
                values[i] = xmlDoc.GetElementsByTagName("v" + i.ToString())[0].InnerText;

            return true;
        }

        



    }
}
