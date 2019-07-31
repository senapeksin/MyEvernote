using MyEvernote.BusinessLayer.Abstract;
using MyEvernote.BusinessLayer.Result;
using MyEvernote.Common.Helpers;
using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
using MyEvernote.Entities.Messages;
using MyEvernote.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
    public class EvernoteUserManager: ManagerBase<EvernoteUser>
    {
      
        public BusinessLayerResult<EvernoteUser> RegisterUser(RegisterViewModel data)
        {
            //kullanıcı  usurname kontrolü
            //kullnaıcı eposta kontrolü
            //kayıt işlemi
            //aktivasyon epostası gönderimi yapılacak
            ///////////////////////////////////////////////////////////////////////////////

            EvernoteUser user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsurnameAlreadyExist, "kullanıcı adı kayıtlı");
                }
                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, "e-posta adresiniz kayıtlı");
                }
            }
            else
            {
                int dbResult = Insert(new EvernoteUser()
                {
                    Username = data.Username,
                    Email = data.Email,
                    ProfileImageFileName = "user_boy.png",
                    password = data.Password,
                    ActiveGuid = Guid.NewGuid(),
                    IsActive = false,
                    IsAdmin = false,
                   // ModifiedUsername = "system"
                });
                if (dbResult > 0)
                {
                    res.Result = Find(x => x.Email == data.Email && x.Username == data.Username);
                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/UserActivate/{res.Result.ActiveGuid}";
                    string body = $"Merhaba {res.Result.Name} {res.Result.Surname};<br><br>Hesabınızı aktifleştirmek için <a href='{activateUri}' target='blank'>tıklayınız.</a>";
                    MailHelper.SendMail(body, res.Result.Email, "MyEvernote Hesap Aktifleştirme");
                }
            }
            return res;
        }
        public BusinessLayerResult<EvernoteUser> LoginUser(LoginViewModel data)
        {
            //giriş kontrolü
            //hesap aktif edilmi mi?
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            res.Result = Find(x => x.Username == data.Username && x.password == data.Password);  //kullanıcı adı ve şifre eşleşiyor mu ?
          
          
            if (res.Result != null) //user eşleşmişse
            {
                if (!res.Result.IsActive) //kulanncı aktif değilse
                {
                    res.AddError(ErrorMessageCode.UserIsNotAktive,"Kullanıcı aktifleştirilmemiştir.");
                    res.AddError(ErrorMessageCode.CheckYourEmail, "Lütfen e-posta adresinizi kontrol ediniz.");
                }

            }
            else
            {
                res.AddError(ErrorMessageCode.UsurnameorPassWrong, "Kullanıcı adı yada şifre uyuşmuyor.");
            }
            return res;
        }

        public BusinessLayerResult<EvernoteUser> GetUserById(int id)
        {
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            res.Result = Find(x=>x.Id==id);
            if (res.Result==null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı bulunamadı");
            }
            return res;
        }

        public BusinessLayerResult<EvernoteUser> ActivateUser(Guid activateId)
        {
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            res.Result = Find(x => x.ActiveGuid== activateId);

            if (res.Result!=null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive,"Kullanıcı zaten aktif edilmiştir.");
                    return res;
                }
                res.Result.IsActive = true;
                Update(res.Result); //kullanıcıyı aktifleştir.
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExist, "Aktifleştirilecek kullanıcı bulunamadı.");

            }
            return res;
        }

        public BusinessLayerResult<EvernoteUser> UpdateProfile(EvernoteUser data)
        {

            EvernoteUser db_user = Find(x => x.Id != data.Id && (x.Username == data.Username || x.Email == data.Email));

            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();

            if (db_user != null && db_user.Id != data.Id) 
            {
                if (db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsurnameAlreadyExist, "kullanıcı adı kayıtlı");
                }
                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, "e-posta adresiniz kayıtlı");
                }
                return res;
            }
            res.Result = Find(x=>x.Id==data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.password = data.password;
            res.Result.Username = data.Username;

            if (string.IsNullOrEmpty(data.ProfileImageFileName)==false)
            {
                res.Result.ProfileImageFileName = data.ProfileImageFileName;
            }
            if (Update(res.Result)==0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Profil Güncellenemedi!");
            }
            return res;
        }

        public BusinessLayerResult<EvernoteUser> RemoveUserById(int id)
        {
            BusinessLayerResult<EvernoteUser> res = new BusinessLayerResult<EvernoteUser>();
            EvernoteUser user = Find(x => x.Id==id);

            if (user!=null)
            {
                if (Delete(user)==0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı silinemedi..");
                    return res;
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı bulunamadı..");

            }
            return res;

        }
    }
}
