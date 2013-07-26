using System;
using System.IO;

namespace GenericTransactionLog
{
    public abstract class TransactionStore
    {
        public abstract Stream OpenRead();

        public abstract Stream OpenAppend();
    }
}