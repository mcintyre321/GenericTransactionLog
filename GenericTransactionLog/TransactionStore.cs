using System.IO;

namespace GenericTransactionLog
{
    public abstract class TransactionStore
    {
        public abstract Stream OpenRead();

        public abstract Stream OpenAppend();
    }

    public class InMemoryTransactionStore : TransactionStore
    {
        private readonly MemoryStream _ms;

        public InMemoryTransactionStore(MemoryStream ms = null)
        {
            _ms = ms ?? new MemoryStream();
        }

        public override Stream OpenRead()
        {
            return _ms;
        }

        public override Stream OpenAppend()
        {

            return _ms;
        }
    }
}