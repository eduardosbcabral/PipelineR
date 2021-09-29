namespace PipelineR.Sample.Pipeline.Handlers
{
    public class InitializeCreateUserRecoveryStep : RecoveryHandler<UserContext, UserRequest>, IInitializeCreateUserRecoveryStep
    {
        public InitializeCreateUserRecoveryStep(UserContext context) : base(context)
        {
        }

        public override void HandleRecovery(UserRequest request)
        {
            this.Context.RecoveryWasExecuted = true;
        }
    }

    public interface IInitializeCreateUserRecoveryStep : IRecoveryHandler<UserContext, UserRequest>
    {

    }
}
