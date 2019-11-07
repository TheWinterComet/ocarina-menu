using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;

    [Header("-1 for unlimited use")]
    public int maxAmmo;

    [Header("0 = Unobtained, 1 = Obtained, 2 = Unusuable")]
    public int itemState;
}
