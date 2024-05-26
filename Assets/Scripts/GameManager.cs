using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject activePanel;
    public bool isPanelActive = false;
    public GameObject gameOverPanel;

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
    }

    // Update is called once per frame
    void Update()
    {
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
        Time.timeScale = 0.0f;
        gameOverPanel.SetActive(true);
        Debug.LogWarning("YOURE DEAD!!!");
    }
}
