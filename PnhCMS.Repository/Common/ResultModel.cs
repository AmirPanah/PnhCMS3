using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnhCMS.Repository.Common
{
    public class ResultModel
    {
        public string Id { get; set; }
        public bool Result { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }
    }

}
