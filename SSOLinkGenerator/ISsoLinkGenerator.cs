using System;
using System.Collections.Generic;
using System.Text;

namespace SSOLinkGenerator
{
    internal interface ISsoLinkGenerator
    {
        string GenerateLink(Dictionary<string, string> input);
    }
}
