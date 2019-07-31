using MyEvernote.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer.Result
{
    public class BusinessLayerResult<T> where T :class
    {
        public List<ErrorMessageObj> Errors { get; set; }   //hata mesajarını burada
        public T Result { get; set; }   //eğerki başarılıysa hata yoksada sonucu veriyor olmak.



        public BusinessLayerResult()
        {
            Errors = new List<ErrorMessageObj>();  //bir list olustaralı ki içine ekleyebilecek halde olsun 
         
        }
        public void AddError(ErrorMessageCode code,string message)
        {
            Errors.Add(new ErrorMessageObj() { Code = code, Message = message });
        }
    }
}
