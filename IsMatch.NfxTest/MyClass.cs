using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsMatch.NfxTest
{
    public class MyClass
    {

        public int Id { get; set; }

        /// <summary>
        /// 时间段
        /// </summary>
        public DateTime TimePoint { get; set; }

        /// <summary>
        /// 站点名称
        /// </summary>
        public string StationName { get; set; }

        public string GetValueByPropName(string propName)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(propName))
            {
                var propInfo = this.GetType().GetProperty(propName);
                if (propInfo != null)
                {
                    result = propInfo.GetValue(this).ToString();
                }
            }

            return result;
        }

    }
}
