using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsMatch.NfxTest
{
    public class RPN
    {
        /// <summary>
        /// 中缀表达式转后缀表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetRPN(string expression)
        {
            string result = string.Empty;

            // 栈顶
            int Top = -1;

            // 初始化空栈，用来对符号进出栈使用
            var operators = new List<char>();

            /* 从左到右遍历
                 数字直接输出
                 符号则判断与栈顶符号的优先级，
                    是右括号或优先级低级栈顶符号则栈顶元素依次出栈并输出，
                        并将当前符号进栈
                一直到最终输出
            */
            for (int i = 0; i < expression.Length; i++)
            {
                var curr = expression[i];

                if (char.IsWhiteSpace(curr))
                {
                    continue;
                }

                switch (curr)
                {
                    case '+':
                    case '-':

                        break;
                    default:
                        result += curr;
                        break;
                }
            }

            return result;
        }

        public static decimal GetResult(string expression)
        {
            decimal result = -99M;

            return result;
        }
    }
}
