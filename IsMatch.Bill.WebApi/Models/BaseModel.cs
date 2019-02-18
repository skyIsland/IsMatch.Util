using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsMatch.Bill.WebApi.Models
{
    public abstract class BaseModel
    {
        public virtual int Id { get; set; }

        public virtual DateTime AddTime { get; set; }

        public virtual DateTime EditTime { get; set; }

        public virtual int IsDelete { get; set; }
    }
}
