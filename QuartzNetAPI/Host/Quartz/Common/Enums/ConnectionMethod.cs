using System.ComponentModel;

namespace Host.Common.Enums
{
    [Description("连接方法")]
    public enum ConnectionMethod
    {
        None = 0,
        TCP = 1,
        TCP_SSL = 2,
        WS = 3,
        WSS = 4,
    }
}
