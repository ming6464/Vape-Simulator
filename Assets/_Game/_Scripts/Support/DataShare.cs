using System;
using System.Collections.Generic;
using UnityEngine;

public class DataShare : Singleton<DataShare>
{
    private Dictionary<DataShareKey, object> _dataShare = new ();

    public void SetData(DataShareKey key, object data)
    {
        if (data == null)
        {
            if (_dataShare.ContainsKey(key))
            {
                RemoveData(key);
            }
            return;
        }
        _dataShare[key] = data;
    }

    public bool TryGetData<T>(DataShareKey key, out T data)
    {
        data = default;
        if (_dataShare.TryGetValue(key, out object value) && value is T typedValue)
        {
            data = typedValue;
            return true;
        }
        return false;
    }

    public T GetData<T>(DataShareKey key)
    {
        T data = default;
        
        if (_dataShare.TryGetValue(key, out object value) && value is T typedValue)
        {
            data = typedValue;
        }
        
        return data;
    }

    public void RemoveData(DataShareKey key)
    {
        if(!HasKey(key)) return;
        _dataShare.Remove(key);
    }

    public bool HasKey(DataShareKey key) => _dataShare.ContainsKey(key);
}
public static class DataShareExtensions
{
    public static bool TryGetData<T>(this MonoBehaviour monoBehaviour, DataShareKey key, out T data)
    {
        return DataShare.Instance.TryGetData(key, out data);
    }

    public static T GetData<T>(this MonoBehaviour monoBehaviour, DataShareKey key)
    {
        return DataShare.Instance.GetData<T>(key);
    }

    public static void SetData(this MonoBehaviour monoBehaviour, DataShareKey key, object data)
    {
        DataShare.Instance.SetData(key, data);
    }

    public static void RemoveData(this MonoBehaviour monoBehaviour, DataShareKey key)
    {
        DataShare.Instance.RemoveData(key);
    }

    public static bool HasKey(this MonoBehaviour monoBehaviour, DataShareKey key)
    {
        return DataShare.Instance.HasKey(key);
    }
}