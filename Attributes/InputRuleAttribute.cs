using System;

namespace Network.UnityServer.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Delegate)]
    public class InputRuleAttribute : Attribute
    {
        public InputRuleAttribute(UNetworkServer server)
        {
            
        }
    }
}