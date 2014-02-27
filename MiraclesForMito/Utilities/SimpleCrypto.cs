using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace MiraclesForMito.Utilities
{
	public class SimpleCrypto
	{
		private static byte[] DESKey = { 10, 5, 90, 55, 9, 18, 19, 4 };
		private static byte[] DESInitializationVector = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

		public static string Encrypt(string value)
		{
			using (var cryptoProvider = new DESCryptoServiceProvider())
			using (var memoryStream = new MemoryStream())
			using (var cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(DESKey, DESInitializationVector), CryptoStreamMode.Write))
			using (var writer = new StreamWriter(cryptoStream))
			{
				writer.Write(value);
				writer.Flush();
				cryptoStream.FlushFinalBlock();
				writer.Flush();
				return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
			}
		}

		public static string Decrypt(string value)
		{
			using (var cryptoProvider = new DESCryptoServiceProvider())
			using (var memoryStream = new MemoryStream(Convert.FromBase64String(value)))
			using (var cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(DESKey, DESInitializationVector), CryptoStreamMode.Read))
			using (var reader = new StreamReader(cryptoStream))
			{
				return reader.ReadToEnd();
			}
		}
	}
}