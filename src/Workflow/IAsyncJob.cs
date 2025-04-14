using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable

namespace DiffClient.Workflow
{
    internal interface IAsyncJob
    {
        bool Run();

        bool Cancel();

        long GetJobId();

        bool Pause();

        bool Resume();
    }

    internal abstract class AsyncJobRoot : IAsyncJob
    {
        #region Internals Member

        protected long _id;
        protected volatile bool isPaused;
        protected volatile bool isStopped;
        protected CancellationTokenSource source = new CancellationTokenSource();
        protected CancellationToken token;

        protected AsyncJobRoot()
        {
            token = source.Token;
            _id = new Random().Next(0x10000, 0xffffff);
        }

        #endregion

        public abstract bool Cancel();
        public abstract long GetJobId();
        public abstract bool Pause();
        public abstract bool Resume();
        public abstract bool Run();
    }

    internal class AsyncJob : AsyncJobRoot
    {
        #region Public Members

        public override bool Cancel()
        {
            source.Cancel();
            isPaused = true;
            isStopped = true;
            return true;
        }

        public override long GetJobId()
        {
            return _id;
        }

        public override bool Pause()
        {
            isPaused = true;
            return true;
        }

        public override bool Resume()
        {
            isPaused = false;
            return true;
        }

        public override bool Run()
        {
            return true;
        }

        #endregion
    }
}
