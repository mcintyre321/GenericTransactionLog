namespace GenericTransactionLog
{
    public abstract class GenericTransaction<TModel>
    {
        public abstract void Apply(TModel target);
    }
}