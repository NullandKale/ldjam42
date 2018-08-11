namespace Dillyo.TheEphern.Pooling
{
    using System;

    /// <summary>
    /// A queue base pool.
    /// </summary>
    /// <typeparam name="T">The type of the objects in the pool.</typeparam>
    public class QueuePool<T> : IPool<T>
    {
        /// <summary>
        /// A method to produce the object.
        /// </summary>
        private readonly Func<T> produce;

        /// <summary>
        /// The capacity of the list.
        /// </summary>
        private readonly int capacity;

        /// <summary>
        /// An array of objects.
        /// </summary>
        private readonly T[] objects;

        /// <summary>
        /// The current index of the queue.
        /// </summary>
        private int index;

        /// <summary>
        /// A constructor for the pool.
        /// </summary>
        /// <param name="factoryMethod">The method to produce the object.</param>
        /// <param name="maxSize">The max size of the list pool.</param>
        public QueuePool(Func<T> factoryMethod, int maxSize)
        {
            produce = factoryMethod;
            capacity = maxSize;
            index = -1;
            objects = new T[maxSize];
        }

        /// <summary>
        /// Gets an item from the queue.
        /// </summary>
        /// <returns>Returns the object from the queue.</returns>
        public T GetInstance()
        {
            index = (index + 1) % capacity;

            if (objects[index] == null)
            {
                objects[index] = produce();
            }

            return objects[index];
        }
    }
}