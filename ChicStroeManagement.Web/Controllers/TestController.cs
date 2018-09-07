using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ChicStoreManagement.IBLL;
using ChicStoreManagement.WEB.ViewModel;

namespace ChicStoreManagement.Controllers
{
    [AllowAnonymous]
    public class TestController : Controller

    {
        private readonly IProductCodeBLL productCodeBLL;
        private readonly IExceptedBuyBLL exceptedBuyBLL;
        private IQueryable<CustomerExceptedBuyModel> exceptedBuyModels;

        public TestController(IExceptedBuyBLL exceptedBuyBLL)
        {
            this.exceptedBuyBLL = exceptedBuyBLL;
        }

        public ActionResult Index()
        {
            BuildExceptedBuy();
            var list = exceptedBuyModels.ToList();
            return View(list);
        }
        public ActionResult Add(CustomerExceptedBuyModel staff)
        {
        //    if (exceptedBuyBLL.Add(staff)!=0)
        //    {
        //        return Redirect("Index");
        //    }
        //    else
        //    {
                return Content("no");
        //    }
        }
        public ActionResult Update(CustomerExceptedBuyModel staff)
        {
            //var Name= Request["姓名"];
            //var Sex = Request["性别"];
            //string[] property={ Name, Sex };
            //if (exceptedBuyBLL.Modify(staff,property)!=0)
            //{
            //    return Redirect("Index");
            //}
            //else
            //{
               return Content("no");
            //}
        }
        public ActionResult Delete(int Id)
        {
            //var staff = exceptedBuyBLL.GetModels(p => p.ID== Id).FirstOrDefault();
            //if (exceptedBuyBLL.Del(staff)!=0)
            //{
            //    return Redirect("Index");
            //}
            //else
            //{
              return Content("no");
            //}
        }
        public ActionResult ExceptedBuyViewAction(int id) {

            BuildExceptedBuy(id);
            var model = exceptedBuyModels.ToList();
            return PartialView("ExceptedBuy_PartialPage", model);
        }

        public ActionResult DelExceptedBuyAction(int id)
        {

            BuildExceptedBuy(id);
            var model = exceptedBuyModels.ToList();
            return PartialView("ExceptedBuy_PartialPage", model);
        }
        private void BuildExceptedBuy()
        {
            
            List<CustomerExceptedBuyModel> models = new List<CustomerExceptedBuyModel>();
            var exceptedBuy = exceptedBuyBLL.GetListBy(p => true);
            if (exceptedBuy != null)
            {
                foreach (var item in exceptedBuy)
                {
                    CustomerExceptedBuyModel exceptedBuyModel = new CustomerExceptedBuyModel();
                    exceptedBuyModel.商品型号 = productCodeBLL.GetModel(p => p.ID == item.商品型号ID).型号;
                    exceptedBuyModel.备注 = item.备注;
                    models.Add(exceptedBuyModel);
                }
                exceptedBuyModels = models.AsEnumerable().AsQueryable();
            }

    }

        private void BuildExceptedBuy(int id)
        {
            if (id == 0)
            {
                return;
            }
            List<CustomerExceptedBuyModel> models = new List<CustomerExceptedBuyModel>();
            var exceptedBuy = exceptedBuyBLL.GetListBy(p => p.接待ID == id);
            if (exceptedBuy != null)
            {
                foreach (var item in exceptedBuy)
                {
                    CustomerExceptedBuyModel exceptedBuyModel = new CustomerExceptedBuyModel();
                    exceptedBuyModel.商品型号 = productCodeBLL.GetModel(p => p.ID == item.商品型号ID).型号;
                    exceptedBuyModel.备注 = item.备注;
                    models.Add(exceptedBuyModel);
                }
                exceptedBuyModels = models.AsEnumerable().AsQueryable();
            }


        }
    }
}

