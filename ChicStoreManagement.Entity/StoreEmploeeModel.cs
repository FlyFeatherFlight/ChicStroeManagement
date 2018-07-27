using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicStoreManagement.Entity
{

    /// <summary>
    /// 店铺人员档案
    /// </summary>
    public class StoreEmploeeModel
    {
        /// <summary>
        /// ID
        /// </summary>		
        private int _id;
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// 店铺ID
        /// </summary>		
        private int _storeId;
        public int StoreID
        {
            get { return _storeId; }
            set { _storeId = value; }
        }
        /// <summary>
        /// 编号
        /// </summary>		
        private string _serialNumber;
        public string SerialNumber
        {
            get { return _serialNumber; }
            set { _serialNumber = value; }
        }
        /// <summary>
        /// 姓名
        /// </summary>		
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// 性别
        /// </summary>		
        private string _gender;
        public string Gender
        {
            get { return _gender; }
            set { _gender = value; }
        }
        /// <summary>
        /// 职务ID
        /// </summary>		
        private int _jobId;
        public int JobID
        {
            get { return _jobId; }
            set { _jobId = value; }
        }
        /// <summary>
        /// 联系方式
        /// </summary>		
        private string _contactWay;
        public string ContactWay
        {
            get { return _contactWay; }
            set { _contactWay = value; }
        }
        /// <summary>
        /// 停用标志
        /// </summary>		
        private bool _stopFlg;
        public bool StopFlg
        {
            get { return _stopFlg; }
            set { _stopFlg = value; }
        }
        /// <summary>
        /// 制单人
        /// </summary>		
        private int _documentMaker;
        public int DocumentMaker
        {
            get { return _documentMaker; }
            set { _documentMaker = value; }
        }
        /// <summary>
        /// 制单日期
        /// </summary>		
        private DateTime _documentData;
        public DateTime DocumentData
        {
            get { return _documentData; }
            set { _documentData = value; }
        }
        /// <summary>
        /// 密码
        /// </summary>		
        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

    }
}

