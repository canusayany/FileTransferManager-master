namespace IOExtensions
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="SafeQueue{T}" />.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    public class SafeQueue<T>
    {
        #region Fields

        /// <summary>
        /// Defines the queue.
        /// </summary>
        internal readonly Queue<T> queue = new Queue<T>();

        /// <summary>
        /// Defines the log.
        /// </summary>
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Count.
        /// </summary>
        public int Count
        {
            get
            {
                lock (queue)
                {
                    return queue.Count;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Clear.
        /// </summary>
        public void Clear()
        {
            lock (queue)
            {
                queue.Clear();
            }
        }

        /// <summary>
        /// 用于存入的队列.
        /// </summary>
        /// <param name="item">.</param>
        public void Enqueue(T item)
        {
            lock (queue)
            {
                queue.Enqueue(item);
            }
        }

        /// <summary>
        /// 用于读取的队列.
        /// </summary>
        /// <param name="result">.</param>
        /// <returns>.</returns>
        public bool TryDequeue(out T result)
        {
            lock (queue)
            {
                result = default(T);
                if (queue.Count > 0)
                {
                    result = queue.Dequeue();
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// The TryDequeueAll.
        /// </summary>
        /// <param name="result">The result<see cref="T[]"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool TryDequeueAll(out T[] result)
        {
            lock (queue)
            {
                result = queue.ToArray();
                queue.Clear();
                return result.Length > 0;
            }
        }

        #endregion
    }
}