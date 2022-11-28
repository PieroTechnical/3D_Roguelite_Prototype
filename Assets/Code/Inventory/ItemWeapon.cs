using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemWeapon : Item
{

    public string animationName = "LightAttack1";

    public virtual bool TryAttack()
    {
        Debug.Log(string.Format("Trying to attack with {0}", itemName));
        return true;
    }

    public virtual string GetAnim()
    {
        return animationName;
    }
}
