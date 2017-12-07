using CMS.EntityFrameWork.Context;
using CMS.EntityFrameWork.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UploadImage.Models;

namespace UploadImage.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewAll() {
            using (UploadImageContext db = new UploadImageContext())
            {
                List<imageModel> imgList = db.images
                    .Select(x => new imageModel
                    { 
                        Id = x.Id,
                        Name = x.Name,
                        ImagePath = x.ImagePath
                    }).ToList();
                return Json(new { data = imgList }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new imageModel());
            }
            else
            {
                using (UploadImageContext db = new UploadImageContext())
                {
                    return View(db.images.Where(x => x.Id == id).FirstOrDefault<image>());
                }
            }
        }
        [HttpPost]
        public ActionResult AddOrEdit(imageModel img)
        {
            if (img.ImageUpload != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(img.ImageUpload.FileName);
                string extension = Path.GetExtension(img.ImageUpload.FileName);
                fileName = img.Name + extension;
                img.ImagePath = "/File/image/" + fileName;
                img.ImageUpload.SaveAs(Path.Combine(Server.MapPath("/File/image/"), fileName));
            }
            using (UploadImageContext db = new UploadImageContext())
            {
                if (img.Id == 0)
                {
                    image obj = new image();
                    obj.ImagePath = img.ImagePath;
                    obj.Name = img.Name;
                    db.images.Add(obj);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved successfully!!!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(img).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated successfully!!!" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (UploadImageContext db = new UploadImageContext())
            {
                image img = db.images.Where(x => x.Id == id).FirstOrDefault<image>();
                db.images.Remove(img);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted successfully!!!" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}