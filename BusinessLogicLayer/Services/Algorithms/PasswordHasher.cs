using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLogicLayer.Services.Algorithms
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly byte[] _key;

        public PasswordHasher(IConfiguration config)
        {
            var key = config["Hashing:SecretKey"] ?? throw new ArgumentNullException("Secret key not found");
            Console.WriteLine(key);
            _key = Encoding.UTF8.GetBytes(key);
        }

        public string HashPassword(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password), "Password cannot be null!");
            }

            using (var hmac = new HMACSHA256(_key))
            {
                byte[] salt = GenerateSalt(16);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] saltedPassword = new byte[salt.Length + passwordBytes.Length];

                Buffer.BlockCopy(salt, 0, saltedPassword, 0, salt.Length);
                Buffer.BlockCopy(passwordBytes, 0, saltedPassword, salt.Length, passwordBytes.Length);

                byte[] hash = hmac.ComputeHash(saltedPassword);

                byte[] result = new byte[salt.Length + hash.Length];
                Buffer.BlockCopy(salt, 0, result, 0, salt.Length);
                Buffer.BlockCopy(hash, 0, result, salt.Length, hash.Length);

                return Convert.ToBase64String(result);
            }
        }

        public bool VerifyHashedPassword(string hashedPassword, string password)
        {
            if (hashedPassword == null)
            {
                throw new ArgumentNullException(nameof(hashedPassword), "Hashed password cannot be null!");
            }
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password), "Password cannot be null!");
            }

            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            byte[] salt = new byte[16];
            Buffer.BlockCopy(hashBytes, 0, salt, 0, salt.Length);

            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltedPassword = new byte[salt.Length + passwordBytes.Length];

            Buffer.BlockCopy(salt, 0, saltedPassword, 0, salt.Length);
            Buffer.BlockCopy(passwordBytes, 0, saltedPassword, salt.Length, passwordBytes.Length);

            using (var hmac = new HMACSHA256(_key))
            {
                byte[] computedHash = hmac.ComputeHash(saltedPassword);

                byte[] storedHash = new byte[computedHash.Length];
                Buffer.BlockCopy(hashBytes, salt.Length, storedHash, 0, storedHash.Length);

                return ByteArraysEqual(computedHash, storedHash);
            }
        }

        private static byte[] GenerateSalt(int length)
        {
            byte[] salt = new byte[length];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        private static bool ByteArraysEqual(byte[] b1, byte[] b2)
        {
            if (b1 == b2) return true;
            if (b1 == null || b2 == null) return false;
            if (b1.Length != b2.Length) return false;

            for (int i = 0; i < b1.Length; i++)
            {
                if (b1[i] != b2[i]) return false;
            }
            return true;
        }
    }
}
