using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsMatch.NfxTest.Sorting
{
    public class SortingMethod
    {
        private static Action<List<int>> DefaultOuput = x =>
        {
            if(x != null && x.Any())
            {
                Console.WriteLine(string.Join(",", x));
            }
        };

        /// <summary>
        /// 冒泡排序 算法的复杂度为O(n2)。
        /// </summary>
        /// <param name="array"></param>
        public static void BubbleSort(List<int> array,Action<List<int>> ouput = null)
        {
            if(array == null || !array.Any())
            {
                return;
            }

            if(ouput == null)
            {
                ouput = DefaultOuput;
            }

            int length = array.Count;
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length - 1 - i; j++)
                {
                    if(array[j] > array[j + 1])
                    {
                        int temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }

            ouput.Invoke(array);
        }

        /// <summary>
        /// 选择排序 算法的复杂度为O(n2)。
        /// </summary>
        /// <param name="array"></param>
        /// <param name="ouput"></param>
        public static void SelectionSort(List<int> array, Action<List<int>> ouput = null)
        {
            if (array == null || !array.Any())
            {
                return;
            }

            if (ouput == null)
            {
                ouput = DefaultOuput;
            }

            int length = array.Count;
            for (int i = 0; i < length - 1; i++)
            {
                int min = i;
                for (int j = i; j < length; j++)
                {
                    if(array[min] > array[j])
                    {
                        min = j;
                    }
                }

                if(min != i)
                {
                    int temp = array[i];
                    array[i] = array[min];
                    array[min] = temp;
                }
            }

            ouput.Invoke(array);
        }
    }
}
