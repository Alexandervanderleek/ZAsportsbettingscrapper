using OddScrapperService.Scrappers.Core;
using OddScrapperService.Scrappers.Jobs;
using Quartz;

namespace OddScrapperService
{
    public class ScrapperWorker(ILogger<ScrapperWorker> logger, ISchedulerFactory schedulerFactory) : BackgroundService
    {
        private readonly ILogger<ScrapperWorker> _logger = logger;
        private readonly ISchedulerFactory _schedulerFactory = schedulerFactory;
        private IScheduler _scheduler;

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            await _scheduler.Start(cancellationToken);
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await SetupScrapingJobs(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        private async Task SetupScrapingJobs(CancellationToken cancellationToken)
        {
            IJobDetail sportingBetJob = JobBuilder.Create<SportingBetJob>()
                .WithIdentity("sportingBet", "scrapers")
                .UsingJobData("sportsBookName", "SportingBet")
                .Build();

            ITrigger sportingBetTrigger = TriggerBuilder.Create()
                .WithIdentity("sportingBetTrigger", "scrapers")
                .StartNow()
                .WithSimpleSchedule(x => x
                .WithIntervalInMinutes(15)
                .RepeatForever())
            .Build();

            await _scheduler.ScheduleJob(sportingBetJob, sportingBetTrigger, cancellationToken);
        }
    }
}
