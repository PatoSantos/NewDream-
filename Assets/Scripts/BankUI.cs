using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankUI : MonoBehaviour
{
    [SerializeField] private GameObject Box;
    [SerializeField] private GameObject BoxOpen;
    [SerializeField] private GameObject building;
    [SerializeField] private GameObject MissClick;
    [SerializeField] private GameObject Alarm;
    [SerializeField] private GameObject ExitButton;
    [SerializeField] private AudioSource WendigoRoar;
    [SerializeField] private AudioSource Chase;


    // Start is called before the first frame update
    void Start()
    {

    }
    
    public void OpenMenu()
    {
        Alarm.SetActive(false);
        MissClick.SetActive(true);
        ExitButton.SetActive(true);
        BoxOpen.SetActive(false);
        Box.SetActive(true);
    }

    private void FixedUpdate()
    {
        if (AlertHandler.Instance.onAlert || AlertHandler.Instance.inChase)
        {
            Alarm.SetActive(true);
        }
    }


    public void boxClick()
    {
        BoxOpen.SetActive(true);
        Box.SetActive(false);
    }
    public void boxOpenClick()
    {
        BoxOpen.SetActive(false);
        Box.SetActive(true);
    }

    public void exitClick()
    {
        //Building.SetActive(false);
        GameManager.Instance.ExitPanel();
        AlertHandler.Instance.EndAlert();
        Alarm.SetActive(false);
    }

    public void badclick()
    {
        AlertHandler.Instance.RegisterClick(.7f);
        if (AlertHandler.Instance.onAlert)
        {
            Alarm.SetActive(true);
            WendigoRoar.Play();
            Chase.Play();

        }
    }
}
