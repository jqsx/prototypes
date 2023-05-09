using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiHashMap<K1, K2, V>
{
    Dictionary<K1, Dictionary<K2, V>> values = new Dictionary<K1, Dictionary<K2, V>>();

    public V set(K1 k1, K2 k2, V v)
    {
        if (values.TryGetValue(k1, out Dictionary<K2, V> value))
        {
            value.Add(k2, v);
        }
        else
        {
            Dictionary<K2, V> n = new Dictionary<K2, V>();
            n.Add(k2, v);
            values.Add(k1, n);
        }
        return v;
    }

    public V get(K1 k1, K2 k2)
    {
        if (values.TryGetValue(k1, out Dictionary<K2, V> value))
        {
            if (value.TryGetValue(k2, out V v)) {
                return v;
            }
        }
        return default(V);
    }
}
