namespace Services.Abstractions
{
    public interface ISorter<T>
    {
        public IQueryable<T> Execute(IQueryable<T> toSort);
    }
}