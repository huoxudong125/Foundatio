﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Foundatio.Caching;
using Foundatio.Extensions;
using Foundatio.Jobs;
using Foundatio.Lock;

namespace Foundatio.Tests.Jobs {
    public class ThrottledJob : JobBase {
        public ThrottledJob(ICacheClient client) {
            _locker = new ThrottlingLockProvider(client, 1, TimeSpan.FromMilliseconds(100));
        }

        private readonly ILockProvider _locker;
        public int RunCount { get; set; }

        protected override IDisposable GetJobLock() {
            return _locker.TryAcquireLock("WithLockingJob", acquireTimeout: TimeSpan.Zero);
        }

        protected override Task<JobResult> RunInternalAsync(CancellationToken token) {
            RunCount++;

            return Task.FromResult(JobResult.Success);
        }
    }
}