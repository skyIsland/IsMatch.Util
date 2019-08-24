using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsMatch.NfxTest.LeetCode
{
    public class ArraySubject
    {
        public static void TestControl()
        {
            Console.WriteLine(string.Join(",", TwoSum(new int[] { 2, 7, 11, 15 }, 9)));

            Console.WriteLine(string.Join("\r\n", ThreeSum(new int[] { -1, 0, 1, 2, -1, -4 }).Select(p => string.Join(",", p))));
        }

        /// <summary>
        /// Input: numbers={2, 7, 11, 15}, target=9 
        /// Output: index1=1, index2=2
        /// </summary>
        /// <param name="nums"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private static int[] TwoSum(int[] nums, int target)
        {
            var result = new int[2];
            if (nums == null || nums.Length <= 1)
            {
                return result;
            }

            Dictionary<int, int> map = new Dictionary<int, int>();

            for (int i = 0; i < nums.Length; i++)
            {
                map.Add(target - nums[i], i);
            }

            for (int i = 0; i < nums.Length; i++)
            {
                if (map.ContainsKey(nums[i]))
                {
                    var v = map[nums[i]];

                    if (v != i)
                    {
                        result[0] = i + 1;
                        result[1] = v + 1;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 给定一个数组，找出其中所有不同的三数和等于0的组合。
        /// For example, given array S = {-1 0 1 2 -1 -4},
        /// solution set is:
        ///(-1, 0, 1)
        ///(-1, -1, 2)
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        private static List<List<int>> ThreeSum(int[] nums)
        {
            if (nums == null || nums.Length < 3)
            {
                return new List<List<int>>();
            }

            HashSet<List<int>> set = new HashSet<List<int>>();

            // 对数组进行排序
            Array.Sort(nums);

            for (int start = 0; start < nums.Length; start++)
            {
                if (start != 0 && nums[start - 1] == nums[start])
                {
                    continue;
                }

                int mid = start + 1, end = nums.Length - 1;

                while (mid < end)
                {
                    int sum = nums[start] + nums[mid] + nums[end];

                    if (sum == 0)
                    {
                        List<int> tmp = new List<int>();

                        tmp.Add(nums[start]);
                        tmp.Add(nums[mid]);
                        tmp.Add(nums[end]);
                        set.Add(tmp);

                        while (++mid < end && nums[mid - 1] == nums[mid]);
                        while (--end > mid && nums[end + 1] == nums[end]);
                    }
                    else if (sum < 0)
                    {
                        mid++;
                    }
                    else
                    {
                        end--;
                    }
                }
            }
            return new List<List<int>>(set);
        }
    }
}
