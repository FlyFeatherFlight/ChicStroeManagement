using ChicStoreManagement.IBLL;
using ChicStoreManagement.WEB.Utils;
using ChicStoreManagement.WEB.ViewModel;
using System;
using System.Collections.Generic;
using ChicStoreManagement.Model;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using PagedList;
using System.Web;

namespace ChicStoreManagement.WEB.Controllers
{
    public class DesignFileController : Controller
    {
        private readonly ICustomerInfoBLL customerInfoBLL;
        private readonly IDesign_CustomerExceptedBuyBLL design_CustomerExceptedBuyBLL;
        private readonly IDesignSubmitBLL designSubmitBLL;
        private readonly IExceptedBuyBLL exceptedBuyBLL;
        private readonly IProductCodeBLL productCodeBLL;
        private readonly IDesign_ProjectDrawingsBLL design_ProjectDrawingsBLL;
        private readonly IFileTypeBLL fileTypeBLL;

        private readonly IStoreBLL storeBLL;
        private readonly IPositionBLL positionBLL;
        private readonly IStoreEmployeesBLL storeEmployeesBLL;
        private int employeeID;
        private string employeeName;
        private string store;
        private int storeID;
        private int DesignSubId=0;

        public DesignFileController(ICustomerInfoBLL customerInfoBLL, IDesign_CustomerExceptedBuyBLL design_CustomerExceptedBuyBLL, IDesignSubmitBLL designSubmitBLL, IExceptedBuyBLL exceptedBuyBLL, IProductCodeBLL productCodeBLL, IDesign_ProjectDrawingsBLL design_ProjectDrawingsBLL, IFileTypeBLL fileTypeBLL, IStoreBLL storeBLL, IPositionBLL positionBLL, IStoreEmployeesBLL storeEmployeesBLL)
        {
            this.customerInfoBLL = customerInfoBLL;
            this.design_CustomerExceptedBuyBLL = design_CustomerExceptedBuyBLL;
            this.designSubmitBLL = designSubmitBLL;
            this.exceptedBuyBLL = exceptedBuyBLL;
            this.productCodeBLL = productCodeBLL;
            this.design_ProjectDrawingsBLL = design_ProjectDrawingsBLL;
            this.fileTypeBLL = fileTypeBLL;
            this.storeBLL = storeBLL;
            this.positionBLL = positionBLL;
            this.storeEmployeesBLL = storeEmployeesBLL;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">设计案申请id</param>
        /// <param name="sortOrder"></param>
        /// <param name="searchString"></param>
        /// <param name="currentFilter"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        // GET: DesignFile
        public ActionResult Index(int? id, string sortOrder, string searchString, string currentFilter, int? page)
        {
            Session["method"] = "N";
            SetEmployee();//获取当前人员信息
            Session["DesignID"] = id;
            ViewBag.DesignFileCurrentSort = sortOrder;
            ViewBag.DesignFileDate = String.IsNullOrEmpty(sortOrder) ? "first_desc" : "";
            ViewBag.DesignFileResult = sortOrder == "last" ? "last_desc" : "last";
            if (id == 0)
            {
                return Content("<script>alert('设计案ID获取出错！');window.history.go(-1);</script>");
            }
            List<DesignFileViewModel> designFileModels = new List<DesignFileViewModel>();
            //构建设计表信息  
            designFileModels = BuildFileInfo(id).ToList();
            ViewBag.DesignPeopleName = employeeName;//将当前操作人员传到前端
            ViewBag.DesignstoreName = store;//将当前店铺名字传到前端
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.DesignCurrentFilter = searchString;//获得前端传回来的搜索关键词

            if (!string.IsNullOrEmpty(searchString))
            {
                designFileModels = designFileModels.Where(w => w.设计案提交表序号.ToString()== searchString).ToList();
            }
            #region 排序，默认按ID升序
            switch (sortOrder)
            {
                case "first_desc":
                    designFileModels = designFileModels.OrderByDescending(w => w.提交时间).ToList();
                    break;
                case "last_desc":
                    designFileModels = designFileModels.OrderByDescending(w => w.提交人).ToList();
                    break;
                case "last":
                    designFileModels = designFileModels.OrderBy(w => w.提交人).ToList();
                    break;
                default:
                    designFileModels = designFileModels.OrderBy(w => w.提交时间).ToList();
                    break;
            }

            #endregion

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(designFileModels.ToPagedList(pageNumber, pageSize));

        }

      

        /// <summary>
        /// 根据设计案上传文件
        /// </summary>
        /// <param name="id">设计申请ID</param>
        /// <returns></returns>
        public ActionResult FileUpload(int? id)
        {
            Session["method"] = "N";
            FileViewModel model = new FileViewModel();

           
            if (id == 0)
            {
                return Content("<script>alert('设计案ID获取出错！');window.history.go(-1);</script>");
            }
            return View(model);
        }

        /// <summary>
        /// 上传文件的具体方法
        /// </summary>
        /// <returns>成功则为true，失败异常则为False</returns>
        public JsonResult UploadFile(FileViewModel fileModel)
        {
            fileModel.DesignId =Int32.Parse(Session["DesignID"].ToString());
            if (fileModel.DesignId == 0)
            {
                return Json("<script>alert('设计案ID获取出错！');window.history.go(-1);</script>");
            }
            //if (Session["method"].ToString() == "Y")
            //{
            //    string str = string.Format("<script>alert('重复操作！');parent.location.href='TrackLogIndex';</script>");
            //    return Json(str);
            //}
            SetEmployee();

            if (ModelState.IsValid)
            {
                
                fileModel.FileName = fileModel.UploadStream.FileName;
                fileModel.UserName = employeeName;
                fileModel.StoreName = storeBLL.GetModel(p => p.ID == storeID).名称;

                var m = fileModel.UploadStream.InputStream.;
                //var path = UploadManager.SaveFile(fileModel.UploadStream.InputStream, fileModel.FileName, fileModel.StoreName, fileModel.DesignId.ToString(), fileModel.FileType.ToString());//获得上传路径
                //fileModel.Path = path;
                //if (path == null)
                //{
                //    return Json("false");
                //}
                销售_设计案提交表_项目相关图纸 model = new 销售_设计案提交表_项目相关图纸
                {
                    提交人 = fileModel.UserName,
                    提交时间 = DateTime.Now,
                    更新人 = fileModel.UserName,
                    更新日期 = DateTime.Now,
                    设计案提交表ID = fileModel.DesignId,
                    文件名=fileModel.FileName
                };
               
                var s="";
                switch (fileModel.FileType)
                {
                    case FileType.CAD图:
                        s = fileModel.FileType.ToString();
                        //if (fileModel.UploadStream.ContentType)
                        //{

                        //}
                        model.存储路径 = fileModel.Path;
                        model.文件类型 = fileTypeBLL.GetModel(p => p.文件类型 == s).ID;
                        break;
                    case FileType.效果3D图:
                        model.存储路径 = fileModel.Path;
                         s = fileModel.FileType.ToString();
                        model.文件类型 = fileTypeBLL.GetModel(p => p.文件类型 == s).ID;
                        break;
                    case FileType.对比图:
                        model.存储路径 = fileModel.Path;
                         s = fileModel.FileType.ToString();
                        model.文件类型 = fileTypeBLL.GetModel(p => p.文件类型 == s).ID;
                        break;
                    case FileType.PDF文件:
                        model.存储路径 = fileModel.Path;
                        s = fileModel.FileType.ToString();
                        model.文件类型 = fileTypeBLL.GetModel(p => p.文件类型 == s).ID;

                        break;
                    case FileType.PPT文件:
                        model.存储路径 = fileModel.Path;
                        s = fileModel.FileType.ToString();
                        model.文件类型 = fileTypeBLL.GetModel(p => p.文件类型 == s).ID;

                        break;
                    case FileType.Excel文件:
                        model.存储路径 = fileModel.Path;
                        s = fileModel.FileType.ToString();
                        model.文件类型 = fileTypeBLL.GetModel(p => p.文件类型 == s).ID;
                        break;
                    default:
                        break;
                }

                if (ModelState.IsValid)
                {
                    //design_ProjectDrawingsBLL.Add(model);
                    Session["method"] = "Y";
                }


                ModelState.Clear();
            }
            else
            {
                List<string> sb = new List<string>();
                //获取所有错误的Key
                List<string> Keys = ModelState.Keys.ToList();
                //获取每一个key对应的ModelStateDictionary
                foreach (var key in Keys)
                {
                    var errors = ModelState[key].Errors.ToList();
                    //将错误描述添加到sb中
                    foreach (var error in errors)
                    {
                        sb.Add(error.ErrorMessage);
                    }
                }
                string s = "修改日志出错：";
                foreach (var item in sb)
                {
                    s += item.ToString() + "<br/>";
                }
                return Json("<script>alert('" + s + "');window.history.go(-1);</script>");
            }
            return Json("success");
        }

        public JsonResult Upload(HttpPostedFileBase httpPostedFileBase) {


            return Json("false");
        }

        /// <summary>
        /// 下载文件的具体方法
        /// </summary>
        /// <returns>成功则为true，失败异常则为false</returns>
        public JsonResult DownLoadFile(string path)
        {
            return null;
        }

        /// <summary>
        /// 设置当前的用户信息
        /// </summary>
        public void SetEmployee()
        {

            string userName = HttpContext.User.Identity.Name;
            if (userName != null)
            {
                var employees = HttpContext.Session["Employee"] as Employees;
                employeeID = employees.ID;
                employeeName = employees.姓名;
                store = employees.店铺;
                storeID = storeBLL.GetModel(p => p.名称 == store).ID;
            }
        }

        /// <summary>
        /// 构建客户信息
        /// </summary>
        /// <returns>客户信息</returns>
        public IQueryable<CustomerInfoModel> BuildCustomerInfo()
        {

            List<CustomerInfoModel> customerInfoModelsList = new List<CustomerInfoModel>();

            var customer = customerInfoBLL.GetModels(p => p.店铺ID == storeID);//查询当前店铺所有顾客接待信息
            if (customer != null)
            {
                foreach (var item in customer)
                {
                    CustomerInfoModel customerInfo = new CustomerInfoModel();
                    try
                    {


                        customerInfo.ID = item.ID;
                        customerInfo.店铺 = storeBLL.GetModel(p => p.ID == item.店铺ID).名称;
                        customerInfo.接待人 = storeEmployeesBLL.GetModel(p => p.ID == item.接待人ID).姓名;
                        customerInfo.接待序号 = item.接待序号;
                        customerInfo.接待日期 = item.接待日期.ToString("d");
                        customerInfo.主导者 = item.主导者;
                        customerInfo.主导者喜好风格 = item.主导者喜好风格;
                        customerInfo.使用空间 = item.使用空间;
                        customerInfo.出店时间 = item.出店时间;
                        customerInfo.制单日期 = item.制单日期;
                        customerInfo.同行人 = item.同行人;
                        customerInfo.如何得知品牌 = item.如何得知品牌;
                        customerInfo.安装地址 = item.安装地址;
                        customerInfo.客户姓名 = item.客户姓名;
                        customerInfo.客户建议 = item.客户建议;
                        customerInfo.客户来源 = item.客户来源;
                        customerInfo.客户电话 = item.客户电话;
                        customerInfo.客户着装 = item.客户着装;
                        customerInfo.客户类别 = item.客户类别;
                        customerInfo.客户类型 = item.客户类型;
                        customerInfo.客户职业 = item.客户职业;
                        customerInfo.家庭成员 = item.家庭成员;
                        customerInfo.年龄段 = item.年龄段;
                        customerInfo.性别 = item.性别;
                        customerInfo.是否有意向 = item.是否有意向;
                        if (item.更新人 != null)
                        {
                            customerInfo.更新人 = storeEmployeesBLL.GetModel(p => p.ID == item.更新人).姓名;
                        }

                        customerInfo.更新日期 = item.更新日期;
                        customerInfo.来店次数 = item.来店次数;
                        customerInfo.比较品牌 = item.比较品牌;
                        customerInfo.特征 = item.特征;
                        customerInfo.社交软件 = item.社交软件;
                        customerInfo.装修情况 = item.装修情况;
                        customerInfo.装修进度 = item.装修进度;
                        customerInfo.装修风格 = item.装修风格;
                        customerInfo.设计师 = item.设计师;
                        if (item.跟进人ID != null)
                        {
                            customerInfo.跟进人 = storeEmployeesBLL.GetModel(p => p.ID == item.跟进人ID).姓名;
                        }

                        customerInfo.返点 = item.返点;
                        customerInfo.进店时长 = item.进店时长;
                        customerInfo.进店时间 = item.进店时间;
                        customerInfo.预报价折扣 = item.预报价折扣;
                        customerInfo.预算金额 = item.预算金额;
                        customerInfo.预计使用时间 = item.预计使用时间;


                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }

                    customerInfoModelsList.Add(customerInfo);
                }
            }


            return customerInfoModelsList.AsEnumerable().AsQueryable();

        }

        private IQueryable<DesignFileViewModel> BuildFileInfo(int? id)
        {
            List<DesignFileViewModel> models = new List<DesignFileViewModel>();
            List<销售_设计案提交表_项目相关图纸> fileModel = new List<销售_设计案提交表_项目相关图纸>();
            try
            {
                if (storeEmployeesBLL.GetModel(p => p.ID == employeeID).职务ID == 3)
                {   //店长可以查看所有图纸信息
                    fileModel = design_ProjectDrawingsBLL.GetModels(p => true).ToList();
                }
                else if (id != 0 && id != null)
                {   //根据接待ID查询当前客户的设计提交表图纸
                    fileModel = design_ProjectDrawingsBLL.GetModels(p => p.设计案提交表ID == id).ToList();
                }
                else
                {
                    //查询当前销售人员的设计提交表图纸
                    fileModel = design_ProjectDrawingsBLL.GetModels(p => p.提交人 == employeeName).ToList();
                }

            }
            catch (Exception e)
            {

                fileModel = null;
            }

            if (fileModel != null)
            {
                foreach (var item in fileModel)
                {
                    DesignFileViewModel fileViewModel = new DesignFileViewModel
                    {
                        Id = item.id,
                        备注 = item.备注,
                        提交人 = item.提交人,
                        提交时间 = item.提交时间,
                        更新人 = item.更新人,
                        更新日期 = item.更新日期,
                        设计案提交表序号 = item.设计案提交表ID,
                        文件类型 = fileTypeBLL.GetModel(p => p.ID == item.文件类型).文件类型,
                        文件名=item.文件名,
                        Path = item.存储路径
                    };
                    models.Add(fileViewModel);
                }
                return models.AsQueryable();
            }
            return null;
        }

    }
}