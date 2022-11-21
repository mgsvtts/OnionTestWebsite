namespace Services.Abstractions;

public interface IFilter<T>
{
    public IQueryable<T> Execute(IQueryable<T> toFilter);
}