using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SSOLinkGenerator
{
    internal abstract class LinkGeneratorBase : ISsoLinkGenerator
    {
        protected string ClientId { get; set; }

        protected string ClientSecret { get; set; }

        protected List<ParameterDefinition> Parameters { get; set; }

        protected string _SSOUrl;

        public virtual string GenerateLink(Dictionary<string, string> input)
        {
            throw new NotImplementedException();
        }

        protected void ValidateMandatoryParameters(Dictionary<string, string> input)
        {
            var allMandatoryPresent = Parameters.Where(x => x.Mandatory).All(x => input.ContainsKey(x.Name));
            if (!allMandatoryPresent)
            {
                throw new ArgumentException($"Insufficent input parameters provied. Mandatory parameters are: {string.Join(",", Parameters.Where(x => x.Mandatory))}");
            }
        }

        protected Dictionary<string, string> GetValidParametersAndUrlEncode(Dictionary<string, string> input)
        {
            return input.Where(x => Parameters.Select(y => y.Name).Contains(x.Key)).ToDictionary(x => x.Key, y => System.Web.HttpUtility.UrlEncode(y.Value, Encoding.UTF8));
        }

        protected T GetFromConfiguration<T>(string key)
        {
            var configFileName = $"{this.GetType().Name}.json";
            if (!File.Exists(configFileName))
            {
                throw new Exception($"Configuration file {configFileName} not found");
            }
            try
            {
                JObject o1 = JObject.Parse(File.ReadAllText(configFileName));

                using (StreamReader file = File.OpenText(configFileName))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject o2 = (JObject)JToken.ReadFrom(reader);
                    return o2[key].ToObject<T>();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error reading url value from the configuration file", e);
            }
        }
    }
}
