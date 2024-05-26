using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject activePanel;
    public bool isPanelActive = false;
    public GameObject gameOverPanel;

    public float gameOverInterval = 1.5f; // Timer interval in seconds
    private float timer = 0f;
    private bool isGameOver = false;

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
        gameOverPanel.SetActive(false);
        isGameOver = false;
    }

    // Update is called once per frame
    void Update()
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
        //Time.timeScale = 0.0f;
        gameOverPanel.SetActive(true);
        isGameOver=true;
        //Debug.LogWarning("YOURE DEAD!!!");
    }
}
