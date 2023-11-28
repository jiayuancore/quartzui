using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Host.Common.Enums
{
    [Description("触发类型")]
    public enum TriggerTypeEnum
    {
        None = 0,
        Cron = 1,
        Simple = 2,
    }
}
