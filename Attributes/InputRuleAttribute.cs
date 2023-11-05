using System;

namespace Network.UnityClient.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Delegate)]
    public class InputRuleAttribute : Attribute
    {
        public InputRuleAttribute(UNetworkClient client)
        {
            
        }
    }
}