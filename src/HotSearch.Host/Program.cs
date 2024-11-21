using Hangfire;
using Hangfire.Redis.StackExchange;
using HotSearch.Domain.DomainServices;
using HotSearch.Domain.Jobs;
using HotSearch.Host;
using HotSearch.Shared.Options;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddDebug().AddConsole();

var redisConn = builder.Configuration.GetConnectionString("redis")!;
builder.Services.AddHangfire(c => c
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseRedisStorage(redisConn))
                .AddHangfireServer();

builder.Services.AddSingleton<IConnectionMultiplexer>(c=> ConnectionMultiplexer.Connect(redisConn));

builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddHostedService<JobHostService>();
builder.Services.AddSingleton<CameraDomainService>();
builder.Services.AddSingleton<TianxingDomainService>();
builder.Services.AddHostedService<InitHostService>();
builder.Services.Configure<TianXingOption>(builder.Configuration.GetSection("TianXingOption"));
{
    var types = AppDomain.CurrentDomain.GetAssemblies()
        .Where(s => s.GetName().Name!.StartsWith("HotSearch"))
        .SelectMany(s => s.GetTypes())
        .Where(s => s.IsClass && !s.IsAbstract && s.IsAssignableTo(typeof(IHotSearchJob))).ToList();

    foreach (var type in types)
    {
        builder.Services.AddSingleton(type);
        builder.Services.AddSingleton(typeof(IHotSearchJob), type);
    }
}

var app = builder.Build();
app.UseHangfireDashboard();

app.MapGet("/hotsearch", ([FromServices] IEnumerable<IHotSearchJob> jobs) => Task.WaitAll(jobs.Select(s => s.GetHotSearch()).ToArray()));
app.MapGet("/camera", ([FromServices] CameraDomainService service) => service.InitCamera());
app.MapGet("/poetry", ([FromServices] TianxingDomainService service) => service.InitPoetry());
app.MapGet("/sentence", ([FromServices] TianxingDomainService service) => service.InitSentence());
app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(_ => true));
app.MapDefaultControllerRoute();
app.Run();
