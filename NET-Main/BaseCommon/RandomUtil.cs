using System;
using System.Collections.Generic;

namespace BaseCommon
{
    /// <summary>
    /// 随机数工具类
    /// </summary>
    public class RandomUtil
    {
        /// <summary>
        /// 蓄水池算法
        /// </summary>
        /// <param name="source"></param>
        /// <param name="num"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> Sampling<T>(List<T> source, int num)
        {
            // 判断原始数据数量是否不足
            if (source.Count <= num)
            {
                return source;
            }
            Random r = new Random();
            // 首先构建一个可容纳num个元素的数组
            var list = new List<T>(num);
            // 将序列的前𝑘个元素放入数组中
            for (int i = 0; i < num; i++)
            {
                list.Add(source[i]);
            }
            // 从第num+1个元素开始，以num/source.Count的概率来决定该元素是否被替换到数组中
            for (int j = num; j < source.Count; j++)
            {
                var temp = r.Next(0, j);
                if (temp < num)
                {
                    list[temp] = source[j];
                }
            }
            // 遍历完所有元素后，数组中剩下的元素即为所需采样的样本
            return list;
        }
    }
}
