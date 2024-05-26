using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankUI : MonoBehaviour
{
    [SerializeField] private GameObject Box;
    [SerializeField] private GameObject BoxOpen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
