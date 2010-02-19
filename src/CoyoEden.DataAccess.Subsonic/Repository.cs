using System;
using System.Linq;
using SystemX.Services;
using SubSonic.Repository;

namespace CoyoEden.DataAccess.SubSonicX
{
    public sealed class Repository<T,TId>:IRepository<T,TId> where T:class,new()
    {

        #region mamber variables
        private string _ConnectionStrKey = "CoyoEden";
        /// <summary>
        /// connection string key
        /// </summary>
        public string ConnectionStrKey {
            get
            {
                return _ConnectionStrKey;
            }
            set
            {
            	_ConnectionStrKey = value;
            }
        }
        private SimpleRepository repo;
        #endregion

        #region .ctor
        /// <summary>
        /// default .ctor
        /// </summary>
        public Repository():this("CoyoEden") { 
        }
        /// <summary>
        /// constructor with special connection string key
        /// </summary>
        /// <param name="conStrKey"></param>
        public Repository(string conStrKey) {
            _ConnectionStrKey = conStrKey;
            repo = new SimpleRepository(_ConnectionStrKey);
        }
        #endregion

        #region IRepository<T,TId> Members

        public void Delete(TId objKey)
        {
            repo.Delete<T>(objKey);
        }


        public T Find(TId Id)
        {
            return repo.Single<T>(Id);
        }

        public IQueryable<T> GetAll()
        {
            return repo.All<T>();
        }

        public IContext NewTrans()
        {
            return new TransactionContext();
        }

        public void Save(T obj)
        {
            repo.Update(obj);
        }

        #endregion

    }
}
