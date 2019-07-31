using MyEvernote.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    //singleton sınıf
    public class RepositoryBase
    {
        protected static DatabaseContext db;
        private static object _lockSync = new object();
        protected RepositoryBase()
        {
            CreateContext();
        }
        private static void CreateContext()
        {
            if (db == null)
            {
                lock (_lockSync)  //aynı anda iiki threadin çalısmayacağını söyler.
                {
                    if (db == null)
                    {
                        db = new DatabaseContext();
                    }
                }
            }
        }
    }
}
