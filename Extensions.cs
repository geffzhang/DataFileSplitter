using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DataFileSplitter
{
    static class Extensions
    {
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> enumerable, int testFraction = 20, int devFraction = 0)
        {
            int itemsReturned = 0;
            var list = enumerable.ToList();
            int count = list.Count;

            int fraction = count / 100;
            int testCount = fraction * testFraction;
            int devCount = fraction * devFraction;
            int trainCount = fraction * (100-(testFraction + devFraction));
            int remaining = count - (trainCount + devCount + testCount);

            if (remaining > 0)
            {
                if (devCount == 0)
                {
                    trainCount += remaining;
                }
                else
                {
                    devCount += remaining;
                }
            }

            yield return list.GetRange(0, trainCount);
            yield return list.GetRange(trainCount, testCount);
            if (devCount>0)
            {
                yield return list.GetRange(trainCount + testCount, devCount);
            }
        }
    }
}
