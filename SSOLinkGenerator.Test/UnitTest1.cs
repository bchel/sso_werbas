using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SSOLinkGenerator.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var generator = new LinkGenerator("ssoDemo", "66a008f6-0918-48de-af52-5fb33fc342ce");

            var link = generator.GenerateLink(LinkGeneratorEnum.HGS, new Dictionary<string, string>
            {
                {"username","ssoDemoUser"},
                {"email","sh@hella-gutmann.dk" },
                {"modeltype","KSP130" },
                {"name","Simon" },
                {"regno","FC28792" },
                {"vin","VNKKL3D300A016125" },
                {"unknown","willbeskipped" }
            });

            Assert.IsTrue(link.StartsWith("https://hgs-idp-webclient.azurewebsites.net/account/sso/?clientid=ssoDemo&"));
            var parameters = link.Split(new char[] { '?', '&' }).ToList();

            Assert.IsTrue(parameters.Contains("checksum=66adbcb9d475863541054ffb6addb6bd7aefd8d312eb8c52b5f817c0df3406bd"));
            Assert.IsTrue(parameters.Contains("email=sh%40hella-gutmann.dk"));
            Assert.IsTrue(parameters.Contains("name=Simon"));
            Assert.IsTrue(parameters.Contains("vin=VNKKL3D300A016125"));
            Assert.IsTrue(parameters.Contains("regno=FC28792"));
            Assert.IsTrue(parameters.Contains("modeltype=KSP130"));
            Assert.IsTrue(parameters.Contains("username=ssoDemoUser"));
        }

        [TestMethod]
        public void UsernameNotProvided()
        {
            var generator = new LinkGenerator("ssoDemo", "66a008f6-0918-48de-af52-5fb33fc342ce");

            Assert.ThrowsException<ArgumentException>(() => generator.GenerateLink(LinkGeneratorEnum.HGS, new Dictionary<string, string>
            {

                {"email","sh@hella-gutmann.dk" },
                {"modeltype","KSP130" },
                {"name","Simon" },
                {"regno","FC28792" },
                {"vin","VNKKL3D300A016125" }
            }));
        }
    }
}
