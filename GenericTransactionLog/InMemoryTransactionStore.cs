using System.IO;
using System.Text;

namespace GenericTransactionLog
{
    public class InMemoryTransactionStore : TransactionStore
    {
        MemoryStream ms = new MemoryStream();
        public InMemoryTransactionStore()
        {
        }

        public override Stream OpenRead()
        {
            return new MemoryStream(ms.ToArray());
        }

        public override Stream OpenAppend()
        {
            var buffer = ms.GetBuffer();
            ms = new MemoryStream();
            new StreamWriter(ms).Write(buffer);
            return ms;
        }
    }
}