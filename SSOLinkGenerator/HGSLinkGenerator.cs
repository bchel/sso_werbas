using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSOLinkGenerator
{
    internal class HGSLinkGenerator : LinkGeneratorBase, ISsoLinkGenerator
    {
        public HGSLinkGenerator(string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
        }

        public override string GenerateLink(Dictionary<string, string> input)
        {
            _SSOUrl = GetFromConfiguration<string>("url");
            Parameters = GetFromConfiguration<List<ParameterDefinition>>("parameters");

            ValidateMandatoryParameters(input);

            var validParameters = GetValidParametersAndUrlEncode(input);
            var hash = GenerateHash(validParameters);

            return ConcatenateLink(validParameters, hash);
        }

        string GenerateHash(Dictionary<string,string> input)
        {
            var concatenatedString = ClientId + ClientSecret + input["username"] + string.Join("", input.Where(x=>x.Key != "username").OrderBy(x=>x.Key).Select(x=>x.Value));
            return Sha256(concatenatedString, ClientSecret);
        }

        static string Sha256(string randomString, string salt)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString+salt));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        string ConcatenateLink(Dictionary<string, string> parameters, string hash)
        {
            return _SSOUrl + "?clientid=" + ClientId + "&" + string.Join("&", parameters.Select(x => $"{x.Key}={x.Value}")) + "&checksum=" + hash;
        }
    }
}
