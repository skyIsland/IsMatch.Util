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

            // 每一次内层循环把这一轮中的最大值放到最后
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

            // 每一次循环中要找出最小的元素。第一次遍历找出最小的元素排在第一位，第二次遍历找出次小的元素排在第二位，以此类推。
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

        /// <summary>
        /// 插入排序
        /// </summary>
        /// <param name="array"></param>
        /// <param name="ouput"></param>
        public static void InsertionSort(List<int> array, Action<List<int>> ouput = null)
        {
            if (array == null || !array.Any())
            {
                return;
            }

            if (ouput == null)
            {
                ouput = DefaultOuput;
            }

            // 每一次循环中要找出最小的元素。第一次遍历找出最小的元素排在第一位，第二次遍历找出次小的元素排在第二位，以此类推。
            int length = array.Count;
            for (int i = 1; i < length; i++)
            {
                int j = i;
                int temp = array[i];
                while (j > 0 && array[j - 1] > array[j])
                {
                    array[j] = array[j];
                    j--;
                }

                array[j] = temp;
            }

            ouput.Invoke(array);
        }

        private static List<int> Merge(List<int> left, List<int> right)
        {
            int i = 0;
            int j = 0;
            List<int> result = new List<int>();

            // 通过这个while循环将left和right中较小的部分放到result中
            while (i < left.Count && j < right.Count)
            {
                if (left[i] < right[i]) result.Add(left[i++]);
                else result.Add(right[j++]);
            }

            // 然后将组合left或right中的剩余部分
            result.AddRange(i < left.Count ? left.Take(i + 1).ToList() : right.Take(j+1).ToList());

            return result;
        }

        /// <summary>
        /// 归并排序 O(nlogn)
        /// </summary>
        /// <param name="array"></param>
        /// <param name="ouput"></param>
        public static List<int> MergeSort(List<int> array)
        {
            if (array == null || !array.Any())
            {
                return array;
            }

            int length = array.Count;
            if(length > 1)
            {
                // 找出中间位置
                int middle = (int)Math.Ceiling((double)length / 2);

                // 递归找出最小left
                List<int> left = MergeSort(array.Take(middle).ToList());

                // 递归找出最小right
                List<int> right = MergeSort(array.Skip(middle).Take(length).ToList());

                array = Merge(left, right);
            }

            return array;
        }

        /// <summary>
        /// 快速排序
        /// </summary>
        /// <param name="array"></param>
        /// <param name="ouput"></param>
        public static void QuickSort(List<int> array, Action<List<int>> ouput = null)
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
                    if (array[min] > array[j])
                    {
                        min = j;
                    }
                }

                if (min != i)
                {
                    int temp = array[i];
                    array[i] = array[min];
                    array[min] = temp;
                }
            }

            ouput.Invoke(array);
        }


        /// <summary>
        /// 堆排序
        /// </summary>
        /// <param name="array"></param>
        /// <param name="ouput"></param>
        public static void HeapSort(List<int> array, Action<List<int>> ouput = null)
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
                    if (array[min] > array[j])
                    {
                        min = j;
                    }
                }

                if (min != i)
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
