using System.Security.Cryptography;
using System.Text;
using Api.Biz;

namespace Api.Services;

public class EncryptionService
{
    private IConfiguration _configuration;
    
    public EncryptionService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    // Encrypt username and password using symetric algorithm.
    // First concatenate username and password as "username":"password"
    // Then encrypt the concatenated string using the symetric algorithm
    public string EncryptCredentials(string username, string password)
    {
        var key = LoadKey();
        string credentials = string.Format("\"{0}\":\"{1}\"", username, password);
        byte[] encryptedCredentials = Encrypt(credentials, key);
        return Convert.ToBase64String(encryptedCredentials);
    }
    
    // Decrypt username and password using symetric algorithm.
    // First decrypt the encrypted string using the symetric algorithm
    // Then split the decrypted string into username and password
    public Credential DecryptCredentials(string encryptedCredentials)
    {
        // read key from environment variable
        var key = LoadKey();
        byte[] credentials = Convert.FromBase64String(encryptedCredentials);
        string decryptedCredentials = Decrypt(credentials, key);
        string[] parts = decryptedCredentials.Split(':');
        string username = parts[0].Trim('"');
        string password = parts[1].Trim('"');
        return new Credential(username, password);
    }

    private string LoadKey()
    {
        string? key = _configuration["EncryptionKey"];
        if (key == null)
        {
            throw new Exception("Encryption key could not found");
        }

        return key;
    }

    private static byte[] Encrypt(string plainText, string? key)
    {
        var keyBytes = Encoding.UTF8.GetBytes(key);
        using (var aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.GenerateIV();
            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            {
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        var IV = aes.IV;
                        var result = new byte[IV.Length + msEncrypt.ToArray().Length];
                        Buffer.BlockCopy(IV, 0, result, 0, IV.Length);
                        Buffer.BlockCopy(msEncrypt.ToArray(), 0, result, IV.Length, msEncrypt.ToArray().Length);
                        return result;
                    }
                }
            }
        }
    }

    private static string Decrypt(byte[] cipherText, string? key)
    {
        var keyBytes = Encoding.UTF8.GetBytes(key);
        using (var aes = Aes.Create())
        {
            aes.Key = keyBytes;
            byte[] IV = new byte[aes.BlockSize / 8];
            byte[] encrypted = new byte[cipherText.Length - IV.Length];
            Buffer.BlockCopy(cipherText, 0, IV, 0, IV.Length);
            Buffer.BlockCopy(cipherText, IV.Length, encrypted, 0, encrypted.Length);
            aes.IV = IV;
            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            {
                using (var msDecrypt = new MemoryStream(encrypted))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}