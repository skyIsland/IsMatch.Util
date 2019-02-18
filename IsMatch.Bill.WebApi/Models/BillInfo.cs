using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IsMatch.Bill.WebApi.Models
{
    public class BillInfo : BaseModel
    {
        /// <summary> 变动标题</summary>
        public string Title { get; set; }

        /// <summary> 变动类型 0支出 1收入 2冲账</summary>
        public int Type { get; set; }

        /// <summary> 变动金额</summary>
        public double Money { get; set; }

    }
}
