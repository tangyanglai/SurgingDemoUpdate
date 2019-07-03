﻿using Autofac;
using Autofac.Extensions.DependencyInjection;
using Bill.Demo.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Surging.Core.Caching.Configurations;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.EventBusRabbitMQ.Configurations;
using System;

namespace Bill.Demo.Services.Server
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfigurationBuilder config)
        {
            ConfigureEventBus(config);
            ConfigureCache(config);
        }


        public IContainer ConfigureServices(ContainerBuilder builder)
        {
            var services = new ServiceCollection();
            ConfigureLogging(services);
            builder.Populate(services);
            ServiceLocator.Current = builder.Build();
            return ServiceLocator.Current;
        }

        public void Configure(IContainer app)
        {
            app.Resolve<ILoggerFactory>()
                    .AddConsole((c, l) => (int)l >= 3);
        }

        #region 私有方法
        /// <summary>
        /// 配置日志服务
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureLogging(IServiceCollection services)
        {
            services.AddLogging();
        }
        
        private static void ConfigureEventBus(IConfigurationBuilder build)
        {
            build
            .AddEventBusFile("Configs/eventBusSettings.json", optional: false);
        }

        /// <summary>
        /// 配置缓存服务
        /// </summary>
        private void ConfigureCache(IConfigurationBuilder build)
        {
            build
              .AddCacheFile("Configs/cacheSettings.json", optional: false);
        }

        #endregion
    }
}
