﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChicStoreManagement.WEB.ViewModel
{
   //echart 门店接待实体图
    public class ReportDatas
    {
        /// <summary>
        /// 时间
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string DataName { get; set; }
    }
}