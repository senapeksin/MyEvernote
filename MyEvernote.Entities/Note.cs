using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities
{
    public class Note:MyEntityBase
    {
        [Required, StringLength(60)]
        public string Title { get; set; }
        [Required, StringLength(2000)]
        public string Text { get; set; }
        public bool IsIDraft { get; set; }   //taslak  mı?
        public int LıkeCount { get; set; }  //likelanacak notlar
         public int CategoryId { get; set; }


        

        //bir notun bir user ı vardır birkişi olusturur.
        public EvernoteUser Owner { get; set; }  //user tipinde bir sahibi owner vardır
        //bir notun yormları vardıdr birden cok
        public virtual List<Comment> Comments { get; set; }
        //her notun bir kategorisi vardır
        public virtual Category Category { get; set; }
        //bir notun  birden cok like ı vardır 

        public virtual  List<Liked>  Likes { get; set; }
        

            
        public Note()
        {
            Comments = new List<Comment>();
            Likes = new List<Liked>();

        }




    }
}
