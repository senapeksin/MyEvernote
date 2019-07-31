using MyEvernote.BusinessLayer;
using MyEvernote.BusinessLayer.Result;
using MyEvernote.Entities;
using MyEvernote.Entities.Messages;
using MyEvernote.Entities.ValueObjects;
using MyEvernote.WepApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.WepApp.Controllers
{

    public class HomeController : Controller
    {
        private NoteManager noteManager = new NoteManager();
        private CategoryManager categoryManager = new CategoryManager();
        private EvernoteUserManager evernoteUserManager = new EvernoteUserManager();



        // GET: Home
        public ActionResult Index()
        {
            //database testleri videodaki adam test i sildi .
            //BusinessLayer.Test test = new BusinessLayer.Test();
            //test.InsertTest();
            //test.UpdateTest();
            //test.DeleteTest();
            //test.CommentTest();
            //**************************************************************
            //CategoryController üzerinden gelen view talebi ve model...
            if (TempData["mm"] != null)   //eğer mm isimli tempdata içinde null değilse   
            {
                return View(TempData["mm"] as List<Note>);  //tempdata içerisindeki notları al model olarak kullan
            }
            //tempdata boşsa zataen normal bir listeleme yöntemine girer.
            return View(noteManager.List().OrderByDescending(x => x.ModifiedOn).ToList());  //notları getallnotes metodundan büyükten kücüge dopru sıralayıp gönderecek
        }
        public ActionResult ByCategory(int? id)  //id boş geçilerek de select çağırılabilir.
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); // 444 error sayfa bulunamadı ! 
            }
            Category cat = categoryManager.Find(x=>x.Id==id.Value);  //id verip  kategoriyi alacağım . id int alabilen bir metod yazdık tip uyuşmazlığı olmasın diye value yazdık.

            if (cat == null)                             //kategori yoksa
            {
                return HttpNotFound();                   //bulunamadı hatası

            }
            return View("Index", cat.Notes.OrderByDescending(x => x.ModifiedOn).ToList());  //bu modeli ındex view inda görüntüle.
         
        }
        public ActionResult MostLiked()
        {
            return View("Index", noteManager.List().OrderByDescending(x => x.LıkeCount).ToList());  //like sayısını büyükten kücüğe göre sıralaycağız.
        }
        public ActionResult About()
        {
            return View();
        }

        public ActionResult ShowProfile()
        {
            EvernoteUser currentUser = Session["login"] as EvernoteUser;
            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.GetUserById(currentUser.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Olustu ",
                    Items = res.Errors
                };
                return View("Error", errorNotifyObj);
            }
            return View(res.Result);
        }
        public ActionResult EditProfile()
        {
            EvernoteUser currentUser = Session["login"] as EvernoteUser;

            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.GetUserById(currentUser.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Olustu ",
                    Items = res.Errors
                };
                return View("Error", errorNotifyObj);
            }
            return View(res.Result);
        }

        [HttpPost]
        public ActionResult EditProfile(EvernoteUser model, HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUsurname");
            if (ModelState.IsValid)
            {
            if (ProfileImage != null &&
                (ProfileImage.ContentType == "image/jpeg" ||
                ProfileImage.ContentType == "image/jpg" ||
                ProfileImage.ContentType == "image/png"))
            {
                string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";

                ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                model.ProfileImageFileName = filename;
            }
            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.UpdateProfile(model);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = res.Errors,
                    Title = "Profil Güncellenemedi",
                    RedirectingUrl="/Home/EditProfile"
                };
                return View("Error", errorNotifyObj);
            }
            //update başarılı olursa;
            Session["login"] = res.Result; //profil güncellendiği için Session da güncellendi.
            return RedirectToAction("ShowProfile");
            }
            return View(model);
        }
        public ActionResult DeleteProfile()
        {
            EvernoteUser currentUser = Session["login"] as EvernoteUser;

            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.RemoveUserById(currentUser.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = res.Errors,
                    Title = "Profil Silinemedi",
                    RedirectingUrl = "/Home/ShowProfile"
                };
                return View("Error", errorNotifyObj);
            }
            Session.Clear();
            return RedirectToAction("Index");
        }
  

        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<EvernoteUser> res = evernoteUserManager.LoginUser(model);

                if (res.Errors.Count > 0)
                {
                    if (res.Errors.Find(x => x.Code == ErrorMessageCode.UserIsNotAktive) != null)
                    {
                        ViewBag.SetLink = "http://Home/Activate/1234-4567-78980";
                    }
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));




                    return View(model);
                }
                Session["login"] = res.Result;     // session'a kullanıcı bilgi saklama
                return RedirectToAction("Index"); //yönlendirme..
            }

            return View(model);
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

                BusinessLayerResult<EvernoteUser> res = evernoteUserManager.RegisterUser(model);
                if (res.Errors.Count > 0)  //0 sa hata var demektir.
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message)); //tüm errorlist listesini foreach le dön her veri için ilgili stringi modelerror a ekle!
                    return View(model);
                }
                OkViewModel notifyObj = new OkViewModel()
                {

                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/Login"
                };
                notifyObj.Items.Add("Lütfen E-posta adresinize gönderdiğimiz aktivasyon link'ine tıklayarak hesabınızı aktive ediniz.Hesabınızı aktive etmeden not ekleyemez ve beğeni yapamazsınız");

                return View("Ok", notifyObj);
            }
            return View(model);
        }


        public ActionResult UserActivate(Guid id)
        {
            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.ActivateUser(id); // evernoteusermanager daki activateuser a gelen aktivate id yi göndermemiz gerekiyor.

            if (res.Errors.Count > 0) //0dan büyükse hata var demektir.
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Geçersiz İşlem ",
                    Items = res.Errors

                };

                return View("Error", errorNotifyObj);
            }
            OkViewModel okNotifyObj = new OkViewModel()
            {
                Title = "Hesap Aktifleştirildi.",
                RedirectingUrl = "/Home/Login",
            };
            okNotifyObj.Items.Add("Hesabınız aktifleştirildi. Artık not paylaşabilir ve beğenme yapabilirsiniz..    ");
            return View("Ok", okNotifyObj);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}