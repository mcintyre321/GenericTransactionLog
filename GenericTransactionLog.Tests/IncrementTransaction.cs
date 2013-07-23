namespace GenericTransactionLog.Tests
{
    public class IncrementTransaction : GenericTransaction<Counter>
    {
        public override void Apply(Counter target)
        {
            target.Value++;
        }
    }
}