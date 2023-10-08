using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BalanceCtrl : MonoBehaviour
{
    [SerializeField] Text m_CoinText;
    [SerializeField] Text m_DeathText;
    [SerializeField] Text m_MoveText;

    private int m_CoinCount = 0;

    private int m_DeathCount = 0;

    private float m_Move = 0;

    private void Start()
    {
        SoundManager.Instance.BGM();
        SetCoin();
        m_DeathCount = PlayerPrefs.GetInt("Death");
        SetDeath();
        m_Move = 0;
    }

    public void OnGameRestartBtn()
    {
        SceneManager.LoadScene("Balance");
    }

    public void OnMusicBtn(GameObject btn)
    {
        GameObject on = btn.transform.Find("On").gameObject;
        GameObject off = btn.transform.Find("Off").gameObject;
        bool active = !on.activeSelf;
        on.SetActive(active);
        off.SetActive(!active);
        SoundManager.Instance.BGM(active);
    }

    public void EatCoin()
    {
        m_CoinCount++;
        SetCoin();
    }

    public void Death()
    {
        m_DeathCount++;
        SetDeath();
    }

    public void Move(float move)
    {
        m_Move += move;
        m_MoveText.text = m_Move.ToString("F2") + "m";
    }

    private void SetCoin()
    {
        m_CoinText.text = m_CoinCount.ToString();
    }

    private void SetDeath()
    {
        m_DeathText.text = m_DeathCount.ToString();
        PlayerPrefs.SetInt("Death", m_DeathCount);
    }
}
