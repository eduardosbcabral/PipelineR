using System;
using System.Linq.Expressions;
using Polly;

namespace PipelineR
{
    public abstract class RollbackHandler<TContext, TRequest> : IRollbackHandler<TContext, TRequest>
        where TContext : BaseContext
    {

        public int Index { get; private set; }
        protected RollbackHandler(TContext context)
        {
            this.Context = context;
        }

        public Expression<Func<TContext, TRequest, bool>> Condition { get; set; }
        internal Expression<Func<TContext, TRequest, bool>> RequestCondition { get; set; }
        public Policy Policy { get; set; }

        public TContext Context { get; private set; }

        public abstract void HandleRollback(TRequest request);

        public RequestHandlerResult Execute(TRequest request)
        {

            if (this.RequestCondition != null && this.RequestCondition.IsSatisfied(this.Context, request) == false)
                return null;

            if (this.Condition != null && this.Condition.IsSatisfied(this.Context, request) == false)
                return null;

            if (this.Policy != null)
            {
                this.Policy.Execute(() =>
                {
                     HandleRollback(request);
                });
            }
            else
            {
                 HandleRollback(request);
            }

            return null;
        }

        internal void AddRollbackIndex(int rollbackIndex) => this.Index = rollbackIndex;

        public string RequestHandleId() => this.GetType().Name;

        public void UpdateContext(TContext context)
        {
            context.ConvertTo(this.Context);
        }
    }

    public interface IRollbackHandler<TContext, TRequest>: IHandler<TContext, TRequest> where TContext : BaseContext
    {
        void HandleRollback(TRequest request);
    }

    public interface IHandler<TContext, TRequest> where TContext : BaseContext
    {
        Expression<Func<TContext, TRequest, bool>> Condition { get; set; }
        TContext Context { get; }
        void UpdateContext(TContext context);
        Policy Policy { get; set; }
        RequestHandlerResult Execute(TRequest request);
        string RequestHandleId();
    }
}
