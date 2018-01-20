// (c) Copyright KTC 2012 by HOMAY
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using DataAccessHandler.Collections;

namespace DataAccessHandler
{
    public struct PrimaryKey
    {
        public string Name;
        public object Value;
    }

    public abstract class IEntity<T>:Object,ICloneable where T:IEntity<T>
    {
        #region Private Memebers
        private List<PrimaryKey> _primaryKey = new List<PrimaryKey>();
        #endregion
        
        #region Publick Members
        private Entities<T> _collection = null;
        public Entities<T> Collection
        {
            get { return _collection; }
        }
        
        //public T This;
        #endregion

        #region Internal Memebers
        public string EntityName 
        { 
            get 
            { 
                string[] temp = this.ToString().Split('.');
                return temp[temp.Length -1]; 
            } 
        }
        public List<PrimaryKey> PrimaryKey
        {
            get
            {
                string experssion = "[System.ComponentModel.DataObjectFieldAttribute((Boolean)True, (Boolean)True, (Boolean)False)]";
                PropertyInfo[] propertyInfo = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                _primaryKey.Clear();
                foreach (var item in propertyInfo)
                {
                    var p = item.GetCustomAttributesData().Where(a => a.ToString() == experssion).FirstOrDefault();
                    if (p != null)
                    {
                        PrimaryKey primaryKey = new PrimaryKey();
                        primaryKey.Name = item.Name.ToString();
                        primaryKey.Value = item.GetValue(this, null);
                        _primaryKey.Add(primaryKey);
                    }
                }
                return _primaryKey;
            }
        }
        #endregion

        #region Public Mehtods

        public IEntity()
        {
            this._collection = new Entities<T>();
        }

        public virtual bool Update(T entity)
        {
            //ToDo: We have not any idea when the primary key is more than one item solve this problem :-&
            T temp = this.Collection.Where(e => e.PrimaryKey.First().Value.ToString() == entity.PrimaryKey.First().Value.ToString()).First();
            int index =   this.Collection.IndexOf(temp);
            bool result = DataAccessAdapter.DoEntity<T>(entity, SqlCommand.Update, "");
            if (result && index >= 0)
            {
                this.Collection.RemoveAt(index);
                this.Collection.Insert(index,entity);
            }
            return result;
        }
        public virtual bool Update(T entity, string where)
        {
            //ToDo: We have not any idea when the primary key is more than one item solve this problem :-&
            int index = this.Collection.IndexOf(Collection.Where(e => e.PrimaryKey.First().Value == entity.PrimaryKey.First().Value).First());
            bool result = DataAccessAdapter.DoEntity(entity, SqlCommand.Update, where);;
            if (result && index >= 0)
            {
                this.Collection.RemoveAt(index);
                this.Collection.Insert(index, entity);
            }
            return result;
        }
        public virtual bool Delete(T entity)
        {
            bool result = DataAccessAdapter.DoEntity(entity, SqlCommand.Delete, "");
            if (result)
                Collection.Remove(entity);
            return result;
        }
        public virtual bool Delete(T entity, string where)
        {
            bool result = DataAccessAdapter.DoEntity(entity, SqlCommand.Delete, where);
            if (result)
                Collection.Remove(entity);
            return result;
        }

        public virtual bool Insert(T entity)
        {
            bool result = DataAccessAdapter.DoEntity(entity, SqlCommand.Insert, "");
            if (result)
                Collection.Add(entity);
            return result;
        }

        public virtual string SqlSelect(T entity)
        {
            return DataAccessAdapter.GenerateCommand(SqlCommand.Select, entity, "");
        }

        public virtual string SqlSelect(T entity, string where)
        {
            return DataAccessAdapter.GenerateCommand<T>(SqlCommand.Select, entity, where);
        }

              
        public virtual bool Load(string where = "")
        {
            try
            {
                if (this != null)
                {
                    List<Dictionary<string, object>> entityDictionary = null;
                    if (where == "")
                        DataAccessAdapter.ExecuteReader(this.SqlSelect((T)this), out entityDictionary);
                    else if (where != "")
                        DataAccessAdapter.ExecuteReader(this.SqlSelect((T)this, where), out entityDictionary);

                    foreach (var item in entityDictionary)
                    {
                        Type type = this.GetType();
                        IEntity<T> newEntity = FormatterServices.GetUninitializedObject(type) as IEntity<T>;
                        ConstructorInfo ci = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, System.Type.EmptyTypes, null);
                        if (ci != null)
                        {
                            ci.Invoke(newEntity, null);
                            Dictionary<string, object> thisInfo = DataAccessAdapter.GetInfo((T)this);

                            foreach (var info in thisInfo)
                            {
                                PropertyInfo propertyInfo = type.GetProperty(info.Key);
                                if (propertyInfo.CanWrite)
                                {
                                    if (item.ToList().Exists(i => i.Key == info.Key))
                                    {
                                        object value = item.Where(i => i.Key == info.Key).First().Value;
                                        if ( value.GetType() != DBNull.Value.GetType() && value != null && propertyInfo.PropertyType.IsEnum == false)
                                            propertyInfo.SetValue(newEntity, value, null);
                                        else if (value != null && propertyInfo.PropertyType.IsEnum)
                                        {
                                            propertyInfo.SetValue(newEntity ,Enum.Parse(propertyInfo.PropertyType,value.ToString()),null);
                                        }
                                    }
                                }
                            }
                            _collection.Add((T)newEntity);
                        }
                    }
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        public virtual T Copy()
        {
            Type type = this.GetType();
            T newEntity = FormatterServices.GetUninitializedObject(type) as T;
            ConstructorInfo ci = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, System.Type.EmptyTypes, null);
            if (ci != null)
            {
                ci.Invoke(newEntity, null);
                Dictionary<string, object> thisInfo = DataAccessAdapter.GetInfo((T)this);

                foreach (var info in thisInfo)
                {
                    PropertyInfo propertyInfo = type.GetProperty(info.Key);
                    if (propertyInfo.CanWrite)
                    {
                        object value = thisInfo.Where(i => i.Key == info.Key).First().Value;
                        if(value != null)
                            propertyInfo.SetValue(newEntity, value, null);
                    }
                }
                newEntity = (T)this.MemberwiseClone();
            }
            return newEntity;
        }

        #endregion

        
    }
}
