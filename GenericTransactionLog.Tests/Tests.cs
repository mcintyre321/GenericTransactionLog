using System;
using System.Collections.Generic;
using System.IO;
using GenericTransactionLog.Currying;
using NUnit.Framework;

namespace GenericTransactionLog.Tests
{
    public class Tests
    {
        [Test]
        public void CanWorkWithAnInMemoryLog()
        {
            var fileTransactionStore = new InMemoryTransactionStore();

            var transactionLog = GetTransactionLog(fileTransactionStore);

            var transaction = new IncrementTransaction();

            transactionLog.LogTransaction(transaction);
            var counter = transactionLog.Value;

            Assert.AreEqual(1, counter.Value);
            transactionLog.LogAndApplyTransaction(transaction);
             
            Assert.AreEqual(2, counter.Value);
        }

        [Test]
        public void CanLogAndApplyATransaction()
        {
            var fileName = Path.GetRandomFileName();
            var fileTransactionStore = new FileTransactionStore(fileName);

            var transactionLog = GetTransactionLog(fileTransactionStore);

            var transaction = new IncrementTransaction();

            transactionLog.LogAndApplyTransaction(transaction);
            var counter = transactionLog.Value;

            Assert.AreEqual(1, counter.Value);

            transactionLog.LogAndApplyTransaction(transaction);

            Assert.AreEqual(2, counter.Value);
        }

        [Test]
        public void CanLogAndReplayATransaction()
        {
            var fileName = Path.GetRandomFileName();
            var fileTransactionStore = new FileTransactionStore(fileName);
            
            var transactionLog = GetTransactionLog(fileTransactionStore);

            var transaction = new IncrementTransaction();

            transactionLog.LogTransaction(transaction);
            var counter = transactionLog.Value;

            Assert.AreEqual(1, counter.Value);
        }

        [Test]
        public void CanWorkWithExistingTransactionLog()
        {
            var fileName = Path.GetRandomFileName();
            File.WriteAllText(fileName, "increment\r\nincrement\r\n");
            var fileTransactionStore = new FileTransactionStore(fileName);

            var transactionLog = GetTransactionLog(fileTransactionStore);

            var counter = transactionLog.Value;

            Assert.AreEqual(2, counter.Value);
        }

        private static TransactionLog<Counter> GetTransactionLog(TransactionStore transactionStore)
        {

            Func<GenericTransaction<Counter>, string> serializeTransactionToString = t => "increment";
            var writeTransaction = serializeTransactionToString.Then(Funcs.AppendStringToStream);

            var readTransactions = Funcs.StreamToStreamReader
                                        .Then(Funcs.ReadStringsFromStreamReader)
                                        .ThenForEach(s => new IncrementTransaction());


            return new TransactionLog<Counter>(() => new Counter(), writeTransaction, readTransactions, transactionStore);
        }
    }
}