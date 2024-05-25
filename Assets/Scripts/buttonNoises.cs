using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonNoises : MonoBehaviour
{
    private int clickingTimes;
    public int clickingLimit;
    [SerializeField]private AudioSource noise;
    // Start is called before the first frame update
    void Start()
    {
        clickingTimes = 0;   
    }

    void onClickNoise()
    {
        clickingTimes++;
        if(clickingTimes == clickingLimit)
        {
            noise.Play();
            clickingTimes = 0;
        }
    }
}
