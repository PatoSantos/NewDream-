using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonNoises : MonoBehaviour
{
    private int clickingTimes;
    public int clickingLimit;
    [SerializeField] private AudioSource noise;
    public GameObject soundBoxPrefab;
    // Start is called before the first frame update
    void Start()
    {
        clickingTimes = 0;
        GameObject spawnedSoundBox = Instantiate(soundBoxPrefab, transform.position, Quaternion.identity);
        noise = spawnedSoundBox.GetComponent<AudioSource>();
    }

    public void onClickNoise()
    {
        clickingTimes++;
        if (clickingTimes >= clickingLimit)
        {
            noise.Play();
            clickingTimes = 0;
        }
    }
}
