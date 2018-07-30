using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ChicStoreManagement.IDAL;
using ChicStoreManagement.Model;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Linq;

namespace ChicStoreManagement.DAL
{
    public class StoreEmployeesDAL : IStoreEmployeesDAL
    {
        public bool AddUser(StoreEmploeeModel storeEmploee)
        {
            throw new NotImplementedException();
        }

        public bool DelUser(StoreEmploeeModel storeEmploee)
        {
            throw new NotImplementedException();
        }

        //根据条件获取的员工信息
        public List<StoreEmploeeModel> GetAll(Expression<Func<StoreEmploeeModel,bool>>where)
        {
            var employees =new  List<StoreEmploeeModel>();
            try
            {
                //using (ISession session=NHibernateHelper.SessionFactory.OpenSession())
                //{
                //   var storeEmploeeModels=session.Query<StoreEmploeeModel>().Select(x => new StoreEmploeeModel { ID = x.ID, StoreID = x.StoreID, SerialNumber = x.SerialNumber, Name = x.Name, Gender = x.Gender, JobID = x.JobID, ContactWay =x.ContactWay, StopFlg=x.StopFlg,DocumentMaker=x.DocumentMaker,DocumentData=x.DocumentData,Password=x.Password}).Where(where);

                //  return  storeEmploeeModels.ToList();
                //}
                NHibernateHelper nhibernateHelper = new NHibernateHelper();
                ISession session = nhibernateHelper.GetSession();
                var storeEmploeeModels = session.Query<StoreEmploeeModel>().Select(x => new StoreEmploeeModel { ID = x.ID, StoreID = x.StoreID, SerialNumber = x.SerialNumber, Name = x.Name, Gender = x.Gender, JobID = x.JobID, ContactWay = x.ContactWay, StopFlg = x.StopFlg, DocumentMaker = x.DocumentMaker, DocumentData = x.DocumentData, Password = x.Password }).Where(where);
                if (employees.Count==0) {
                    employees = new List<StoreEmploeeModel>();
                    employees = storeEmploeeModels.ToArray().ToList();
                    
                }
                return employees;
            }
            catch (Exception e)
            {

                throw e;
            }
            
        }

        public StoreEmploeeModel SelOne(string id)
        {
            throw new NotImplementedException();
        }

        public bool UpdateUser(StoreEmploeeModel storeEmploee)
        {
            throw new NotImplementedException();
        }
    }
}
