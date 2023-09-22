using UnityEngine;

public class TestRunningAverage : MonoBehaviour
{
    public bool validate;
    public int sampleSize = 100;
    public int sampleCount;
    public float average;
    public float[] rewards;
    
    void OnValidate()
    {
        if (!validate) return;
        validate = false;
        AddDataPoint();
    }
    
    void AddDataPoint()
    {
        if (sampleCount < sampleSize)
        {
            rewards[sampleCount] = Random.Range(-1f, 1f);
            sampleCount++;
        }
        else
        {
            average = GetAverage(rewards);
            ClearHistory();
            AddDataPoint();
        }        
    }
    
    void ClearHistory()
    {
        sampleCount = 0;
        for (int i = 0; i < rewards.Length; i++)
            rewards[i] = 0;        
    }
    
    float GetAverage(float[] array)
    {
        float sum = 0f;
        foreach (var item in array)
            sum += item;
        
        return sum/array.Length;
    }
}
