using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RandomUtil
{
    public static T GetRandomElemntFromWeightedList<T>(IList<System.Tuple<T, float>> pairs)
    {
        float maxWeight = pairs.Sum(p => p.Item2);
        float rnd = Random.Range(0, maxWeight);
        float runningWeight = pairs[0].Item2;
        for (int i = 0; i < pairs.Count(); i++)
        {
            var currentWeight = pairs[i].Item2;
            if (rnd < runningWeight)
                return pairs[i].Item1;
            runningWeight += currentWeight;
        }
        return pairs.Last().Item1;
    }
}
