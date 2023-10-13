using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    [SerializeField]
    private int gold = 500;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.G))
        {
            gold += 1000;
        }
    }

    public int GetGold() { return gold; }
    public void AddGold(int amount)
    {
        gold += amount;
    }
    public bool SubGold(int amount)
    {
        if (gold < amount)
            return false;
        gold -= amount;
        return true;
    }
}
