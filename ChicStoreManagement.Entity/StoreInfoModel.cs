using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicStoreManagement.Model

{

    /// <summary>
    /// 店铺档案
    /// </summary>
    class StoreInfoModel
    {

        /// <summary>
        /// ID
        /// </summary>		
        private int _id;
        public virtual int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// 品牌ID
        /// </summary>		
        private int _brandId;
        public virtual int BrandID
        {
            get { return _brandId; }
            set { _brandId = value; }
        }
        /// <summary>
        /// 编号
        /// </summary>		
        private string _serialNumber;
        public virtual string SerialNumber
        {
            get { return _serialNumber; }
            set { _serialNumber = value; }
        }
        /// <summary>
        /// 名称
        /// </summary>		
        private string _storeName;
        public virtual string Name
        {
            get { return _storeName; }
            set { _storeName = value; }
        }
        /// <summary>
        /// 地址
        /// </summary>		
        private string _storeAddress;
        public virtual string address
        {
            get { return _storeAddress; }
            set { _storeAddress = value; }
        }
        /// <summary>
        /// 商场
        /// </summary>		
        private string _market;
        public virtual string Market
        {
            get { return _market; }
            set { _market = value; }
        }
        /// <summary>
        /// 地区ID
        /// </summary>		
        private int _districtId;
        public virtual int DistrictID
        {
            get { return _districtId; }
            set { _districtId = value; }
        }
        /// <summary>
        /// 负责人
        /// </summary>		
        private string _principal;
        public virtual string Principal
        {
            get { return _principal; }
            set { _principal = value; }
        }
        /// <summary>
        /// 负责人电话
        /// </summary>		
        private string _principalPhone;
        public virtual string PrincipalPhone
        {
            get { return _principalPhone; }
            set { _principalPhone = value; }
        }
        /// <summary>
        /// 联系人
        /// </summary>		
        private string _linkman;
        public virtual string Linkman
        {
            get { return _linkman; }
            set { _linkman = value; }
        }
        /// <summary>
        /// 联系人电话
        /// </summary>		
        private string _linkmanPhone;
        public virtual string LinkmanPhone
        {
            get { return _linkmanPhone; }
            set { _linkmanPhone = value; }
        }
        /// <summary>
        /// 收货人
        /// </summary>		
        private string _consignee;
        public virtual string Consignee
        {
            get { return _consignee; }
            set { _consignee = value; }
        }
        /// <summary>
        /// 收货地址
        /// </summary>		
        private string _receivingAddress;
        public virtual string ReceivingAddress
        {
            get { return _receivingAddress; }
            set { _receivingAddress = value; }
        }
        /// <summary>
        /// 收货人电话
        /// </summary>		
        private string _consigneePhone;
        public virtual string ConsigneePhone
        {
            get { return _consigneePhone; }
            set { _consigneePhone = value; }
        }
        /// <summary>
        /// 使用面积
        /// </summary>		
        private int _area;
        public virtual int Area
        {
            get { return _area; }
            set { _area = value; }
        }
        /// <summary>
        /// 等级
        /// </summary>		
        private string _level;
        public virtual string Level
        {
            get { return _level; }
            set { _level = value; }
        }
        /// <summary>
        /// 停用标志
        /// </summary>		
        private bool _stopFlg;
        public virtual bool StopFlg
        {
            get { return _stopFlg; }
            set { _stopFlg = value; }
        }
        /// <summary>
        /// 制单人
        /// </summary>		
        private int _documentMaker;
        public virtual int DocumentMaker
        {
            get { return _documentMaker; }
            set { _documentMaker = value; }
        }
        /// <summary>
        /// 制单日期
        /// </summary>		
        private DateTime _documentData;
        public virtual DateTime DocumentData
        {
            get { return _documentData; }
            set { _documentData = value; }
        }
        /// <summary>
        /// 密码
        /// </summary>		
        private string _password;
        public virtual string Password
        {
            get { return _password; }
            set { _password = value; }
        }

    }
}



