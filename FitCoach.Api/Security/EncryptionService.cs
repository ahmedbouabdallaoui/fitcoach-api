using System.Security.Cryptography;
using System.Text;

namespace FitCoach.Api.Security;

// Encrypts payloads sent from .NET to the Python ML service.
// Uses AES-256-GCM which provides both encryption and tamper detection.
public class EncryptionService
{
    private readonly byte[] _key;

    public EncryptionService(IConfiguration configuration)
    {
        var base64Key = configuration["Encryption:Key"]
            ?? throw new InvalidOperationException("Encryption:Key is missing from configuration.");

        _key = Convert.FromBase64String(base64Key);
    }

    // Encrypts a plaintext string and returns a base64-encoded result.
    // Format: [nonce (12 bytes)] + [tag (16 bytes)] + [ciphertext]
    public string Encrypt(string plaintext)
    {
        var nonce = new byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(nonce);

        var plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
        var ciphertext = new byte[plaintextBytes.Length];
        var tag = new byte[AesGcm.TagByteSizes.MaxSize];

        using var aes = new AesGcm(_key, AesGcm.TagByteSizes.MaxSize);
        aes.Encrypt(nonce, plaintextBytes, ciphertext, tag);

        // Combine nonce + tag + ciphertext into one base64 string
        var result = new byte[nonce.Length + tag.Length + ciphertext.Length];
        nonce.CopyTo(result, 0);
        tag.CopyTo(result, nonce.Length);
        ciphertext.CopyTo(result, nonce.Length + tag.Length);

        return Convert.ToBase64String(result);
    }
}