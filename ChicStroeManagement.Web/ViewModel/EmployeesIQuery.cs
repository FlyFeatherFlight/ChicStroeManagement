//using ChicStoreManagement.BLL;
//using ChicStoreManagement.DAL;
//using ChicStoreManagement.IBLL;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace ChicStoreManagement.WEB.ViewModel
//{
//    public class EmployeesIQuery
//    {
//        private IStoreEmployeesBLL storeEmployeesBLL;
//        private IStoreBLL storeBLL;
//        private IPositionBLL positionBLL;

//        public EmployeesIQuery()
//        {
//        }

//        public EmployeesIQuery(IStoreEmployeesBLL storeEmployeesBLL, IStoreBLL storeBLL, IPositionBLL positionBLL, IQueryable<Employees> queryEmployees)
//        {
//            this.storeEmployeesBLL = storeEmployeesBLL;
//            this.storeBLL = storeBLL;
//            this.positionBLL = positionBLL;
//            IQueryEmployees = queryEmployees;
//        }

//        public IQueryable<Employees> IQueryEmployees { get; private set; }

//        /// <summary>
//        /// 将员工数据优化
//        /// </summary>
//        public void BuildEmploess()
//        {

//            if (IQueryEmployees == null)
//            {
//                List<Employees> ListEmployees = new List<Employees>();
//                var worker = storeEmployeesBLL.GetModels(p => true);//查询初始所有员工信息
//                #region 对员工信息进行数据优化


//                foreach (var item in worker)
//                {
//                    Employees employees = new Employees
//                    {
//                        ID = item.ID,
//                        停用标志 = item.停用标志,
//                        制单人 = item.制单人,
//                        制单日期 = item.制单日期,
//                        姓名 = item.姓名,
//                        密码 = item.密码,
//                        性别 = item.性别,
//                        编号 = item.编号,
//                        职务 = positionBLL.GetModel(p => p.ID == item.职务ID).职务,
//                        店铺 = storeBLL.GetModel(p => p.ID == item.店铺ID).名称,
//                        联系方式 = item.联系方式
//                    };
                    
//                    ListEmployees.Add(employees);
//                }
//                #endregion
//                IQueryEmployees = ListEmployees.AsEnumerable().AsQueryable();
//            }
//        }
//    }
//}