// 방장 제안의 유효성 추적.
namespace ProjectDawn.Combat
{
    public sealed class HostProposalHealth
    {
        public const long SilenceUs = 2000000;
        public int Missing { get; private set; }
        public long LastProgressUs { get; private set; }

        public void Progress(long evaluatedUs)
        {
            LastProgressUs = evaluatedUs;
            Missing = 0;
        }

        public void Omitted()
        {
            Missing++;
        }

        public bool Failed(long finalizedUs) => Missing >= 3 && finalizedUs - LastProgressUs >= SilenceUs;
    }
}
