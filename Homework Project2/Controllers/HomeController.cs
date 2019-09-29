using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Homework_Project2.Models;

namespace Homework_Project2.Controllers
{
    public class HomeController : Controller
    {
        //ShoppingCarEntities1 db = new ShoppingCarEntities1();
        ShoppingCarsEntities db = new ShoppingCarsEntities();
        // GET: Home
        public ActionResult Index()
        {
            var query = from o in db.Products
                        select o;
            var products = query.ToList();
            if (Session["User"] == null)
            {
                return View("Index", "_Layout", products);
            }
            return View("Index", "_LayoutUser", products);
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string UserId, string Pwd)
        {
            var query = from o in db.Users
                        where (UserId == o.UserId && Pwd == o.Pwd)
                        select o;
            var User = query.FirstOrDefault();
            if (User == null)
            {
                ViewBag.Message = "帳密錯誤，登入失敗";
                return View();
            }
            Session["Welcome"] = User.Name + "歡迎光臨";
            Session["User"] = User;

            return RedirectToAction("Index");
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(User NewUser)
        {
            if (ModelState.IsValid == false)
            {

                return View();
            }
            var query = from o in db.Users
                        where (o.UserId == NewUser.UserId)
                        select o;
            var Person = query.FirstOrDefault();
            if (Person == null)
            {
                db.Users.Add(NewUser);
                db.SaveChanges();

                return RedirectToAction("Login");
            }
            ViewBag.Message = "此帳號已有人使用，註冊失敗";
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
        public ActionResult shoppingCar()
        {
            if (Session["User"] == null)
            {
                return View("Login");
            }
            string UserId = (Session["User"] as User).UserId;
            var query = from o in db.Details
                        where (o.UserId == UserId && o.IsApproved == "否")
                        select o;
            var detail = query.ToList();
            return View("shoppingCar", "_LayoutUser", detail);
        }
        [HttpPost]
        public ActionResult shoppingCar(string Receiver, string Email, string Address)
        {
            string UserId = (Session["User"] as User).UserId;
            string guid = Guid.NewGuid().ToString();
            Order order = new Order();
            order.OrderGuid = guid;
            order.UserId = UserId;
            order.Receiver = Receiver;
            order.Email = Email;
            order.Address = Address;
            order.Date = DateTime.Now;
            db.Orders.Add(order);

            var query = from o in db.Details
                        where (o.IsApproved == "否" && o.UserId == UserId)
                        select o;
            var carList = query.ToList();
            foreach (var item in carList)
            {
                item.IsApproved = "是";
                item.OrderGuid = guid;
            }
            db.SaveChanges();
            return RedirectToAction("OrderList");
        }
        public ActionResult OrderList()
        {
            if (Session["User"]==null)
            {
                return View("Login");
            }
            string UserId = (Session["User"] as User).UserId;
            var query = from o in db.Orders
                        where o.UserId == UserId
                        orderby o.Date descending
                        select o;
            var order = query.ToList();
            return View("OrderList", "_LayoutUser", order);
        }
        public ActionResult AddCar(string PId)
        {
            if (Session["User"]==null)
            {
                return RedirectToAction("Login");
            }
            string UserId = (Session["User"] as User).UserId;
            var query = from o in db.Details
                        where (UserId == o.UserId && PId == o.PId && o.IsApproved == "否")
                        select o;
            var currentCar = query.FirstOrDefault();
            if (currentCar == null)
            {
                var query2 = from o in db.Products
                             where o.PId == PId
                             select o;
                var product = query2.FirstOrDefault();
                Detail orderDetail = new Detail();
                orderDetail.UserId = UserId;
                orderDetail.PId = product.PId;
                orderDetail.Name = product.Name;
                orderDetail.Price = product.Price;
                orderDetail.Qty = 1;
                orderDetail.IsApproved = "否";
                db.Details.Add(orderDetail);
            }
            else
            {
                currentCar.Qty += 1;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteCar(int Id)
        {
            var query = from o in db.Details
                        where o.Id == Id
                        select o;
            var ProductInCar = query.FirstOrDefault();
            db.Details.Remove(ProductInCar);
            db.SaveChanges();
            return RedirectToAction("shoppingCar");
        }
        public ActionResult OrderDetail(string OrderGuid)
        {
            if (Session["User"]==null)
            {
                return View("Login");
            }
            var query = from o in db.Details
                        where OrderGuid == o.OrderGuid
                        select o;
            var OrderDetail = query.ToList();
            return View("OrderDetail", "_LayoutUser", OrderDetail);
        }
    }
}