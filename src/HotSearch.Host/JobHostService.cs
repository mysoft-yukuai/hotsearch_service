using Hangfire;
using HotSearch.Domain;
using HotSearch.Domain.DomainServices;
using HotSearch.Domain.Jobs;

namespace HotSearch.Host
{
    public class JobHostService(IEnumerable<IHotSearchJob> jobs,TianxingDomainService tianxingDomainService) : IHostedService
    {
        private readonly IEnumerable<IHotSearchJob> _jobs = jobs;
        private readonly TianxingDomainService _tianxingDomainService = tianxingDomainService;
        public Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (var job in _jobs)
            {
                RecurringJob.AddOrUpdate(job.Type.ToString(), () => job.GetHotSearch(), Cron.Hourly);
            }
            RecurringJob.AddOrUpdate(RedisKeyNames.Poetry, () => _tianxingDomainService.InitPoetry(), Cron.Hourly);
            RecurringJob.AddOrUpdate(RedisKeyNames.Sentence, () => _tianxingDomainService.InitSentence(), Cron.Hourly);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
