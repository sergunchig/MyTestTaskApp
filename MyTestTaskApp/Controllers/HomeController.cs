using MyTestTaskApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyTestTaskApp.Controllers
{
    public class HomeController : Controller
    {
        TestdatabaseEntities db = new TestdatabaseEntities();
        public ActionResult Index(string filters)
        {
            ViewBag.Title = "Home Page";
            string filtersList = string.Empty;

            if (filters == null) {
                ViewBag.Orders = db.Order;
            }
            else
            {
                List<Order> list = new List<Order>();
                string[] filtersArray = filters.TrimStart('|').Split('|');
                foreach (var ai in filtersArray)
                {
                    int providerId = db.Provider.First(p => p.Name.Equals(ai)).id;
                    var orders = db.Order.Where(p => p.ProviderId.Equals(providerId));
                    list.AddRange(orders);
                    filtersList += ai + ";";
                }
                ViewBag.Orders = list;
            }
            ViewBag.addedFilters = filtersList;
            var ovl = db.Provider;

            return View(ovl);
        }
        [HttpGet]
        public ActionResult OrderInformation(string order)
        {
            ViewBag.Title = order;
            ViewBag.Providers = db.Provider.ToList();

            var orderInfo = db.Order.Where(o => o.Number.Equals(order)).First();
            //ViewBag.Order = orderInfo;
            return View(orderInfo);
        }
        [HttpPost]
        public ActionResult OrderInformation()
        {
            string date = Request.Form["Date"];
            string oldNum = Request.Form["OldNum"];
            string number = Request.Form["Number"];
            string ProviderName = Request.Form["ProviderName"];

            int providerId = db.Provider.First(p => p.Name.Equals(ProviderName)).id;
            Order order = db.Order.First(o => o.Number.Equals(oldNum));

            order.Number = number;
            order.ProviderId = providerId;
            order.Date = Convert.ToDateTime(date);

            db.Entry(order).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult AddOrder()
        {
            IEnumerable<Provider> providers = db.Provider.AsEnumerable();
            return View(providers);
        }
        [HttpPost]
        public ActionResult AddOrder([Bind(Include = "Number")] Order order)
        {
            string providerName = Request.Form["ProviderName"];
            Provider provider = db.Provider.First(p => p.Name.Equals(providerName));
            order.ProviderId = provider.id;

            db.Order.Add(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult AddOrderItem(int orderId)
        {
            ViewBag.orderId = orderId;
            return View();
        }
        [HttpPost]
        public ActionResult AddOrderItem(OrderItem orderItem)
        {
            db.OrderItem.Add(orderItem);
            db.SaveChanges();
            string ordernum = db.Order.First(o => o.id == orderItem.OrderId).Number;
            return RedirectToAction("OrderInformation", "Home", new { order = ordernum });
        }
        public ActionResult DeleteItem(int item, int order)
        {
            string orderNum = db.Order.First(o=> o.id == order).Number;

            db.OrderItem.Remove(db.OrderItem.First(oi=> oi.id == item));
            db.SaveChanges();

            return RedirectToAction("OrderInformation", "Home", new { order = orderNum });
        }

        public ActionResult EditOrderItem(int item)
        {
            OrderItem orderitem = db.OrderItem.First(oi => oi.id == item);
            return View(orderitem);
        }
        [HttpPost]
        public ActionResult EditOrderItem(OrderItem orderItem)
        {
            string orderNum = db.Order.First(o => o.id == orderItem.OrderId).Number;

            db.Entry(orderItem).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("OrderInformation", "Home", new { order = orderNum });
        }
    }
}
