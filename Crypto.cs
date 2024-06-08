using System;
using System.Security.Cryptography;
using System.Text;

public class Crypto
{
    public byte[] _key { get; set; }
    public byte[] _iv { get; set; }

    public Crypto(string key)
    {
        _key = Encoding.Default.GetBytes(MD5.CreateMD5(key));
        _iv = new byte[16];
    }

    private static string Reverse(string s)
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    public byte[] Encrypt(byte[] _byteData)
    {
        using (ICryptoTransform _iCrypto = new AesCryptoServiceProvider().CreateEncryptor(_key, _iv))
            {
        {
                var _encryptedData = _iCrypto.TransformFinalBlock(_byteData, 0, _byteData.Length);
                return _encryptedData; //Convert.ToBase64String(_encryptedData, 0, _encryptedData.Length);
            }
        }
    }
    public byte[] Decrypt(byte[] _byteData)
    {
        using (ICryptoTransform _iCrypto = new AesCryptoServiceProvider().CreateDecryptor(_key, _iv))
        {
            //var _byteData = Convert.FromBase64String(data);
            var _decryptedData = _iCrypto.TransformFinalBlock(_byteData, 0, _byteData.Length);
            return _decryptedData;
        }
    }

    public int EncryptBlock(byte[] _byteData, byte[] output,  int outputOffset)
    {
        using (ICryptoTransform _iCrypto = new AesCryptoServiceProvider().CreateEncryptor(_key, _iv))
        {
            {
                int _encryptedData = _iCrypto.TransformBlock(_byteData, 0, _byteData.Length, output, outputOffset);
                return _encryptedData; //returns number of bytes transferred
            }
        }
    }
    public int DecryptBlock(byte[] _byteData, byte[] output, int outputOffset)
    {
        using (ICryptoTransform _iCrypto = new AesCryptoServiceProvider().CreateDecryptor(_key, _iv))
        {
            var _decryptedData = _iCrypto.TransformBlock(_byteData, 0, _byteData.Length, output, outputOffset);
            return _decryptedData; //returns number of bytes transferred
        }
    }
}