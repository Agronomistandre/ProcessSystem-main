using System;
using Prometheus.DotNetRuntime;

namespace Common.Helpers
{
    public static class PrometheusCollectorHelper
    {
        public static IDisposable CreateCollector()
        {
            var builder = DotNetRuntimeStatsBuilder.Customize()
                .WithContentionStats(CaptureLevel.Informational)
                .WithGcStats(CaptureLevel.Verbose)
                .WithThreadPoolStats(CaptureLevel.Informational)
                .WithExceptionStats(CaptureLevel.Errors)
                .WithJitStats()
                .RecycleCollectorsEvery(TimeSpan.FromDays(1))
                .WithDebuggingMetrics(true);

            return builder
                .StartCollecting();
        }

    }
}
