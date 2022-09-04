using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.ResponeModels
{
    public class ResponseModel<T>
    {
        public ResponseModel(ICollection<T> data)
        {
            Data = data;
            Message = "Successfull";
            Status = 200;
        }
        public string Message { get; set; }
        public int Status { get; set; }
        public string Type { get; set; }
        public int Total { get; set; }
        public ICollection<T> Data { get; set; }
    }
}
