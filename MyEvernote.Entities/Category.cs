using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace MyEvernote.Entities
{
    [Table("Categories")]
    public class Category : MyEntityBase
    {
        [DisplayName("Başlık"), Required(ErrorMessage ="{0} alanı gereklidir."),
            StringLength(50,ErrorMessage ="{0} alanı max {1} karakter olmalıdır.")]
        public string Title { get; set; }



        [DisplayName("Başlık"), Required(ErrorMessage = "{0} alanı gereklidir."),
             StringLength(150, ErrorMessage = "{0} alanı max {1} karakter olmalıdır.")]
        public string Description { get; set; } //açıklama

        //kategorinin notları vardır. birden fazla not olabileceği iççin List yapısı kullanıyorum . ilişkisi olacağı için => virtual olmalı.

        public virtual List<Note> Notes { get; set; }

        public Category()  // notlarının otomatik olarak listenin olusmasın ı sağlıyoruz ki not olusturmak istediğimizde NULL HATASI almayalım..!
        {
            Notes = new List<Note>();
        }

    }
}
