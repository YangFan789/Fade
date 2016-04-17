namespace Fade.Common
{
    public interface ICacheSource<out TSource, out TResult>
    {
        TSource Source { get; }
        TResult Eof { get; }
        TResult GetNextResult();
    }
}