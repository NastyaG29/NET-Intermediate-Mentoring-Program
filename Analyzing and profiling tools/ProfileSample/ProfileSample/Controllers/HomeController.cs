using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProfileSample.DAL;
using ProfileSample.Models;

namespace ProfileSample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var imageModels = new List<ImageModel>();

            using (var context = new ProfileSampleEntities()) 
                imageModels = context.ImgSources.Take(20).Select(s => new ImageModel {Name = s.Name, Url = s.Url})
                    .ToList();

            return View(imageModels);
        }

        public ActionResult Convert()
        {
            var files = Directory.GetFiles(Server.MapPath("~/Content/Img"), "*.jpg");

            using (var context = new ProfileSampleEntities())
            {
                foreach (var file in files)
                {
                    var imageSource = new ImgSource()
                    {
                        Name = Path.GetFileName(file),
                        Url = file
                    };

                    context.ImgSources.Add(imageSource);
                    context.SaveChanges();
                } 
            }

            return RedirectToAction("Index");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}