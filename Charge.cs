using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : MonoBehaviour
{
    // Every time the drone moves, its charge decreases. It starts with 100%, it decreases 0.5% per second while moving


    public int maxCharge = 100;
    public int currentCharge;

    // Start is called before the first frame update
    void Start()
    {
        currentCharge = maxCharge;
        
    }

    // Update is called once per frame
    public void TakeCharge(int amount)
    {
        currentCharge -= amount;

    }
}