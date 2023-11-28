using System.ComponentModel;

namespace Host.Common.Enums
{
    /// <summary>
    /// 任务类型
    /// </summary>
    [Description("任务类型")]
    public enum JobTypeEnum
    {
        None = 0,
        Url = 1,
        Emial = 2,
        Mqtt = 3,
        RabbitMQ = 4,
        Hotreload = 5,
    }
}
