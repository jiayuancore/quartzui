using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Host.Quartz.Services
{
    public interface IJobService
    {
        public Task<BaseResult> AddJob([FromBody] ScheduleEntity entity);
    }
}
