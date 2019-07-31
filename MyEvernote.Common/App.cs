using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//dışarıdan kullanılacak class bu olacak 
namespace MyEvernote.Common
{
   public static class App
    {
        public static ICommon Common = new DefaultCommon();  //app classın common  fieldinda , değişkeninde  "new defaultcommon" nesneden bir tane new yapıp verdim.Artık su an bu nesne ile çalışıyorum. bu sınıf içerisinde de ICommondan geldiği için bu sınıf  getusurname metodunu implement ettik ve erişebildik. 
    }
}
