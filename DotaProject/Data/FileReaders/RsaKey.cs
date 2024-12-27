using System.Security.Cryptography;

namespace DotaProject.Data.FileReaders
{
    public static class RsaKey
    {
        public static RSA GetPrivateKey(string privateKeyPath)
        {
            var privateKey = File.ReadAllText(privateKeyPath);
            var rsa = RSA.Create();
            rsa.ImportFromPem(privateKey.ToCharArray());
            return rsa;
        }

        public static RSA GetPublicKey(string publicKeyPath)
        {
            var publicKey = File.ReadAllText(publicKeyPath);
            var rsa = RSA.Create();
            rsa.ImportFromPem(publicKey.ToCharArray());
            return rsa;
        }
    }
}
