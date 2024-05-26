using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public GameObject buildingPanel;
    public bool entered;
    // Start is called before the first frame update
    void Start()
    {
        entered = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (entered && !GameManager.Instance.isPanelActive)
        {
            entered = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!entered & Input.GetKey(KeyCode.E))
        {
            Debug.Log("Enter building");
            entered = true;
            buildingPanel.SetActive(true);
            GameManager.Instance.isPanelActive = true;
            GameManager.Instance.activePanel = buildingPanel;
            bool inChase = FindObjectOfType<Wendigo>().state == Wendigo.State.CHASE ||
                FindObjectOfType<Wendigo>().state == Wendigo.State.FOLLOW;
            AlertHandler.Instance.Initialize(inChase);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        entered = false;
    }
}
