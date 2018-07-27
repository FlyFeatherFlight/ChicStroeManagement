using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicStoreManagement.Model
{

    /// <summary>
    /// 店铺销售职位
    /// </summary>
   public  class StoreJobTitleModel
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
            /// 职务
            /// </summary>		
            private string _JobTitle;
            public string JobTitle
        {
                get { return _JobTitle; }
                set { _JobTitle = value; }
            }

        }
    }
