using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SherrifUI : MonoBehaviour
{
    private int missClickCount;
    [Header("Menu Basics")]
    [SerializeField] private GameObject Building;
    [SerializeField] private GameObject ExitButton;
    [SerializeField] private GameObject MissButton;
    [SerializeField] private GameObject Alarm;
    [Header("Interactable Objects")]
    [SerializeField] private GameObject C1_Item;
    [SerializeField] private GameObject IC_Item;
    [SerializeField] private GameObject C1_Note;
    [SerializeField] private GameObject IC_Note;

    // Start is called before the first frame update
    void Start()
    {
        OpenMenu();
    }

    private void FixedUpdate()
    {
        if (AlertHandler.Instance.onAlert || AlertHandler.Instance.inChase)
        {
            Alarm.SetActive(true);
        }
    }

    public void OpenMenu()
    {
        missClickCount = 0;
        Alarm.SetActive(false);
        MissButton.SetActive(true);
        ExitButton.SetActive(true);
        C1_Note.SetActive(false);
        IC_Note.SetActive(false);
        C1_Item.SetActive(true);
        IC_Item.SetActive(true);
    }

    public void CloseMenu()
    {
        //Building.SetActive(false);
        GameManager.Instance.ExitPanel();
        AlertHandler.Instance.EndAlert();
        Alarm.SetActive(false);
    }

    public void C1_Open()
    {
        C1_Note.SetActive(true);
        IC_Note.SetActive(false);
        C1_Item.SetActive(false);
        IC_Item.SetActive(false);
    }

    public void C1_Close()
    {
        C1_Note.SetActive(false);
        IC_Note.SetActive(false);
        C1_Item.SetActive(true);
        IC_Item.SetActive(true);
    }

    public void IC_Open()
    {
        C1_Note.SetActive(false);
        IC_Note.SetActive(true);
        C1_Item.SetActive(false);
        IC_Item.SetActive(false);
    }

    public void IC_Close()
    {
        C1_Note.SetActive(false);
        IC_Note.SetActive(false);
        C1_Item.SetActive(true);
        IC_Item.SetActive(true);
    }

    public void MissClick()
    {
        AlertHandler.Instance.RegisterClick(.7f);
        if (AlertHandler.Instance.onAlert)
        {
            Alarm.SetActive(true);
        }
    }
}
