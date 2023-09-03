using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickBtn()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("UserInfo");
    }
}
