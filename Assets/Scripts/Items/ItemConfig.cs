using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemConfig : MonoBehaviour //COMMENT THIS
{
    public static ItemConfig Instance;
    [Serializable]
    public struct TypeItemPair
    {
        public ItemType _type;
        public GameObject worldItem;
    }

    public List<TypeItemPair> ItemList;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject GetPrefab(ItemType type)
    {
        foreach (var pair in ItemList)
        {
            if (type == pair._type)
            {
                return pair.worldItem;
            }
        }
        return null;
    }
    
}
