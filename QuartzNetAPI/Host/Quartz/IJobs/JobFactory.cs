using System;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using Robo.Notification.Provider;

namespace Host.IJobs
{
    /// <summary>
    /// Job工厂
    /// </summary>
    public class JobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public JobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;
                var job =  (IJob)scopedServiceProvider.GetRequiredService(bundle.JobDetail.JobType);
                return job;
            }  
        }
        /// <summary>
        /// 销毁IJob
        /// </summary>
        /// <param name="job"></param>
        public void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}
