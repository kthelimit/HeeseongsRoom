using System;
using System.Collections.Generic;

public class WeightRandom
{
    public Dictionary<string, float> inputDatas = new Dictionary<string, float>();
    public Dictionary<string, float> weightDatas = new Dictionary<string, float>();
    float total = 0;
    bool isFinded = false;
    float check;
    public void Init()
    {
        foreach (KeyValuePair<string, float> a in inputDatas)
        {
            total += a.Value;
        }
        foreach (KeyValuePair<string, float> a in inputDatas)
        {
            weightDatas.Add(a.Key, a.Value / total);
        }
    }

    public string PickRandom()
    {
        isFinded = false;
        check = 0;
        Random random = new Random();
        double randomvalue = random.NextDouble();

        while(!isFinded)
        {
            foreach(KeyValuePair<string, float> a in weightDatas)
            {
                check += a.Value;
                if(randomvalue<check)
                {
                    isFinded = true;
                    return a.Key;
                }
            }
        }
        return null;
    }


}
