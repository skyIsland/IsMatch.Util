using System;
using System.Collections.Generic;
using System.Text;

namespace IsMatch.Models
{
    /// <summary>
    /// 通用返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Prompt<T>
    {
        public int Status { get; set; } = 1;

        public T Data { get; set; }

        public string Message { get; set; }
    }

    public class Prompt : Prompt<object> { }
}
