using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.WebUtilities;

namespace Utilities
{
    public static class TTSCryptography
    {
        private static Lazy<byte[]> _key = new Lazy<byte[]>(LoadKey);
        private static Regex rxPiped = new Regex("^([^\\|]+)\\|([a-zA-Z0-9_-]+)$");

        private static bool _deterministicEncryption => true;

        private static byte[] LoadKey()
        {
            byte[] buffer = { }; //(byte[])new ImageConverter().ConvertTo((object)Resources.encryption_key, typeof(byte[]));
            using (SHA256 shA256 = (SHA256)new SHA256Managed())
                return shA256.ComputeHash(buffer);
        }

        private static string GetRandomBytesAsString(int howManyBytes)
        {
            if (howManyBytes < 0)
                throw new ArgumentOutOfRangeException(nameof(howManyBytes), "howManyBytes must be greater than or equal to 0.");
            if (howManyBytes == 0)
                return "";
            using (RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create())
            {
                byte[] numArray = new byte[howManyBytes];
                randomNumberGenerator.GetBytes(numArray);
                return WebEncoders.Base64UrlEncode(numArray);
            }
        }

        private static string EncryptInternal(string plaintext)
        {
            byte[] numArray = _key.Value;
            byte[] bytes = Encoding.UTF8.GetBytes(plaintext);
            using (AesManaged aesManaged = new AesManaged())
            {
                aesManaged.Mode = CipherMode.CBC;
                aesManaged.Key = numArray;
                if (_deterministicEncryption)
                    aesManaged.IV = new byte[aesManaged.BlockSize / 8];
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    if (!_deterministicEncryption)
                        memoryStream.Write(aesManaged.IV, 0, aesManaged.BlockSize / 8);
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, aesManaged.CreateEncryptor(aesManaged.Key, aesManaged.IV), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(bytes, 0, bytes.Length);
                        cryptoStream.Flush();
                        cryptoStream.FlushFinalBlock();
                        return WebEncoders.Base64UrlEncode(memoryStream.ToArray());
                    }
                }
            }
        }

        private static string DecryptInternal(string cyphertext)
        {
            byte[] numArray = _key.Value;
            byte[] buffer1 = WebEncoders.Base64UrlDecode(cyphertext);
            if (buffer1 == null)
                throw new CryptographicException("The input data was not in the correct format. This can happen if an attempt is made to decrypt plaintext data.");
            using (AesManaged aesManaged = new AesManaged())
            {
                aesManaged.Mode = CipherMode.CBC;
                aesManaged.Key = numArray;
                using (MemoryStream memoryStream1 = new MemoryStream(buffer1))
                {
                    if (_deterministicEncryption)
                    {
                        aesManaged.IV = new byte[aesManaged.BlockSize / 8];
                    }
                    else
                    {
                        byte[] buffer2 = new byte[aesManaged.BlockSize / 8];
                        memoryStream1.Read(buffer2, 0, aesManaged.BlockSize / 8);
                        aesManaged.IV = buffer2;
                    }
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream1, aesManaged.CreateDecryptor(aesManaged.Key, aesManaged.IV), CryptoStreamMode.Read))
                    {
                        byte[] buffer2 = new byte[4096];
                        using (MemoryStream memoryStream2 = new MemoryStream())
                        {
                            while (true)
                            {
                                int count = cryptoStream.Read(buffer2, 0, buffer2.Length);
                                if (count != 0)
                                    memoryStream2.Write(buffer2, 0, count);
                                else
                                    break;
                            }
                            return Encoding.UTF8.GetString(memoryStream2.ToArray());
                        }
                    }
                }
            }
        }

        public static string Encrypt(string plaintext)
        {
            if (string.IsNullOrEmpty(plaintext))
                return plaintext;
            return EncryptInternal(plaintext);
        }

        public static string Decrypt(string cyphertext)
        {
            if (string.IsNullOrEmpty(cyphertext))
                return cyphertext;
            Match match = rxPiped.Match(cyphertext);
            string str1;
            string cyphertext1;
            if (match.Success)
            {
                str1 = match.Groups[1].Value;
                cyphertext1 = match.Groups[2].Value;
            }
            else
            {
                str1 = (string)null;
                cyphertext1 = cyphertext;
            }
            string str2 = DecryptInternal(cyphertext1);
            if (!string.IsNullOrEmpty(str1) && str1 != str2)
                throw new CryptographicException("Plaintext does not match decrypted text.");
            return str2;
        }
    }
}
