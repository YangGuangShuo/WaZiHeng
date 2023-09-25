using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BalanceCtrl : MonoBehaviour
{
    public void OnGameRestartBtn()
    {
        SceneManager.LoadScene("Balance");
    }
}
