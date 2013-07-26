using System;
using System.Collections.Generic;
using System.IO;

namespace GenericTransactionLog
{
    public class TransactionLog<TContext>  : IDisposable
        where TContext : class
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


        private readonly object _lock = new object();
        private TContext _value = null;

        public TContext Value
        {
            get
            {
                lock(_lock)
                {
                    return _value ?? (_value = Rebuild());
                }
            }
        }

        private TContext Rebuild()
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
            lock (_lock)
            {
                DoLog(transaction);
            }
        }

        private void DoLog(GenericTransaction<TContext> transaction)
        {
            using (var stream = _store.OpenAppend())
            {
                _writeTransaction(transaction, stream);
                stream.Flush();
            }
        }

        public void LogAndApplyTransaction(GenericTransaction<TContext> transaction)
        {
            lock (_lock)
            {
                var value = Value; //ensure rebuild
                DoLog(transaction);
                transaction.Apply(Value);
            }
        }

        public void Dispose()
        {
            
        }
    }
}