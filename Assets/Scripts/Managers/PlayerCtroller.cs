using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Wazi;

public class PlayerCtroller : MonoBehaviour
{
    [SerializeField]
    private Transform mPlayer;
    [SerializeField]
    private Transform mBad;
    [SerializeField]
    private Transform mFaDai;
    [SerializeField]
    private Transform mWuLiao;
    [SerializeField]
    private Transform mXingFen;
    [SerializeField]
    private Transform mShengQi;
    [SerializeField]
    private Transform mShangXin;
    [SerializeField]
    private RuntimeAnimatorController[] mAnimatorControllers;

    private CharacterController mCharacterCtrl;

    private Animator mAnimator;

    private bool IsRunning;

    private int mAnimatorIndex;

    // Start is called before the first frame update
    void Start()
    {
        mCharacterCtrl = mPlayer.GetComponent<CharacterController>();
        mAnimator = mPlayer.GetComponentInChildren<Animator>();
        IsRunning = false;
        mAnimatorIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float _x = InputManager.Instance.Horizontal;
        float _y = InputManager.Instance.Vertical;
        if (IsRunning)
        {
            if (_x == 0 && _y == 0)
            {
                IsRunning = false;
                mAnimator.SetBool("Running", false);
            }
        }
        //if (_x == 0)
        //{
        //    mAnimator.SetFloat("Left", 0);
        //    return;
        //}
        //if (_x == 0 && _y == 0)
        //{
        //    return;
        //}
            Vector3 direction = Vector3.right * _x + Vector3.forward * _y;
        float distance = Vector2.Distance(InputManager.Instance.Direction, Vector2.zero);
        mAnimator.SetFloat("Froward", distance);
        direction = direction.normalized;
        direction.y = 0;
        mCharacterCtrl.SimpleMove(direction * 1.2f);
        if (distance > 0.5)
        {
            mAnimator.SetBool("Running", true);
            IsRunning = true;
        }
        if (_x != 0 || _y != 0)
        {
            mAnimator.SetBool("Sitting", false);
            mAnimator.SetBool("IsSitting", false);
            mBad.gameObject.SetActive(false);
            mFaDai.gameObject.SetActive(false);
            mWuLiao.gameObject.SetActive(false);
            mXingFen.gameObject.SetActive(false);
            mShengQi.gameObject.SetActive(false);
            mShangXin.gameObject.SetActive(false);
        }

        //Debug.Log(direction);
        if (direction.x != 0 || direction.y != 0)
            mPlayer.rotation = Quaternion.Lerp(mPlayer.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 50);
    }

    public void Greeting()
    {
        mAnimator.SetBool("Greeting", true);

        mAnimator.SetBool("Sitting", false);
        mAnimator.SetBool("IsSitting", false);
        mBad.gameObject.SetActive(false);
        mFaDai.gameObject.SetActive(false);
        mWuLiao.gameObject.SetActive(false);
        mXingFen.gameObject.SetActive(false);
        mShengQi.gameObject.SetActive(false);
        mShangXin.gameObject.SetActive(false);
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);

        mAnimator.SetBool("Greeting", false);
    }

    IEnumerator Wait1()
    {
        yield return new WaitForSeconds(0.5f);

        mAnimator.SetBool("IsSitting", true);
    }

    public void Sitting()
    {
        mPlayer.localEulerAngles = Vector3.up * -180.0f;
        mAnimator.SetBool("Sitting", true);
        if (mAnimatorIndex == 1)
        {
            mFaDai.gameObject.SetActive(true);
        } 
        else if (mAnimatorIndex == 2)
        {
            StartCoroutine(Wait1());
            mWuLiao.gameObject.SetActive(true);
        }
        else if (mAnimatorIndex == 3)
        {
            mXingFen.gameObject.SetActive(true);
        }
        else if (mAnimatorIndex == 4)
        {
            mShengQi.gameObject.SetActive(true);
        }
        else if (mAnimatorIndex == 5)
        {
            mShangXin.gameObject.SetActive(true);
        }
        else
        {
            mBad.gameObject.SetActive(true);
        }
    }

    public void ChangeState()
    {
        mAnimatorIndex++;
        if (mAnimatorIndex >= mAnimatorControllers.Length)
        {
            mAnimatorIndex = 0;
        }
        mAnimator.runtimeAnimatorController = mAnimatorControllers[mAnimatorIndex];
        mBad.gameObject.SetActive(false);
        mFaDai.gameObject.SetActive(false);
        mWuLiao.gameObject.SetActive(false);
        mXingFen.gameObject.SetActive(false);
        mShengQi.gameObject.SetActive(false);
        mShangXin.gameObject.SetActive(false);
    }
}
