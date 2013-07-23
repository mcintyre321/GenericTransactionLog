using System.IO;

namespace GenericTransactionLog
{
    public class FileTransactionStore : TransactionStore
    {
        private readonly string fileName;

        public FileTransactionStore(string fileName)
        {
            this.fileName = fileName;
            if (!File.Exists(fileName))
                File.WriteAllBytes(fileName, new byte[]{});
        }
        public override Stream OpenRead()
        {
            return File.OpenRead(fileName);
        }

        public override Stream OpenAppend()
        {
            return File.Open(fileName, FileMode.Append);
        }
    }
}