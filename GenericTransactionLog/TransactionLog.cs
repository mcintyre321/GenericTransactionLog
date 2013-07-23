using System;
using System.Collections.Generic;
using System.IO;

namespace GenericTransactionLog
{
    public class TransactionLog<TContext> 
    {
        private readonly Func<TContext> _initialiseContext;
        private readonly Action<GenericTransaction<TContext>, Stream> _writeTransaction;
        private readonly Func<Stream, IEnumerable<GenericTransaction<TContext>>> _readTransactions;
        private readonly TransactionStore _store;

        public TransactionLog(
            Func<TContext> initialiseContext,
            Action<GenericTransaction<TContext>, Stream> writeTransaction,
            Func<Stream, IEnumerable<GenericTransaction<TContext>>> readTransactions,
            TransactionStore store)
        {
            _initialiseContext = initialiseContext;
            _writeTransaction = writeTransaction;
            _readTransactions = readTransactions;
            _store = store;
        }

        public TContext Rebuild()
        {
            var model = _initialiseContext();
            using (var stream = _store.OpenRead())
            {
                foreach (var readTransaction in _readTransactions(stream))
                {
                    readTransaction.Apply(model);
                }
            }
            return model;
        }

        public void LogTransaction(GenericTransaction<TContext> transaction)
        {
            
            using (var fileStream = _store.OpenAppend())
            {
                _writeTransaction(transaction, fileStream);
                fileStream.Flush();
            }
        }
    }
}