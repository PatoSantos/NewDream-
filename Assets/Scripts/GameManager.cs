using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject activePanel;
    public bool isPanelActive = false;
    public GameObject gameOverPanel;
    public GameObject endPanel;

    public float gameOverInterval = 2.0f; // Timer interval in seconds
    private float timer = 0f;
    public bool isGameOver = false;

    AudioSource audioManager;
    public AudioClip wendigoScream;
    public AudioClip cowboyScream;

    public float maxHealthTimer = 30f;
    public float healthTimer = 0f;

    public Sprite[] phases;
    public GameObject hand;

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
        healthTimer += Time.fixedDeltaTime;

        hand.GetComponent<Image>().sprite = phases[0];
        if (healthTimer > (maxHealthTimer / 5))
        {
            hand.GetComponent<Image>().sprite = phases[1];
        }
        if (healthTimer > 2*(maxHealthTimer / 5))
        {
            hand.GetComponent<Image>().sprite = phases[2];
        }
        if (healthTimer > 3*(maxHealthTimer / 5))
        {
            hand.GetComponent<Image>().sprite = phases[3];
        }
        if (healthTimer > 4*(maxHealthTimer / 5))
        {
            hand.GetComponent<Image>().sprite = phases[4];
        }

        if (healthTimer > maxHealthTimer)
        {
            isGameOver = true;
            audioManager.volume = 1.0f;
            audioManager.clip = cowboyScream;
            audioManager.Play();
        }
        if (isGameOver)
        {
            timer += Time.fixedDeltaTime;

            if (timer >= gameOverInterval)
            {
                endPanel.SetActive(true);
                Debug.Log("quitting");
                Application.Quit();
            }
        }
        if (isPanelActive && Input.GetKey(KeyCode.Escape))
        {
            ExitPanel();
            AlertHandler.Instance.EndAlert();
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            hand.SetActive(!hand.activeSelf);
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
