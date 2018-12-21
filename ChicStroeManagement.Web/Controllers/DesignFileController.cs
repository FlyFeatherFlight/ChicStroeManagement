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
using System.IO;
using Microsoft.Win32;

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
        /// <param name="content">文件类型</param>
        /// <returns></returns>
        // GET: DesignFile
        public ActionResult Index(int? id, string sortOrder, string searchString, string currentFilter, int? page,int? content)
        {
            Session["method"] = "N";
            SetEmployee();//获取当前人员信息
            ViewBag.Store = store;
            ViewBag.Employee = employeeName;
            ViewBag.DesignID = id;
            ViewBag.IsManager = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否店长;
            ViewBag.IsDesigner = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否设计师;
            ViewBag.IsEmployee = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否销售;
            ViewBag.DesignFileCurrentSort = sortOrder;
            ViewBag.DesignFileDate = String.IsNullOrEmpty(sortOrder) ? "first_desc" : "";
            ViewBag.DesignFileResult = sortOrder == "last" ? "last_desc" : "last";
            if (id == 0||id==null)
            {
                return Content("<script>alert('设计案ID获取出错！');window.history.go(-1);</script>");
            }
            List<DesignFileViewModel> designFileModels = new List<DesignFileViewModel>();
            //构建设计表信息  
            if (BuildFileInfo(id, content)!=null)
            {
                designFileModels = BuildFileInfo(id, content).ToList();
            }
          
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
        public ActionResult FileUpload(int? id,string content)
        {
            Session["method"] = "N";
            ViewBag.content = content;
            FileViewModel model = new FileViewModel();
            SetEmployee();
            ViewBag.IsManager = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否店长;
            ViewBag.IsDesigner = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否设计师;
            ViewBag.IsEmployee = storeEmployeesBLL.GetModel(p => p.ID == employeeID).是否销售;
            ViewBag.Store = store;
            ViewBag.Employee = employeeName;
            var lis=fileTypeBLL.GetModels(p => true);
            if (id == 0||id==null)
            {
                return Content("<script>alert('设计案ID获取出错！');window.history.go(-1);</script>");
            }
            model.DesignId = id.Value;
            return View(model);
        }

        /// <summary>
        /// 上传文件的具体方法
        /// </summary>
        /// <returns>成功则为true，失败异常则为False</returns>
        public JsonResult UploadFile(FileViewModel fileModel)
        {
            string msg = "true";
           
            if (fileModel.DesignId == 0)
            {
                msg = "设计案ID获取出错！";
                return Json(msg);
            }
            //if (Session["method"].ToString() == "Y")
            //{
            //    string str = string.Format("<script>alert('重复操作！');parent.location.href='TrackLogIndex';</script>");
            //    return Json(str);
            //}
            SetEmployee();



            fileModel.FileName = fileModel.UploadStream.FileName;
            fileModel.UserName = employeeName;
            fileModel.StoreName = storeBLL.GetModel(p => p.ID == storeID).名称;

            var m = fileModel.UploadStream.InputStream;
            msg = IsAllowedExtension(fileModel.UploadStream, fileModel.Filetype);
            if (msg != "true")
            {
                return Json(msg);//如果上传文件不匹配，则返回出错信息！
            }
            var path = UploadManager.SaveFile(fileModel.UploadStream, fileModel.FileName, fileModel.StoreName, fileModel.DesignId.ToString(), fileModel.Filetype.ToString());//获得上传路径
            fileModel.Path = path;
            if (path == null )/*|| design_ProjectDrawingsBLL.GetModel(p => p.存储路径 == path) != null)*/
            {
                return Json("false");
            }
            销售_设计案提交表_项目相关图纸 model = new 销售_设计案提交表_项目相关图纸
            {
                提交人 = fileModel.UserName,
                提交时间 = DateTime.Now,
                更新人 = fileModel.UserName,
                更新日期 = DateTime.Now,
                设计案提交表ID = fileModel.DesignId,
                文件名 = fileModel.FileName
            };
            model.设计案提交表ID = fileModel.DesignId;
            var s = "";
            switch (fileModel.Filetype)
            {
                case FileType.CAD图:
                    s = fileModel.Filetype.ToString();
                    model.存储路径 = fileModel.Path;
                    model.文件类型 = fileTypeBLL.GetModel(p => p.文件类型 == s).ID;
                    break;
                case FileType.效果3D图:
                    model.存储路径 = fileModel.Path;
                    s = fileModel.Filetype.ToString();
                    model.文件类型 = fileTypeBLL.GetModel(p => p.文件类型 == s).ID;
                    break;
                case FileType.对比图:
                    model.存储路径 = fileModel.Path;
                    s = fileModel.Filetype.ToString();
                    model.文件类型 = fileTypeBLL.GetModel(p => p.文件类型 == s).ID;
                    break;
                case FileType.PDF文件:
                    model.存储路径 = fileModel.Path;
                    s = fileModel.Filetype.ToString();
                    model.文件类型 = fileTypeBLL.GetModel(p => p.文件类型 == s).ID;
                    break;
                case FileType.PPT文件:
                    model.存储路径 = fileModel.Path;
                    s = fileModel.Filetype.ToString();
                    model.文件类型 = fileTypeBLL.GetModel(p => p.文件类型 == s).ID;

                    break;
                case FileType.Excel文件:
                    model.存储路径 = fileModel.Path;
                    s = fileModel.Filetype.ToString();
                    model.文件类型 = fileTypeBLL.GetModel(p => p.文件类型 == s).ID;
                    break;
                case FileType.其它:
                    model.存储路径 = fileModel.Path;
                    s = fileModel.Filetype.ToString();
                    model.文件类型 = fileTypeBLL.GetModel(p => p.文件类型 == s).ID;
                    break;
                case FileType.完成效果文件:
                    model.存储路径 = fileModel.Path;
                    s = fileModel.Filetype.ToString();
                    model.文件类型 = fileTypeBLL.GetModel(p => p.文件类型 == s).ID;
                    break;
                default:
                    break;
            }

            if (ModelState.IsValid)
            {
                design_ProjectDrawingsBLL.Add(model);
                Session["method"] = "Y";
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
                msg = "上传文件出错：";
                foreach (var item in sb)
                {
                    msg+= item.ToString() + "<br/>";
                }
                return Json(msg);
            }
            msg = "上传成功！";
            return Json(msg);
        }

     
        /// <summary>
        /// 下载文件的具体方法
        /// </summary>
        /// <returns>成功则为true，失败异常则为false</returns>
        public ActionResult DownLoadFile(string path,string fileName)
        {
          
            //path = Server.MapPath(path);
            string msg = "";
            if (!System.IO.File.Exists(path))
            {
               var model= design_ProjectDrawingsBLL.GetModel(p => p.存储路径 == path);
                model.备注 = "false";
                try
                {
                    design_ProjectDrawingsBLL.Modify(model);
                }
                catch (Exception e)
                {
                   
                    return Content(e.Message);
                }
                
                msg = "<script>alert('文件下载失败！');parent.location.href='Index';</script>";
                return Content(msg);
            }
            FileStream fs = new FileStream(path, FileMode.Open);
            byte[] bytes = new byte[(int)fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            FileInfo fi = new FileInfo(path);
            //**********处理可以解决文件类型问题
            string fileextname = fi.Extension;
            string DEFAULT_CONTENT_TYPE = "application/unknown";
            RegistryKey regkey, fileextkey;
            string filecontenttype;
            try
            {
                regkey = Registry.ClassesRoot;
                fileextkey = regkey.OpenSubKey(fileextname);
                filecontenttype = fileextkey.GetValue("Content Type", DEFAULT_CONTENT_TYPE).ToString();
            }
            catch
            {
                filecontenttype = DEFAULT_CONTENT_TYPE;
            }
            //**********end
            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Response.ContentType = filecontenttype;


            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);//设置文件信息
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
            return new EmptyResult();

            
        }
        public void DownloadFiletest(string path,string fileName)
        {
     
            //判断文件是否存在
            if (!System.IO.File.Exists(path))
            {
                Response.Write("该文件不存在服务器上");
                Response.End();
                return;
            }
            FileInfo fi = new FileInfo(path);
            //**********处理可以解决文件类型问题
            string fileextname = fi.Extension;
            string DEFAULT_CONTENT_TYPE = "application/unknown";
            RegistryKey regkey, fileextkey;
            string filecontenttype;
            try
            {
                regkey = Registry.ClassesRoot;
                fileextkey = regkey.OpenSubKey(fileextname);
                filecontenttype = fileextkey.GetValue("Content Type", DEFAULT_CONTENT_TYPE).ToString();
            }
            catch
            {
                filecontenttype = DEFAULT_CONTENT_TYPE;
            }
            //**********end
            Response.Clear();
            Response.Charset = "utf-8";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
            Response.ContentType = filecontenttype;
            Response.WriteFile(path);
            Response.End();
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

     
        
        private IQueryable<DesignFileViewModel> BuildFileInfo(int? id,int? content)
        {
            List<DesignFileViewModel> models = new List<DesignFileViewModel>();
            List<销售_设计案提交表_项目相关图纸> fileModel = new List<销售_设计案提交表_项目相关图纸>();
            try
            {
                if (id != 0 && id != null)
                {   //根据接待ID查询当前客户的设计提交表图纸
                    fileModel = design_ProjectDrawingsBLL.GetModels(p => p.设计案提交表ID == id).ToList();
                }
               else  if (storeEmployeesBLL.GetModel(p => p.ID == employeeID).职务ID == 3)
                {   //店长可以查看所有图纸信息
                    fileModel = design_ProjectDrawingsBLL.GetModels(p => true).ToList();
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
            if (content==8)
            {
               fileModel=fileModel.Where(p => p.文件类型 == 8).ToList();
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

        /// <summary>
        /// 判断选择的文件类型是否与上传类型相符合
        /// </summary>
        /// <param name="filestream"></param>
        /// <param name="fileType">文件类型</param>
        /// <returns>返回的是否匹配的信息或者异常信息</returns>
        public string IsAllowedExtension(HttpPostedFileBase filestream,FileType fileType) {
            string isAllow = "";
            string fileclass = "";
            try
            {
                BinaryReader reader = new BinaryReader(filestream.InputStream);
                
                for (int i = 0; i < 2; i++)
                {
                    fileclass += reader.ReadByte().ToString();
                }
            }
            catch (Exception e)
            {

                return e.Message;
            }
           
            switch (fileType)
            {
                case FileType.CAD图:
                    if (fileclass =="255216"||fileclass=="13780")
                    {
                        isAllow = "true";
                    }
                    else
                    {
                        isAllow = "请上传JPG或PNG格式文件。";
                    }
                    break;
                case FileType.效果3D图:
                    if (fileclass == "255216" || fileclass == "13780")
                    {
                        isAllow = "true";
                    }
                    else
                    {
                        isAllow = "请上传JPG或PNG格式文件。";
                    }
                    break;
                case FileType.对比图:
                    if (fileclass == "255216" || fileclass == "13780")
                    {
                        isAllow = "true";
                    }
                    else
                    {
                        isAllow = "请上传JPG或PNG格式文件。";
                    }
                    break;
                case FileType.PDF文件:
                    if (fileclass == "3780" )
                    {
                        isAllow = "true";
                    }
                    else
                    {
                        isAllow = "请上传pdf格式文件。";
                    }
                    break;
                case FileType.PPT文件:
                    if (fileclass == "208207"|| fileclass == "8075")
                    {
                        isAllow = "true";
                    }
                    else
                    {
                        isAllow = "请上传ppt,doc,xls,xlsx,docx,pptx格式文件。";
                    }
                    break;
                case FileType.Excel文件:
                    if (fileclass == "8075"|| fileclass == "208207")
                    {
                        isAllow = "true";
                    }
                    else
                    {
                        isAllow = "请上传xlsx,docx,pptx,ppt,doc,xls格式文件";
                    }
                    break;
                case FileType.其它:
                    isAllow = "true";
                    break;
                case FileType.完成效果文件:
                    isAllow = "true";
                    break;
                default:
                    isAllow = "未知格式文件!";
                    break;
            }

            return isAllow;

        }
    }
}