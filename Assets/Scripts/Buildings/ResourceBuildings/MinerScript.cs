using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerScript : BuildingSuperScript
{
    public float miningTimer = 5f;
    public int amountMined = 1;
    public GameObject ore;
    private ItemInfo _itemInfo;
    // Start is called before the first frame update
    void Start()
    {
        name = "Miner";
        MaxDurability = 60f;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Deposit")) return;
        miningTimer = 5f;
        _itemInfo = other.GetComponent<ItemInfo>();
        ore = ItemConfig.Instance.GetPrefab(_itemInfo.Item.Type);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Deposit"))
            miningTimer -= Time.deltaTime;

        if (miningTimer <= 0 && _itemInfo.Item.Amount > 0)
        {
            _itemInfo.Item.Amount -= amountMined;
            Instantiate(ore, output);
            miningTimer = 5f;
        }
                 
    }
}
