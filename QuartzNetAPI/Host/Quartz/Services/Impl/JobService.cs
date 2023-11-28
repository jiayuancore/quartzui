using Talk.Extensions;
using System.Threading.Tasks;
using Host.Common;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using Host.Common.Enums;

namespace Host.Quartz.Services.Impl
{
    public class JobService : IJobService
    {
        private readonly SchedulerCenter scheduler;
        public JobService(SchedulerCenter schedulerCenter)
        {
            scheduler = schedulerCenter;
        }
        public async Task<BaseResult> AddJob([FromBody] ScheduleEntity entity)
        {
            if (ConfigurationManager.GetTryConfig("EnvironmentalRestrictions", "false") == "true")
            {
                if (entity.TriggerType == TriggerTypeEnum.Simple &&
                    entity.IntervalSecond.HasValue &&
                    entity.IntervalSecond <= 10)
                {
                    return new BaseResult()
                    {
                        Code = 403,
                        Msg = "当前环境不允许低于10秒内循环执行任务！"
                    };
                }
                else if (entity.TriggerType == TriggerTypeEnum.Cron &&
                         entity.Cron == "* * * * * ?")
                {
                    return new BaseResult()
                    {
                        Code = 403,
                        Msg = "当前环境不允许过频繁执行任务！"
                    };
                }

            }
            return await scheduler.AddScheduleJobAsync(entity);
        }
    }
}
