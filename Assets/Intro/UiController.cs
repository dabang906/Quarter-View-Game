using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    public GameObject ui;
    public GameObject uiCam;
    public GameObject mainCam;

    public void OnUI()
    {
        mainCam.SetActive(false);
        ui.SetActive(true);
        uiCam.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
