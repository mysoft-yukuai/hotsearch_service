using HotSearch.Domain.DomainServices;
using HotSearch.Domain.Jobs;

namespace HotSearch.Host
{
    public class InitHostService(CameraDomainService cameraService, TianxingDomainService tianxingDomainService, IEnumerable<IHotSearchJob> jobs) : IHostedService
    {
        private readonly CameraDomainService _cameraService = cameraService;
        private readonly TianxingDomainService _tianxingDomainService = tianxingDomainService;
        private readonly IEnumerable<IHotSearchJob> _jobs = jobs;
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _cameraService.InitCamera();
            await _tianxingDomainService.InitPoetry();
            await _tianxingDomainService.InitSentence();
            foreach (var job in _jobs)
            {
                await job.GetHotSearch();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
