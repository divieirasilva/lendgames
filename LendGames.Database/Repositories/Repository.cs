using LendGames.Database.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

/*
* **************************************************************
* 
* Esta classe abstrata será a base de todos os repositórios
* de dados do sistema, sendo assim possível centralizar 
* eventos como "BeforeInsert", "BeforeUpdate", "BeforeDelete".
* Além de centralizar toda a lógica de acesso ao banco de dados
* em um único lugar os eventos serão úteis em momentos de 
* validação de dados.
* 
* **************************************************************
*/

namespace LendGames.Database.Repositories
{
    public abstract class Repository<TContext, TModel>
        where TModel : Model
        where TContext : LendGamesContext
    {
        #region Constructors

        public Repository(TContext context)
        {
            this.context = context;
            dbSet = this.context.Set<TModel>();
        }

        #endregion

        #region Events

        protected delegate void BeforeInsertEventHandler(RepositoryEventArgs<TModel> e);
        protected event BeforeInsertEventHandler BeforeInsert;

        protected delegate void BeforeUpdateEventHandler(RepositoryEventArgs<TModel> e);
        protected event BeforeUpdateEventHandler BeforeUpdate;

        protected delegate void BeforeDeleteEventHandler(RepositoryEventArgs<TModel> e);
        protected event BeforeDeleteEventHandler BeforeDelete;

        #endregion

        #region Fields

        protected TContext context;
        protected DbSet<TModel> dbSet;

        #endregion

        #region Methods

        public virtual int SqlCommand(string query, params object[] parameters)
        {
            return context
                .Database
                .ExecuteSqlCommand(query, parameters);
        }

        public virtual IQueryable<TModel> SqlQuery(string query, params object[] parameters)
        {
            return dbSet
                .SqlQuery(query, parameters)
                .AsQueryable();
        }

        public virtual IQueryable<TModel> Where()
        {
            return Where(string.Empty);
        }

        public virtual IQueryable<TModel> Where(string includes)
        {
            return Where(null, includes);
        }

        public virtual IQueryable<TModel> Where(Expression<Func<TModel, bool>> query)
        {
            return Where(query, string.Empty);
        }

        public virtual IQueryable<TModel> Where(Expression<Func<TModel, bool>> query, string includes)
        {
            IQueryable<TModel> dbQuery = dbSet;
            if (query != null)
                dbQuery = dbQuery.Where(query);

            var includeList = includes.Split(
                new char[] { ',' },
                StringSplitOptions.RemoveEmptyEntries
            );

            foreach (var include in includeList)
                dbQuery.Include(include);

            return dbQuery;
        }

        public virtual TModel Find(object id)
        {
            return dbSet.Find(id);
        }

        public virtual Task<TModel> FindAsync(object id)
        {
            return dbSet.FindAsync(id);
        }

        public virtual void Insert(TModel model)
        {
            var eArgs = new RepositoryEventArgs<TModel>();
            eArgs.Model = model;

            BeforeInsert?.Invoke(eArgs);

            dbSet.Add(model);
        }

        public virtual void InsertRange(params TModel[] models)
        {
            var eArgs = new RepositoryEventArgs<TModel>();
            eArgs.Models = models;

            BeforeInsert?.Invoke(eArgs);

            dbSet.AddRange(models);
        }

        public virtual void Update(TModel model)
        {
            var eArgs = new RepositoryEventArgs<TModel>();
            eArgs.Model = model;

            BeforeUpdate?.Invoke(eArgs);

            dbSet.Attach(model);
            context.Entry(model).State = EntityState.Modified;
        }

        public virtual void Delete(object id)
        {
            var model = Find(id);
            Delete(model);
        }

        public virtual void Delete(TModel model)
        {
            var eArgs = new RepositoryEventArgs<TModel>();
            eArgs.Model = model;

            BeforeDelete?.Invoke(eArgs);

            if (context.Entry(model).State == EntityState.Detached)
                dbSet.Attach(model);

            dbSet.Remove(model);
        }

        public virtual void DeleteRange(params TModel[] models)
        {
            var eArgs = new RepositoryEventArgs<TModel>();
            eArgs.Models = models;

            BeforeDelete?.Invoke(eArgs);

            dbSet.RemoveRange(models);
        }

        #endregion
    }
}
