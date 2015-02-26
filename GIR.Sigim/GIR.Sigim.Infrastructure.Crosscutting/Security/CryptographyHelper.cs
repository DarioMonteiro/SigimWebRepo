using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Infrastructure.Crosscutting.Security
{
    public static class CryptographyHelper
    {
        private const int SaltValueSize = 24;

        public static string CreateHashPassword(string password)
        {
            HashAlgorithm hash = new SHA256CryptoServiceProvider();
            UnicodeEncoding encoding = new UnicodeEncoding();

            if (!string.IsNullOrEmpty(password))
            {
                var saltValue = GenerateSaltValue(password + password.Length.ToString());

                byte[] binarySaltValue = new byte[SaltValueSize];

                binarySaltValue[0] = byte.Parse(saltValue.Substring(0, 2), System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat);
                binarySaltValue[1] = byte.Parse(saltValue.Substring(2, 2), System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat);
                binarySaltValue[2] = byte.Parse(saltValue.Substring(4, 2), System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat);
                binarySaltValue[3] = byte.Parse(saltValue.Substring(6, 2), System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat);

                byte[] valueToHash = new byte[SaltValueSize + encoding.GetByteCount(password)];
                byte[] binaryPassword = encoding.GetBytes(password);

                binarySaltValue.CopyTo(valueToHash, 0);
                binaryPassword.CopyTo(valueToHash, SaltValueSize);

                byte[] hashValue = hash.ComputeHash(valueToHash);

                StringBuilder hashedPassword = new StringBuilder();

                foreach (byte hexdigit in hashValue)
                    hashedPassword.Append(hexdigit.ToString("X2", CultureInfo.InvariantCulture.NumberFormat));

                return hashedPassword.ToString();
            }

            return null;
        }

        public static bool VerifyHashedPassword(string password, string profilePassword)
        {
            int saltLength = SaltValueSize * UnicodeEncoding.CharSize;

            if (string.IsNullOrEmpty(profilePassword) ||
                string.IsNullOrEmpty(password) ||
                profilePassword.Length < saltLength)
            {
                return false;
            }

            string saltValue = profilePassword.Substring(0, saltLength);

            string hashedPassword = CreateHashPassword(password);
            if (profilePassword.Equals(hashedPassword, StringComparison.Ordinal))
                return true;
            return false;
        }

        private static string GenerateSaltValue(string text)
        {
            byte[] saltValue = new byte[SaltValueSize];
            saltValue = GetBytes(text);
            string saltValueString = ByteArrayToString(saltValue);

            return saltValueString;
        }

        private static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private static string ByteArrayToString(byte[] byteArray)
        {
            StringBuilder hex = new StringBuilder(byteArray.Length * 2);

            foreach (byte b in byteArray)
                hex.AppendFormat("{0:x2}", b);

            return hex.ToString();
        }
    }
}