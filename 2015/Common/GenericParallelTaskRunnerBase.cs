using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public abstract class GenericParallelTaskRunnerBase<TParameter, TReturnValue>
    {
        private int _maxWorkers;
        private List<Task> _taskList = new List<Task>();

        public bool RunComplete { get; protected set; }

        public GenericParallelTaskRunnerBase(int maxWorkers)
        {
            _maxWorkers = maxWorkers;
        }

        public void RunToEnd()
        {
            while (!RunComplete)
            {
                while (_taskList.Count < _maxWorkers)
                {
                    var parameter = CreateTaskParameter();
                    if (parameter == null)
                        break;

                    var task = Task.Run<TReturnValue>(() =>
                    {
                        return Worker(_taskList.Count, parameter);
                    }).ContinueWith(result =>
                    {
                        OnTaskFinished(parameter, result.Result);
                    });

                    _taskList.Add(task);
                }

                if (!_taskList.Any())
                {
                    RunComplete = true;
                    return;
                }

                int taskIndex = Task.WaitAny(_taskList.ToArray());
                var finishedTask = _taskList[taskIndex];

                _taskList.Remove(finishedTask);
            }
        }

        protected abstract TParameter CreateTaskParameter();
        protected abstract TReturnValue Worker(int taskId, TParameter parameter);
        protected abstract void OnTaskFinished(TParameter taskParameter, TReturnValue taskReturnValue);
    }
}
