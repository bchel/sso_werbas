using System;
using System.Collections.Generic;

namespace SSOLinkGenerator
{
    public class LinkGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId">Unique HGS value. It is used to uniquely identify the external system. The value is supplied by HGS, and is unique pr. System.</param>
        /// <param name="clientSecret">ClientSecret is supplied by HGS.</param>
        public LinkGenerator(string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
        }

        private string ClientId { get; }
        private string ClientSecret { get; }

        public string GenerateLink(LinkGeneratorEnum siteId, Dictionary<string, string> input)
        {
            ISsoLinkGenerator generator;
            switch (siteId)
            {
                case LinkGeneratorEnum.HGS:
                    generator = new HGSLinkGenerator(ClientId, ClientSecret);
                    break;
                default:
                    throw new ArgumentException($"Site:{siteId} not supported");
            }

            return generator.GenerateLink(input);
        }
    }
}
