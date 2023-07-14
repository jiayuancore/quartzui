using DotNetCore.CAP;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Robo.Utils.Events.Cap;
using System;
using System.IO;
using System.Linq;

namespace Host.Setup
{
    public static class EventBusSetup
    {
        /// <summary>
        /// 配置事件总线
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddEventBusSetup(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEventBus(cap =>
            {
                // 获取RabbitMq服务器配置
                var mq = configuration.GetSection("RabbitMq").Get<RabbitMQOptions>() ?? throw new ArgumentNullException(nameof(RabbitMQOptions));
                cap.UseRabbitMQ(cfg =>
                {
                    cfg.HostName = mq.HostName;
                    cfg.Port = mq.Port;
                    cfg.UserName = mq.UserName;
                    cfg.Password = mq.Password;
                    cfg.VirtualHost = mq.VirtualHost;
                });
                // 获取数据库连接字符串
                var sqlConnectionString = configuration.GetSection("Quartz:connectionString").Value ?? throw new ArgumentNullException("PostgreSQLConnectionString");
                cap.UsePostgreSql(opt =>
                {
                    opt.ConnectionString = sqlConnectionString;
                    opt.Schema = "public";
                });

                cap.UseDashboard();
                cap.DefaultGroup = "Robo.EmailRequestQueue"; // 指定队列名称，可自定义
                                                             // Robo.EmailRequestFailureQueue
                                                             // Robo.EmailRequestUnavailableQueue
                cap.FailedRetryInterval = 10; // 失败重试的时间间隔
                cap.FailedRetryCount = 10; // 失败重试的次数

            });
        }
    }
}
