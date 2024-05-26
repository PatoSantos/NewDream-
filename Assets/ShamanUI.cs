using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShamanUI : MonoBehaviour
{
    [Header("Menu Basics")]
    [SerializeField] private GameObject Building;
    [SerializeField] private GameObject ExitButton;
    [SerializeField] private GameObject MissButton;
    [SerializeField] private GameObject Alarm;
    [Header("Interactable Objects")]
    [SerializeField] private GameObject Caja_Item;
    [SerializeField] private GameObject Banco_Item;
    [SerializeField] private GameObject Caja_Note;
    [SerializeField] private GameObject Banco_Note;
    [Header("Vault")]
    private int num1;
    private int num2;
    private int num3;
    private bool open;
    [SerializeField] private GameObject Num1Text;
    [SerializeField] private GameObject Num2Text;
    [SerializeField] private GameObject Num3Text;
    [SerializeField] private GameObject VaultTry;
    [SerializeField] private GameObject Gun;

    // Start is called before the first frame update
    void Start()
    {
        OpenMenu();
    }

    public void OpenMenu()
    {
        num1 = 0;
        num2 = 0;
        num3 = 0;
        open = false;
        Alarm.SetActive(false);
        MissButton.SetActive(true);
        ExitButton.SetActive(true);
        Caja_Note.SetActive(false);
        Banco_Note.SetActive(false);
        Caja_Item.SetActive(true);
        Banco_Item.SetActive(true);
        Num1Text.SetActive(true);
        Num2Text.SetActive(true);
        Num3Text.SetActive(true);
        VaultTry.SetActive(true);
        Gun.SetActive(false);
    }

    public void CloseMenu()
    {
        //Building.SetActive(false);
        GameManager.Instance.ExitPanel();
        AlertHandler.Instance.EndAlert();
    }

    public void Caja_Open()
    {
        Caja_Note.SetActive(true);
        Banco_Note.SetActive(false);
        Caja_Item.SetActive(false);
        Banco_Item.SetActive(false);
        Num1Text.SetActive(false);
        Num2Text.SetActive(false);
        Num3Text.SetActive(false);
        VaultTry.SetActive(false);
        Gun.SetActive(false);
    }

    public void Caja_Close()
    {
        Caja_Note.SetActive(false);
        Banco_Note.SetActive(false);
        Caja_Item.SetActive(true);
        Banco_Item.SetActive(true);
        if (open)
        {
            Gun.SetActive(true);
        }
        else
        {
            Num1Text.SetActive(true);
            Num2Text.SetActive(true);
            Num3Text.SetActive(true);
            VaultTry.SetActive(true);
        }
    }

    public void Banco_Open()
    {
        Caja_Note.SetActive(false);
        Banco_Note.SetActive(true);
        Caja_Item.SetActive(false);
        Banco_Item.SetActive(false);
        Num1Text.SetActive(false);
        Num2Text.SetActive(false);
        Num3Text.SetActive(false);
        VaultTry.SetActive(false);
        Gun.SetActive(false);
    }

    public void Banco_Close()
    {
        Caja_Note.SetActive(false);
        Banco_Note.SetActive(false);
        Caja_Item.SetActive(true);
        Banco_Item.SetActive(true);
        if (open)
        {
            Gun.SetActive(true);
        }
        else
        {
            Num1Text.SetActive(true);
            Num2Text.SetActive(true);
            Num3Text.SetActive(true);
            VaultTry.SetActive(true);
        }
    }

    public void Num1()
    {
        num1 = (num1 + 1) % 10;
        Num1Text.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().SetText(num1.ToString());
    }

    public void Num2()
    {
        num2 = (num2 + 1) % 10;
        Num2Text.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().SetText(num2.ToString());
    }

    public void Num3()
    {
        num3 = (num3 + 1) % 10;
        Num3Text.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().SetText(num3.ToString());
    }

    public void OpenVault()
    {
        if (num1 == 2 && num2 == 3 && num3 == 5)
        {
            open = true;
            Num1Text.SetActive(false);
            Num2Text.SetActive(false);
            Num3Text.SetActive(false);
            VaultTry.SetActive(false);
            Gun.SetActive(true);
        }
        else {
            MissClick();
        }
    }

    public void TakeGun()
    {
        Debug.Log("Comenzar Final");
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
