namespace Dillyo.TheEphern.Core
{
    using Pooling;
    using UnityEngine;

    /// <summary>
    /// A MonoBehavior that uses the pools.
    /// </summary>
    public class MonoPool : MonoBehaviour
    {
        /// <summary>
        /// The Capacity of the pool.
        /// </summary>
        public int Capacity;

        /// <summary>
        /// The GameObject to use for the pool.
        /// </summary>
        public GameObject Prototype;

        /// <summary>
        /// Deterimines the type of pool.
        /// </summary>
        public PoolType Pooltype = PoolType.Queue;

        /// <summary>
        /// Tests if the pool is expandable.
        /// </summary>
        public bool Expandable = true;

        /// <summary>
        /// The types of pool that the MonoPool can use.
        /// </summary>
        public enum PoolType
        {
            Queue,
            List
        }

        /// <summary>
        /// The Pool that the MonoPool uses.
        /// </summary>
        public IPool<GameObject> Pool { get; private set; }

        /// <summary>
        /// Unity Awake.
        /// </summary>
        public void Awake()
        {
            switch (Pooltype)
            {
                case PoolType.Queue:
                    Pool = new QueuePool<GameObject>(() => Instantiate(Prototype),
                                                    Capacity);
                    break;

                case PoolType.List:
                    Pool = new ListPool<GameObject>(() => Instantiate(Prototype),
                                                    Capacity,
                                                    g => g.activeInHierarchy,
                                                    Expandable);
                    break;

                default:
                    return;
            }
        }
    }
}