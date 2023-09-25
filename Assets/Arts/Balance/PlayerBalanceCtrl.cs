

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Wazi;
using static UnityEngine.GraphicsBuffer;

public class PlayerBalanceCtrl : MonoBehaviour
{
    private Animator mAnimator;

    private Vector3 gyroInitialGravity;

    private CharacterController characterController;

    private List<Collider> colliders = new List<Collider>();

    private List<Renderer> renderers = new List<Renderer>();

    private bool mIsDeath = false;

    private float mSpeed = 0.3f;

    private int mStatus = 0;

    private bool isJump = false;
    private bool isBeginJump = false;
    public float jumpTime = 1f;//跳跃时间
    public float jumpTimeFlag = 0;//累计跳跃时间用来判断是否结束跳跃
    public float jumpSpeed = 2;//跳跃速度
    public float gravity = 0.95f;//模拟重力

    void Start()
    {
        mAnimator = gameObject.GetComponent<Animator>();
        Input.gyro.enabled = true;
        gyroInitialGravity = Input.gyro.gravity;
        characterController = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        if (mIsDeath)
        {
            transform.Translate(Vector3.down * Time.deltaTime * 3);
            return;
        }
        float _x = InputManager.Instance.Horizontal;
        float _y = InputManager.Instance.Vertical;
        Vector3 direction = Vector3.right * _x + Vector3.forward * _y;
        float distance = Vector2.Distance(InputManager.Instance.Direction, Vector2.zero);
        mAnimator.SetFloat("forward", distance);
        direction = direction.normalized;
        //direction.y = 0;

        mAnimator.SetFloat("right", (Input.gyro.gravity - gyroInitialGravity).x);
        if ((direction.x != 0 || direction.y != 0))
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
        if (distance > 0.9f && mStatus != 2)
        {
            mSpeed = 1f;
        } else
        {
            mSpeed = 0.3f;
        }
        
        CheckDeath();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        //if (isBeginJump)
        //{
        //    isBeginJump = false;
        //    direction.y = jumpSpeed;
        //}
        if (isJump)
        {
            if (jumpTimeFlag < jumpTime)
            {
                characterController.Move((transform.up + transform.forward * 2) * jumpSpeed * Time.deltaTime);
                jumpTimeFlag += Time.deltaTime;
            }
            else if (jumpTime < jumpTimeFlag)
            {
                characterController.Move(transform.up * -gravity * Time.deltaTime);
            }
            if (characterController.collisionFlags == CollisionFlags.Below)
            {
                jumpTimeFlag = 0;
                isJump = false;
            }
        }
        else
        {
            if (characterController.collisionFlags != CollisionFlags.Below)
                characterController.Move(transform.up * -gravity * Time.deltaTime);
            direction.y = 0;
        }
        characterController.Move(direction * mSpeed * Time.deltaTime);
        IsMirror(direction);
    }

    private void CheckDeath()
    {
        if (transform.position.y <= -0.5f && !mIsDeath)
        {
            mIsDeath = true;
            StartCoroutine(WaitRestart());
        }
    }

    private void IsMirror(Vector3 direction)
    {
        if (mAnimator.GetCurrentAnimatorStateInfo(0).IsName("IP_Loop_Walk_Edge_Right_02") || mAnimator.GetCurrentAnimatorStateInfo(0).IsName("IP_Loop_Run_Right_Edge_01"))
        {
            if (direction.z < 0 || direction.x < 0)
            {
                mAnimator.SetBool("isMirror", true);
                transform.GetChild(0).localEulerAngles = Vector3.up * 90.0f;
            } 
            else
            {
                mAnimator.SetBool("isMirror", false);
                transform.GetChild(0).localEulerAngles = Vector3.up * -90.0f;
            }
        }
        
    }

    private void CheckFade()
    {
        RaycastHit hit;
        Ray rayForward = new Ray(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward));

        if (Physics.Raycast(rayForward, out hit, 3000))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Renderer _renderer = hit.collider.GetComponent<Renderer>();
                renderers.Add(_renderer);
                SetMaterialsColor(_renderer, 0.5f);
            } else
            {
                for (int i = 0; i < renderers.Count; i++)
                {
                    Renderer obj = renderers[i];
                    SetMaterialsColor(obj, 1f);
                }
            }
        }
        else
        {
            for (int i = 0; i < renderers.Count; i++)
            {
                Renderer obj = renderers[i];
                SetMaterialsColor(obj, 1f);
            }
        }
    }

    private void SetMaterialsColor(Renderer _renderer, float Transpa)
    {
        //换shader或者修改材质

        //获取当前物体材质球数量
        int materialsNumber = _renderer.sharedMaterials.Length;
        for (int i = 0; i < materialsNumber; i++)
        {

            //获取当前材质球颜色
            Color color = _renderer.materials[i].color;

            //设置透明度  0-1;  0 = 完全透明
            color.a = Transpa;

            //置当前材质球颜色
            _renderer.materials[i].SetColor("_Color", color);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            colliders.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log(colliders.Count);
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") && mIsDeath == false)
        {
            colliders.Remove(other);
            if (colliders.Count <= 0 )
            {
                mIsDeath = true;
                StartCoroutine(WaitRestart());
            }
        }
    }

    IEnumerator WaitRestart()
    {
        mAnimator.SetFloat("forward", 0);
        yield return new WaitForSeconds(0.5f);
        GameObject.Find("Canvas").transform.Find("GameOver").gameObject.SetActive(true);
        //SceneManager.LoadScene("Balance");
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        mIsDeath = false;
        colliders.Clear();
    }

    public void SetStatus(int status)
    {
        int cur = mStatus;
        if (cur == status)
        {
            mStatus = 0;
            mAnimator.Play("Idle_01");
            transform.GetChild(0).localRotation = Quaternion.identity;
            mAnimator.SetInteger("status", 0);
        } 
        else
        {
            mStatus = status;
            if (status == 2)
            {
                mAnimator.Play("Idle_Crouch_03");
                transform.GetChild(0).localRotation = Quaternion.identity;
            } 
            else if (status == 1)
            {
                mAnimator.Play("IP_Idle_Ege_01");
                transform.GetChild(0).localEulerAngles = Vector3.up * -90.0f;
            }
            mAnimator.SetInteger("status", status);
        }
    }

    public void Jump()
    {
        if (!isJump)
        {
            isJump = true;
            mAnimator.Play("Jump");
            isBeginJump = true;
        }
        //characterController.attachedRigidbody;
    }


    //private void Update()
    //{
    //    float _x = InputManager.Instance.Horizontal;
    //    float _y = InputManager.Instance.Vertical;
    //    Vector3 direction = Vector3.right * _x + Vector3.forward * _y;
    //    direction = direction.normalized;
    //    direction.y = 0;
    //    characterController.SimpleMove(direction * 0.3f);
    //    float distance = Vector2.Distance(InputManager.Instance.Direction, Vector2.zero);
    //    mAnimator.SetFloat("forward", distance);
    //    mAnimator.SetFloat("right", (Input.gyro.gravity - gyroInitialGravity).x);
    //}

}
