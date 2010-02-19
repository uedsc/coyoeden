using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Linq.Expressions;
using SubSonic.Repository;

namespace CoyoEden.DataAccess.SubSonicX
{
    public class RecordBase<TId>
    {

        public TId Id { get; set; }
        private Hashtable ForeignCache = new Hashtable();
        protected T GetForeign<T, TFkId>(TFkId key) where T : RecordBase<TFkId>, new()
        {
            string relation = typeof(T).Name;
            T foreign = ForeignCache[relation] as T;
            if (foreign == null || !foreign.Id.Equals(key))
            {
                foreign = repo.Single<T>(key);
                ForeignCache[relation] = foreign;
            }
            return foreign;
        }

        protected TFkId SetForeign<T, TFkId>(T foreign) where T : RecordBase<TFkId>, new()
        {
            string relation = typeof(T).Name;
            ForeignCache[relation] = foreign;
            return (foreign == null) ? default(TFkId) : foreign.Id;
        }

        protected List<T> GetForeignList<T, TFkId>(Expression<Func<T, bool>> expression) where T : RecordBase<TFkId>, new()
        {
            return GetForeignList<T, TFkId>(expression, false);
        }

        protected List<T> GetForeignList<T, TFkId>(Expression<Func<T, bool>> expression, bool refresh) where T : RecordBase<TFkId>, new()
        {
            string relation = String.Format("l-{0}", typeof(T).Name);
            List<T> foreign = ForeignCache[relation] as List<T>;
            if (foreign == null || refresh)
            {
                foreign = repo.Find(expression).ToList();
                ForeignCache[relation] = foreign;
            }
            return foreign;
        }

        private IRepository repo;
        public RecordBase()
            : this(new SimpleRepository("CoyoEden", SimpleRepositoryOptions.None))
        {
        }
        public RecordBase(IRepository _repo)
        {//for the sake of tdd,inject repo via ioc.
            repo = _repo;
        }
        /// <summary>
        /// attach to a specified IRepository
        /// </summary>
        /// <param name="_repo"></param>
        /// <returns></returns>
        public RecordBase<TId> AttachRepo(IRepository _repo)
        {
            repo = _repo;
            return this;
        }
    }
}
