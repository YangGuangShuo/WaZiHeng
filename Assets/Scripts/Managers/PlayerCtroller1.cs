using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using Wazi;

public class PlayerCtroller1 : MonoBehaviour
{
    [SerializeField]
    private Transform mPlayer;
    [SerializeField]
    private Transform[] mMeshs;

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

        SetMaterialAnim();
    }

    // Update is called once per frame
    void Update()
    {
        return;
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
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);

        mAnimator.SetBool("Greeting", false);
    }

    public void Sitting()
    {
        mPlayer.localEulerAngles = Vector3.up * -180.0f;
        mAnimator.SetBool("Sitting", true);
    }

    public void SetMaterialAnim()
    {
        for (int i = 0; i < mMeshs.Length; i++)
        {
            Material[] mat = mMeshs[i].GetComponent<MeshRenderer>().materials;

            StartCoroutine(SetMaterialAnim1(mat));
        }
    }

    public IEnumerator SetMaterialAnim1(Material[] materials)
    {
        for (int j = 0; j < materials.Length; j++)
        {
            materials[j].DisableKeyword("_EMISSION");
            yield return new WaitForSeconds(0.1f);
            materials[j].EnableKeyword("_EMISSION");
            yield return new WaitForSeconds(0.1f);
            materials[j].DisableKeyword("_EMISSION");
            yield return new WaitForSeconds(0.1f);
            materials[j].EnableKeyword("_EMISSION");
        }
    }
}
