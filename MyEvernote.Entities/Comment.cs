﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities
{
    [Table("Comments")]
   public  class Comment:MyEntityBase
    {
        [Required,StringLength(300)]
        public string Text { get; set; }
        //HANGİ NOTA YORUM YAZDIN?
        public virtual Note Note { get; set; }

        //hangi kullanıcı bu yorumu yazdı
        public virtual EvernoteUser Owner { get; set; }

       
    }
}
