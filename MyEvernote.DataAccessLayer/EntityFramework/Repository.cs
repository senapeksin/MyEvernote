
using MyEvernote.Common;
using MyEvernote.Core.DataAccess;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    public class Repository<T> : RepositoryBase, IDataAccess<T> where T : class
    {
        private DbSet<T> _objectSet;

        public object HttpContext { get; private set; }

        public Repository()    //sürekli db.set<t> bulmak yeirne bunu bir nesneye atarım bir kez dbset bulur ve devam eder.
        {
            // db = RepositoryBase.CreateContext(); SİLDİK =>database context i bir metot üzerinden alıyorum.
            _objectSet = db.Set<T>();
        }
        public List<T> List()  //list metodu geriye o tablonun  tüm kayıtlarını döndürecek 
        {
            return _objectSet.ToList();  //gelen tip ne ise onun set ini bul ve onun ona to list uygula ve dönen listi de geri döndür .
        }

        //bir de tüm tabloyu değilde istediğim kritere göre listeleyen metot olsun.
        public List<T> List(Expression<Func<T, bool>> where)     //where sorgusnu otomatikleştirmiş olduk ve where parametresine atadık.
        {
            return _objectSet.Where(where).ToList();
        }
        public int Insert(T obj)
        {
            _objectSet.Add(obj);   //tabloyu bulmamız lazım add ile obj veriyoruz ardından save metodunu çağırıyoruz.Tipi bilmiyorsak set metodu ile bulup addliyoruz biliyorsak direk yazıyoruz . 
            if (obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase;
                DateTime now = DateTime.Now;

                o.CreatedOn = now;
                o.ModifiedOn = now;
               
                //int x = 5;
                //var isim = x == 5 ? "suat" : "sena"; 

                //if(o.ModifiedUsername=="system")                                                                     //system yazıp yazmadıgını kontrol et
                //{                                                                                                     //yazmıyorsa
                //    EvernoteUser ModifiedUsurname = Session["login"] as EvernoteUser;           //atama yap                 
                //}


                o.ModifiedUsername = App.Common.GetCurrentUsurname();
            }
            return Save();
        }
        public int Update(T obj)       //update de sadece save çağıırırız.
        {
            if (obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase;

                o.ModifiedOn = DateTime.Now;
                o.ModifiedUsername = App.Common.GetCurrentUsurname();
            }
            return Save();
        }
        public int Delete(T obj)
        {
           
            _objectSet.Remove(obj); //obj setinden remove ile ilgili seti sil.
            return Save();
        }
        public int Save()
        {
            try
            {
                // Your code...
                // Could also be before try if you know the exception occurs in SaveChanges

                return db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
           
        }
        public T Find(Expression<Func<T, bool>> where)      //geriye TEK bir tür döndürüyorum liste döndürmüyorum bulabilirse nesneyi döner bulamazsa null döner
        {
            return _objectSet.FirstOrDefault(where);
        }
    }
}