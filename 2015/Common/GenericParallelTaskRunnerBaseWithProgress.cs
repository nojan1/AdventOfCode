using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public abstract class GenericParallelTaskRunnerBaseWithProgress<TParameter, TReturnValue, TProgress> 
        : GenericParallelTaskRunnerBase<TParameter, TReturnValue>
    {
        protected IProgress<TProgress> _progress;

        public GenericParallelTaskRunnerBaseWithProgress(int maxWorkers, IProgress<TProgress> progress) : base(maxWorkers)
        {
            _progress = progress;
        }

        protected void ReportProgress(TProgress progressObject)
        {
            if (_progress != null)
                _progress.Report(progressObject);
        }
    }
}
