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
        /// 中缀表达式转后缀表达式 “9+(3- 1)x3+10+2” --> “931-3*+102/+”
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetRPN(string expression)
        {
            string result = string.Empty;

            // 栈顶
            int top = -1;

            // 初始化空栈，用来对符号进出栈使用
            var operators = new List<char>();

            /* 从左到右遍历
                 数字直接输出
                 符号则判断与栈顶符号的优先级，
                    是右括号或优先级低级栈顶符号则栈顶元素依次出栈并输出
                        并将当前符号进栈
                一直到最终输出
            */
            for (int i = 0; i < expression.Length; i++)
            {
                var expressionCurr = expression[i];

                if (char.IsWhiteSpace(expressionCurr))
                {
                    continue;
                }

                switch (expressionCurr)
                {
                    case '+':
                    case '-':
                       if(top >= 0)
                        {
                            var operatorsCurr = operators[top];

                            // 栈顶是乘除比加减优先级高 则栈顶元素依次出栈
                            if(operatorsCurr.Equals("*") || operatorsCurr.Equals("/"))
                            {
                                while (top >= 0)
                                {
                                    result += operators[top--];
                                }
                            }
                        }
                        operators.Add(expressionCurr);// push 
                        break;
                    default:
                        result += expressionCurr;
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
