using System.Security.Cryptography;
using AuthService.Application.Interfaces;

namespace AuthService.Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 32;
    private const int HashSize = 64;
    private const int Iterations = 10000;
    
    public string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA256, 
            HashSize);
        
        byte[] hashedPassword = new byte[HashSize + SaltSize];
        Array.Copy(salt, 0, hashedPassword, 0, SaltSize);
        Array.Copy(hash, 0, hashedPassword, SaltSize, HashSize);
        
        return Convert.ToBase64String(hashedPassword);
    }

    public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        if (string.IsNullOrWhiteSpace(hashedPassword) || string.IsNullOrWhiteSpace(providedPassword))
            return false;
        
        byte[] hashedPasswordBytes = Convert.FromBase64String(hashedPassword);
        
        byte[] salt = new byte[SaltSize];
        Array.Copy(hashedPasswordBytes, 0, salt, 0, SaltSize);
        
        byte[] providedPasswordHash = Rfc2898DeriveBytes.Pbkdf2(
            providedPassword, 
            salt, 
            Iterations, 
            HashAlgorithmName.SHA256,
            HashSize);
        
        // compare hash passwords
        for (int i = 0; i < HashSize; i++)
            if (hashedPasswordBytes[i + SaltSize] != providedPasswordHash[i])
                return false;
        
        return true;
    }
    
}