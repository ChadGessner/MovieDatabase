using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApiHandler
{
    [Serializable]
    public class ResponseModel
    {
        public int ResponseId { get; set; }
        public int ResponseMessage { get; set; }
    }
}
