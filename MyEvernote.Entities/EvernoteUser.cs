using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities
{
    [Table("EvernoteUsers")]
    public class EvernoteUser : MyEntityBase  //id buradan gelsin
    {
        [DisplayName("İsim"), StringLength(25,
            ErrorMessage = "{0} alanı max.{1} karakter olmalıdır..")]
        public string Name { get; set; }

        [DisplayName("Soyad"), 
            StringLength(25, ErrorMessage = "{0} alanı max.{1} karakter olmalıdır..")]
        public string Surname { get; set; }


        [DisplayName("Kullanıcı Adı"), 
            Required(ErrorMessage ="{0} alanı gereklidir."), StringLength(25, ErrorMessage = "{0} alanı max.{1} karakter olmalıdır..")]
        public string Username { get; set; }


        [DisplayName("E-posta"), Required(ErrorMessage = "{0} alanı gereklidir."), 
            StringLength(70, ErrorMessage = "{0} alanı max.{1} karakter olmalıdır..")]
        public string Email { get; set; }


        [DisplayName("Şİfre"), Required(ErrorMessage = "{0} alanı gereklidir."), 
            StringLength(25, ErrorMessage = "{0} alanı max.{1} karakter olmalıdır..")]
        public string password { get; set; }

        [StringLength(30)]//  images/user_12.jpg 
        public string ProfileImageFileName { get; set; }

        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }

        [Required]
        public Guid ActiveGuid { get; set; }




        //bir kullanıcının  birden fazla notu var (kendi olusturduğu likeladığı değil )
        public virtual List<Note> Notes { get; set; } 
        public virtual List<Comment> Comments { get; set; }
        //bir kullanıcının birden cok like ı vardır
         public  virtual  List<Liked> Likes { get; set; }

    }
}

