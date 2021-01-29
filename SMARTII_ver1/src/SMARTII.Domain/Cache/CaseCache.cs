using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;

namespace SMARTII.Domain.Cache
{
    public class CaseCache<T1, T2>
    {
        protected readonly Dictionary<T1, HashSet<T2>> _collection =
                  new Dictionary<T1, HashSet<T2>>();

        public CaseCache() : base()
        {
        }

        /// <summary>
        /// 取得連線
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<T2> GetLookupUsers(T1 key)
        {
            HashSet<T2> connections;
            if (_collection.TryGetValue(key, out connections))
            {
                return connections;
            }

            return Enumerable.Empty<T2>();
        }

        /// <summary>
        /// 使用者目前加入的案件
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<T1> UserJoinedRooms(Func<T2, bool> filter)
        {
            var caseRooms =  this._collection.Where(@case => @case.Value.Any(filter));

            return caseRooms.Select(x => x.Key).ToList();
        }

        /// <summary>
        /// 移除連線
        /// </summary>
        /// <param name="connectionId"></param>
        public void Remove(Func<T2, bool> filter)
        {
            lock (_collection)
            {
                List<T1> keyColl = _collection.Keys.ToList();
                List<T2> retColl = new List<T2>();

                foreach (var key in keyColl)
                {
                    HashSet<T2> connections;
                    if (_collection.TryGetValue(key, out connections))
                        lock (connections)
                        {

                            _collection[key] = new HashSet<T2>(connections.Where(filter));
                            

                            if (_collection[key].Count == 0)
                                _collection.Remove(key);
                        }
                }
            }
        }

        /// <summary>
        /// 移除連線
        /// </summary>
        /// <param name="key"></param>
        /// <param name="connectionId"></param>
        public void Remove(T1 key, T2 connectionId, Func<T2, bool> filter)
        {

            lock (_collection)
            {
                HashSet<T2> connections;
                if (_collection.TryGetValue(key, out connections))
                    lock (connections)
                    {
                        _collection[key] = new HashSet<T2>(connections.Where(filter));

                        if (_collection[key].Count == 0)
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
                    connections.Add(connectionId);
                }
            }
        }
    }
}
