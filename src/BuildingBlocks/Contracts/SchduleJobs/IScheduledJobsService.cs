using System.Linq.Expressions;

namespace Contracts.SchduleJobs
{
    public interface IScheduledJobsService
    {
        #region Fire and Forget

        string Enqueue(Expression<Action> functionCall);
        string Enqueue<T>(Expression<Action<T>> functionCall);

        #endregion

        #region Delayed Jobs

        string Schedule(Expression<Action> functionCall, TimeSpan delay);
        string Schedule<T>(Expression<Action<T>> functionCall, TimeSpan delay);
        string Schedule(Expression<Action> functionCall, DateTimeOffset enqueueAt);
        //string Schedule<T>(Expression<Action<T>> functionCall, DateTimeOffset enqueueAt);

        #endregion

        #region Continuos Jobs

        string ContinueQueueWith(string parentJobId, Expression<Action> functionCall);
        bool Delete(string jobId);
        bool Requeue(string jobId);

        #endregion
    }
}
