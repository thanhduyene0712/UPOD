using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPOD.REPOSITORIES.ResponeModels
{
    public class ObjectModelResponse
    {
        public ObjectModelResponse(object data)
        {
            Message = "Successfull";
            Status = 200;
            Data = data;
        }

        public string Message { get; set; }
        public int Status { get; set; }
        public string Type { get; set; }
        public object Data { get; set; }

    }
}
