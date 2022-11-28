using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{

    public string itemName;
    public int weight;
    public int value;
    public GameObject itemPrefab;

    public virtual bool TryUseItem() => false;

    public virtual bool TryHoldItem() => false;

    public virtual bool TryDropItem() => true;
}
