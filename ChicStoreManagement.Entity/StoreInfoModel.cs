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
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// 品牌ID
        /// </summary>		
        private int _brandId;
        public int BrandID
        {
            get { return _brandId; }
            set { _brandId = value; }
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
        /// 名称
        /// </summary>		
        private string _storeName;
        public string Name
        {
            get { return _storeName; }
            set { _storeName = value; }
        }
        /// <summary>
        /// 地址
        /// </summary>		
        private string _storeAddress;
        public string address
        {
            get { return _storeAddress; }
            set { _storeAddress = value; }
        }
        /// <summary>
        /// 商场
        /// </summary>		
        private string _market;
        public string Market
        {
            get { return _market; }
            set { _market = value; }
        }
        /// <summary>
        /// 地区ID
        /// </summary>		
        private int _districtId;
        public int DistrictID
        {
            get { return _districtId; }
            set { _districtId = value; }
        }
        /// <summary>
        /// 负责人
        /// </summary>		
        private string _principal;
        public string Principal
        {
            get { return _principal; }
            set { _principal = value; }
        }
        /// <summary>
        /// 负责人电话
        /// </summary>		
        private string _principalPhone;
        public string PrincipalPhone
        {
            get { return _principalPhone; }
            set { _principalPhone = value; }
        }
        /// <summary>
        /// 联系人
        /// </summary>		
        private string _linkman;
        public string Linkman
        {
            get { return _linkman; }
            set { _linkman = value; }
        }
        /// <summary>
        /// 联系人电话
        /// </summary>		
        private string _linkmanPhone;
        public string LinkmanPhone
        {
            get { return _linkmanPhone; }
            set { _linkmanPhone = value; }
        }
        /// <summary>
        /// 收货人
        /// </summary>		
        private string _consignee;
        public string Consignee
        {
            get { return _consignee; }
            set { _consignee = value; }
        }
        /// <summary>
        /// 收货地址
        /// </summary>		
        private string _receivingAddress;
        public string ReceivingAddress
        {
            get { return _receivingAddress; }
            set { _receivingAddress = value; }
        }
        /// <summary>
        /// 收货人电话
        /// </summary>		
        private string _consigneePhone;
        public string ConsigneePhone
        {
            get { return _consigneePhone; }
            set { _consigneePhone = value; }
        }
        /// <summary>
        /// 使用面积
        /// </summary>		
        private int _使用面积;
        public int 使用面积
        {
            get { return _使用面积; }
            set { _使用面积 = value; }
        }
        /// <summary>
        /// 等级
        /// </summary>		
        private string _level;
        public string Level
        {
            get { return _level; }
            set { _level = value; }
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



