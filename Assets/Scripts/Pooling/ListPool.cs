namespace Dillyo.TheEphern.Pooling
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A list based pool.
    /// </summary>
    /// <typeparam name="T">The type of the objects in the pool.</typeparam>
    public class ListPool<T> : IPool<T> where T : class
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
        /// The list of objects.
        /// </summary>
        private readonly List<T> objects;

        /// <summary>
        /// A test to see if an object is in use.
        /// </summary>
        private readonly Func<T, bool> useTest;

        /// <summary>
        /// Tests if the list is expandable.
        /// </summary>
        private readonly bool expandable;

        /// <summary>
        /// A constructor for the pool.
        /// </summary>
        /// <param name="factoryMethod">The method to produce the object.</param>
        /// <param name="maxSize">The max size of the list pool.</param>
        /// <param name="inUse">A way to tell if the object is in use.</param>
        /// <param name="expandable">If the pool is expnadable.</param>
        public ListPool(Func<T> factoryMethod, int maxSize, Func<T, bool> inUse, bool expandable = true)
        {
            produce = factoryMethod;
            capacity = maxSize;
            objects = new List<T>(maxSize);
            useTest = inUse;
            this.expandable = expandable;
        }

        /// <summary>
        /// Gets an item from the list.
        /// </summary>
        /// <returns>Returns the object from the list.</returns>
        public T GetInstance()
        {
            for (var i = 0; i < objects.Count; i++)
            {
                if (!useTest(objects[i]))
                {
                    return objects[i];
                }
            }

            if (objects.Count >= capacity && !expandable)
            {
                return null;
            }

            var obj = produce();
            objects.Add(obj);
            return obj;
        }
    }
}