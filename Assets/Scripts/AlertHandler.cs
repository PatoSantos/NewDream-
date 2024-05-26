using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertHandler : MonoBehaviour
{
    public static AlertHandler Instance;

    public int counterTolerance = 5;
    public int alertCounter = 0;
    public bool onAlert = false;
    public float noiseChance = 0.5f;

    public float timerInterval = 3.0f; // Timer interval in seconds
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (onAlert)
        {
            timer += Time.fixedDeltaTime;
            if (timer >= timerInterval)
            {
                GameManager.Instance.GameOver();
            }
        }
    }

    public void Initialize()
    {
        alertCounter = 0;
        onAlert = false;
    }

    public void StartAlert()
    {
        onAlert = true;
        timer = 0f;
    }

    public void EndAlert()
    {
        onAlert = false;
    }

    public void RegisterClick(float noiseChance)
    {
        if (Random.Range(0f, 1.0f) < noiseChance)
        {
            alertCounter++;
            if (alertCounter >= counterTolerance)
            {
                StartAlert();
            }
        }
    }

    public void RegisterClick()
    {
        if (Random.Range(0f, 1.0f) < noiseChance)
        {
            Debug.Log("Noise made!");
            alertCounter++;
            if (alertCounter >= counterTolerance)
            {
                StartAlert();
            }
        }
    }
}
