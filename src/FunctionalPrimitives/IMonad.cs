namespace FunctionalPrimitives;

public interface IMonad<out T>
{
    // ReSharper disable once InconsistentNaming
    IMonad<U> Bind<U>(Func<T, IMonad<U>> f);
}
