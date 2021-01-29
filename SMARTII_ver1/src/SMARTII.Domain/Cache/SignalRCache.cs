using System.Collections.Generic;
using System.Linq;

namespace SMARTII.Domain.Cache
{
    public class SignalRCache<T1, T2>
    {
        protected readonly Dictionary<T1, HashSet<T2>> _collection =
                  new Dictionary<T1, HashSet<T2>>();

        public SignalRCache() : base()
        {
        }

        /// <summary>
        /// 取得連線
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<T2> GetConnections(T1 key)
        {
            HashSet<T2> connections;
            if (_collection.TryGetValue(key, out connections))
            {
                return connections;
            }

            return Enumerable.Empty<T2>();
        }

        /// <summary>
        /// 取得全部連線資訊
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T1> GetConnections()
        {
            List<T1> keyColl = _collection.Keys.ToList();
            List<T1> retColl = new List<T1>();

            foreach (T1 item in keyColl)
            {
                HashSet<T2> connections;
                if (_collection.TryGetValue(item, out connections))
                    retColl.Add(item);
            }

            return retColl;
        }

        /// <summary>
        /// 移除連線
        /// </summary>
        /// <param name="connectionId"></param>
        public void Remove(T2 connectionId)
        {
            lock (_collection)
            {
                List<T1> keyColl = _collection.Keys.ToList();
                List<T2> retColl = new List<T2>();

                foreach (var item in keyColl)
                {
                    HashSet<T2> connections;
                    if (_collection.TryGetValue(item, out connections))
                        lock (connections)
                        {
                            connections.Remove(connectionId);

                            if (connections.Count == 0)
                                _collection.Remove(item);
                        }
                }
            }
        }

        /// <summary>
        /// 移除連線
        /// </summary>
        /// <param name="key"></param>
        /// <param name="connectionId"></param>
        public void Remove(T1 key, T2 connectionId)
        {
           
            lock (_collection)
            {
                HashSet<T2> connections;
                if (_collection.TryGetValue(key, out connections))
                    lock (connections)
                    {
                        connections.Remove(connectionId);

                        if (connections.Count == 0)
                            _collection.Remove(key);
                    }

            }
        }

        /// <summary>
        /// 新增連線
        /// </summary>
        /// <param name="key"></param>
        /// <param name="connectionId"></param>
        public void Add(T1 key, T2 connectionId)
        {
            lock (_collection)
            {
                HashSet<T2> connections;
                if (!_collection.TryGetValue(key, out connections))
                {
                    connections = new HashSet<T2>();
                    _collection.Add(key, connections);
                }

                lock (connections)
                {
                    //connections.Clear();
                    connections.Add(connectionId);
                }
            }
        }
    }
}
