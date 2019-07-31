using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using MyEvernote.Entities;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    public class MyInitializer : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)  //database olustuktan sonra örnek database  kullanılan metod  
        {

            //adding admin user ..
            EvernoteUser admin = new EvernoteUser()
            {
                Name = "Sena",
                Surname = "Pekşin",
                Email = "*******,
                ActiveGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                Username = "senapeksin",
                ProfileImageFileName="user_boy.png",
                password = "123456",
                CreatedOn = DateTime.Now,  //su an olusturuldu
                ModifiedOn = DateTime.Now.AddMinutes(5),  //su andan 5 dk sonra güncelllendi
                ModifiedUsername = "senapeksin" //ben güncelledim
            };

            //adding standart user..
            EvernoteUser standartUser = new EvernoteUser()
            {
                Name = "Hilal",
                Surname = "Pekşin",
                Email = "*******",
                ActiveGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                Username = "hilalpeksin",
                password = "654321",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "senapeksin" //ben güncelledim
            };
            context.EvernoteUsers.Add(admin);
            context.EvernoteUsers.Add(standartUser);



            // 8 tane random fake kullanıcı ekleme
            for (int i = 0; i < 8; i++)
            {
                EvernoteUser user = new EvernoteUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    ProfileImageFileName = "user_boy.png",
                    ActiveGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    Username = $"user{i}",
                    password = "123",
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUsername = $"user{i}"    //ben güncelledim
                };
                context.EvernoteUsers.Add(user);
            }
            context.SaveChanges();

            //user list for 
            List<EvernoteUser> userlist = context.EvernoteUsers.ToList();


            //FAKE KATEGORI eklemek..
            for (int i = 0; i < 10; i++)
            {
                Category cat = new Category()
                {
                    Title = FakeData.PlaceData.GetStreetName(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedUsername = "senapeksin"
                };

                context.Categories.Add(cat);   //!!!!!!!!!!!!!!!!!!!!!!!!!!!buradan hata gelebilir video da burası CATEGORİES di cünkü!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                //adding fake notes not ekleme..
                for (int k = 0; k < FakeData.NumberData.GetNumber(5, 9); k++)  //5 ile 9 arasında bir not olusturacak
                {
                    EvernoteUser owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];
                    Note note = new Note()  //notu oluusturdu
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        IsIDraft = false,  //taslak değil
                        LıkeCount = FakeData.NumberData.GetNumber(1, 9),
                        Owner = owner,   //diziden random bir şekilde kullanıcı çekiyoruz.
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUsername =owner.Username,
                        
                    };
                    cat.Notes.Add(note); //notu notes ın özelliğine eklemek gerekecek.   notes null gelebeilir category sayfasına constructor olustur ve ..

                    //adding     fake comments..
                    for (int l = 0; l < FakeData.NumberData.GetNumber(3, 5); l++)   // 3 ile 5 arasında comment belirlesin her bir not için 
                    {
                        EvernoteUser comment_owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];
                        Comment comment = new Comment()
                        {
                            Text = FakeData.TextData.GetSentence(),

                            Owner = comment_owner,
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedUsername = comment_owner.Username  //sahini kimse o modified etmiş olsun

                        };
                        note.Comments.Add(comment);
                    }
                    //adding fake likes..



                    for (int m = 0; m <note.LıkeCount ; m++)
                    {
                        Liked liked = new Liked()
                        {
                            LikedUser =userlist[m]
                        };
                        note.Likes.Add(liked);
                    }


                }
            }

            context.SaveChanges();

        }

    }
}
