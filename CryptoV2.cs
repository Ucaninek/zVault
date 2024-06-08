using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace zVault
 {
    internal class CryptoV2
    {
        private const int ChunkSize = 4096;

        public static byte[] EncryptFile(string filePath, string password, IProgress<int> progress = null)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] key = GetHashedKey(password);
                byte[] iv = new byte[16]; // Generate IV using a secure method
                using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(iv);
                }

                using (MemoryStream encryptedStream = new MemoryStream())
                {
                    using (Aes aes = Aes.Create())
                    {
                        aes.Key = key;
                        aes.IV = iv;
                        aes.Padding = PaddingMode.PKCS7;

                        encryptedStream.Write(iv, 0, iv.Length);

                        using (CryptoStream cryptoStream = new CryptoStream(encryptedStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            byte[] buffer = new byte[ChunkSize];
                            int bytesRead;
                            long totalBytesRead = 0;
                            long fileSize = fileStream.Length;

                            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                cryptoStream.Write(buffer, 0, bytesRead);
                                totalBytesRead += bytesRead;
                                int progressPercentage = (int)((totalBytesRead * 100) / fileSize);
                                progress?.Report(progressPercentage);
                            }
                            cryptoStream.FlushFinalBlock();
                        }
                    }

                    return encryptedStream.ToArray();
                }
            }
        }

        public static byte[] DecryptFile(string filePath, string password, IProgress<int> progress = null)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] key = GetHashedKey(password);
                byte[] iv = new byte[16]; // Retrieve IV from the encrypted file
                fileStream.Read(iv, 0, iv.Length);

                using (MemoryStream decryptedStream = new MemoryStream())
                {
                    using (Aes aes = Aes.Create())
                    {
                        aes.Key = key;
                        aes.IV = iv;
                        aes.Padding = PaddingMode.PKCS7;

                        using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            byte[] buffer = new byte[ChunkSize];
                            int bytesRead;
                            long totalBytesRead = 0;
                            long fileSize = fileStream.Length;
                            while ((bytesRead = cryptoStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                decryptedStream.Write(buffer, 0, bytesRead);
                                totalBytesRead += bytesRead;
                                int progressPercentage = (int)((totalBytesRead * 100) / fileSize);
                                progress?.Report(progressPercentage);
                            }
                        }
                    }

                    return decryptedStream.ToArray();
                }
            }
        }

        public static async Task<byte[]> EncryptFileAsync(string filePath, string password, CancellationToken cancellationToken, IProgress<int> progress = null)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] key = GetHashedKey(password);
                byte[] iv = new byte[16]; // Generate IV using a secure method
                using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(iv);
                }

                using (MemoryStream encryptedStream = new MemoryStream())
                {
                    using (Aes aes = Aes.Create())
                    {
                        aes.Key = key;
                        aes.IV = iv;
                        aes.Padding = PaddingMode.PKCS7;

                        await encryptedStream.WriteAsync(iv, 0, iv.Length);

                        using (CryptoStream cryptoStream = new CryptoStream(encryptedStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            byte[] buffer = new byte[ChunkSize];
                            int bytesRead;
                            long totalBytesRead = 0;
                            long fileSize = fileStream.Length;
                            while ((bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
                            {
                                await cryptoStream.WriteAsync(buffer, 0, bytesRead, cancellationToken);
                                totalBytesRead += bytesRead;
                                int progressPercentage = (int)((totalBytesRead * 100) / fileSize);
                                progress?.Report(progressPercentage);

                                // Check for cancellation
                                if (cancellationToken.IsCancellationRequested)
                                {
                                    // Clean up any resources and throw a cancellation exception
                                    // or return null to indicate cancellation
                                    throw new OperationCanceledException(cancellationToken);
                                }
                            }
                            cryptoStream.FlushFinalBlock();
                        }
                    }

                    return encryptedStream.ToArray();
                }
            }
        }

        public static async Task<byte[]> DecryptFileAsync(string filePath, string password, CancellationToken cancellationToken, IProgress<int> progress = null)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] key = GetHashedKey(password);
                byte[] iv = new byte[16]; // Retrieve IV from the encrypted file
                await fileStream.ReadAsync(iv, 0, iv.Length, cancellationToken);

                using (MemoryStream decryptedStream = new MemoryStream())
                {
                    using (Aes aes = Aes.Create())
                    {
                        aes.Key = key;
                        aes.IV = iv;
                        aes.Padding = PaddingMode.PKCS7;

                        using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            byte[] buffer = new byte[ChunkSize];
                            int bytesRead;
                            long totalBytesRead = 0;
                            long fileSize = fileStream.Length;
                            while ((bytesRead = await cryptoStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
                            {
                                await decryptedStream.WriteAsync(buffer, 0, bytesRead, cancellationToken);
                                totalBytesRead += bytesRead;
                                int progressPercentage = (int)((totalBytesRead * 100) / fileSize);
                                progress?.Report(progressPercentage);

                                // Check for cancellation
                                if (cancellationToken.IsCancellationRequested)
                                {
                                    // Clean up any resources and throw a cancellation exception
                                    // or return null to indicate cancellation
                                    throw new OperationCanceledException(cancellationToken);
                                }
                            }
                        }
                    }

                    return decryptedStream.ToArray();
                }
            }
        }

        private static byte[] GetHashedKey(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                return sha256.ComputeHash(passwordBytes);
            }
        }
    }
}
