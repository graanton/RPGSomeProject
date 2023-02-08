using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

[Serializable]
public class Pool<T>
{
    public List<WeightedObject<T>> weigthObjects;

    public WeightedObject<T> GetRandomWeightedObject()
    {
        float totalWeigth = CulculateWeigths();
        float randomValue = Random.Range(0f, totalWeigth);

        float weigth = 0;
        foreach (var currentWeightOject in weigthObjects)
        {
            weigth += currentWeightOject.weight;
            if (weigth >= randomValue)
            {
                return currentWeightOject;
            }
        }

        throw new NullReferenceException("List is void");
    }

    private float CulculateWeigths()
    {
        float totalWeigth = 0f;

        foreach (var currentWeightOject in weigthObjects)
        {
            totalWeigth += currentWeightOject.weight;
        }

        return totalWeigth;
    }
}

[Serializable]
public struct WeightedObject<T>
{
    public T obj;
    public float weight;
}