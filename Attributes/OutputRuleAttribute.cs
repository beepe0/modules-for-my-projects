using System;

namespace Network.UnityClient.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Delegate)]
    public class OutputRuleAttribute : Attribute
    {
        public OutputRuleAttribute(UNetworkClient client)
        {
            
        }
    }
}