using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject activePanel;
    public bool isPanelActive = false;
    public GameObject gameOverPanel;

    public float gameOverInterval = 2.0f; // Timer interval in seconds
    private float timer = 0f;
    public bool isGameOver = false;

    AudioSource audioManager;
    public AudioClip wendigoScream;
    public AudioClip cowboyScream;

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
        audioManager = GetComponent<AudioSource>();
        gameOverPanel.SetActive(false);
        isGameOver = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isGameOver)
        {
            timer += Time.fixedDeltaTime;

            if (timer >= gameOverInterval)
            {
                Debug.Log("quitting");
                Application.Quit();
            }
        }
        if (isPanelActive && Input.GetKey(KeyCode.Escape))
        {
            ExitPanel();
            AlertHandler.Instance.EndAlert();
        }
    }

    public void ExitPanel()
    {
        if (activePanel != null)
        {
            activePanel.gameObject.SetActive(false);
        }
        isPanelActive = false;
    }

    public void GameOver()
    {
        if (!isGameOver)
        {
            gameOverPanel.SetActive(true);
            isGameOver = true;
            audioManager.volume = 1.0f;
            audioManager.clip = cowboyScream;
            audioManager.Play();
        }
        //Time.timeScale = 0.0f;
        
        //Debug.LogWarning("YOURE DEAD!!!");
    }

    public void PlayScream()
    {
        audioManager.clip = wendigoScream;
        audioManager.Play();
    }
}
