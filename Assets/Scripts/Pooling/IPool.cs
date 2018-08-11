namespace Dillyo.TheEphern.Pooling
{
    /// <summary>
    /// An interface for object pools.
    /// </summary>
    /// <typeparam name="T">The type of Pool.</typeparam>
    public interface IPool<out T>
    {
        /// <summary>
        /// Gets an instance of an object in the pool.
        /// </summary>
        /// <returns>The object from the pool.</returns>
        T GetInstance();
    }
}