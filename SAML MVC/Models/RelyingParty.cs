using System.Security.Cryptography.X509Certificates;

namespace SAML_MVC.Models
{
    public class RelyingParty
    {
        public string Metadata { get; set; }

        public string Issuer { get; set; }

        public Uri AcsDestination { get; set; }

        public Uri SingleLogoutDestination { get; set; }

        public IEnumerable<X509Certificate2> SignatureValidationCertificates { get; set; }

        public IEnumerable<X509Certificate2> EncryptionCertificates { get; set; }
    }
}
