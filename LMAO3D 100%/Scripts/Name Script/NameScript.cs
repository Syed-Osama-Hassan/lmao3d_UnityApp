using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameScript : MonoBehaviour
{   
    [SerializeField] private GameObject testConnect, singletonReference;
    [SerializeField] private Canvas name_canvas;
    [SerializeField] private GameObject _canvases;

    public void Start()
    {
        testConnect.SetActive(false);
        singletonReference.SetActive(false);
        _canvases.gameObject.SetActive(false);
    }


    public void continue_Btn()
    {
        name_canvas.gameObject.SetActive(false);
        testConnect.SetActive(true);
        singletonReference.SetActive(true);
        _canvases.gameObject.SetActive(true);
    }
}
