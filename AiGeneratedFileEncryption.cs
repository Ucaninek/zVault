using System;
using System.IO;
using System.Security.Cryptography;

public static class AiGeneratedFileEncryption
{
    public static void EncryptFile(string inputFile, string outputFile, string password)
    {
        byte[] salt = GenerateRandomBytes(16);
        byte[] key = DeriveKey(password, salt);

        using (FileStream inputStream = new FileStream(inputFile, FileMode.Open))
        using (FileStream outputStream = new FileStream(outputFile, FileMode.Create))
        using (AesManaged aes = new AesManaged())
        {
            aes.Key = key;
            aes.IV = GenerateRandomBytes(aes.BlockSize / 8);

            outputStream.Write(salt, 0, salt.Length);
            outputStream.Write(aes.IV, 0, aes.IV.Length);

            using (CryptoStream cryptoStream = new CryptoStream(outputStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    cryptoStream.Write(buffer, 0, bytesRead);
                }
            }
        }
    }

    public static void DecryptFile(string inputFile, string outputFile, string password)
    {
        byte[] salt = new byte[16];
        byte[] key = DeriveKey(password, salt);

        using (FileStream inputStream = new FileStream(inputFile, FileMode.Open))
        using (FileStream outputStream = new FileStream(outputFile, FileMode.Create))
        using (AesManaged aes = new AesManaged())
        {
            inputStream.Read(salt, 0, salt.Length);
            aes.IV = new byte[aes.BlockSize / 8];
            inputStream.Read(aes.IV, 0, aes.IV.Length);

            aes.Key = key;

            using (CryptoStream cryptoStream = new CryptoStream(outputStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
            {
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    cryptoStream.Write(buffer, 0, bytesRead);
                }
            }
        }
    }

    private static byte[] GenerateRandomBytes(int length)
    {
        byte[] randomBytes = new byte[length];
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(randomBytes);
        }
        return randomBytes;
    }

    private static byte[] DeriveKey(string password, byte[] salt)
    {
        const int Iterations = 10000;
        const int KeySize = 32;

        using (Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(password, salt, Iterations))
        {
            return deriveBytes.GetBytes(KeySize);
        }
    }
}