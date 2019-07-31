using MyEvernote.Common;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEvernote.WepApp.Init
{
    public class WebCommon : ICommon
    {
        public string GetCurrentUsurname()
        {
            if (HttpContext.Current.Session["login"] != null)  //sessionda login isimli session nesne item var mı ?
            {
                EvernoteUser user = HttpContext.Current.Session["login"] as EvernoteUser; //varsa bunu evernote user nesnesine çevir..
                return user.Username;// bu userin username ini döndürüyor olacağım .
            }
            //if (HttpContext.Current.Session["system"] == null)
            //{
            //    EvernoteUser ModifiedUsurname = HttpContext.Current.Session["system"] as EvernoteUser;
            //    return "system";
            //}
            return "system"; // eğer yoksa usurname system döndür.
        }
    }
}