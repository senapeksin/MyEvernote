using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
    public class Test
    {
        private Repository<EvernoteUser> repo_user = new Repository<EvernoteUser>();
        private Repository<Category> repo_category = new Repository<Category>();
        private Repository<Comment> repo_comment = new Repository<Comment>();
        private Repository<Note> repo_note = new Repository<Note>();
        public Test()
        {

            List<Category> categories = repo_category.List();
        }
        public void InsertTest()
        {
            int result = repo_user.Insert(new EvernoteUser()
            {
                Name = "hiloş",
                Surname = "aaa",
                Email = "aaa@gmail.com",
                ActiveGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                Username = "aaahiloş",
                password = "123",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "aaahiloş"
            });
        }
        public void UpdateTest()
        {
            EvernoteUser user = repo_user.Find(x => x.Username == "aaahiloş");
            if (user != null)
            {
                user.Username = "xxx";
                int result = repo_user.Update(user);
            }
        }
        public void DeleteTest()
        {
            EvernoteUser user = repo_user.Find(x => x.Username == "xxx");  //ilk önce user i bul.

            if (user != null)
            {
                int result = repo_user.Delete(user);
            }

        }

        //ilişkili tablolarda crud işlemleri nasıl olacak???

        //mesela comment eklemek isteyeceğim zaman , şu commenti şu nota insert edeceksin ve şu kullanıcının commenti bu diyebilmem gerekiyor...Çünkü yorum not ile İLİŞKİLİYDİ!
        //hangi  notun yorumu? ve kim bu yorumu bıraktı??? sorularım bunlar. Dolayısıyla ilk önce bir comment nesnesi olmalı.
        public void CommentTest()
        {
            EvernoteUser user = repo_user.Find(x => x.Id == 1);  // id si 1 olan kullanıcıyı bul ve o yorum yapmış olsun
            Note note = repo_note.Find(x => x.Id == 3);             //id si 3 olan nota at. (yorumu)

            Comment comment = new Comment()
            {
                Text = "bu bir yorum testidir.",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedUsername = "senapeksin",
                //hangi nota ekleyeceksin yorummu?
                Note = note,
                //ve kim ekliyor?
                Owner = user
            };
            repo_comment.Insert(comment);
        }



    }
}
