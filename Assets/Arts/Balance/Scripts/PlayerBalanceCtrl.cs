

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

    public BalanceCtrl mBalanceCtrl;

    private BalanceGroundCtrl mBalanceGroundCtrl;

    private bool mIsRuning = false;

    void Start()
    {
        mAnimator = gameObject.GetComponent<Animator>();
        Input.gyro.enabled = true;
        gyroInitialGravity = Input.gyro.gravity;
        characterController = gameObject.GetComponent<CharacterController>();
        mIsRuning = false;
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
        distance = Mathf.Clamp(distance, 0, mIsRuning ? 1 : 0.9f);
        mAnimator.SetFloat("forward", distance);
        direction = direction.normalized;
        mAnimator.SetFloat("right", (Input.gyro.gravity - gyroInitialGravity).x);
        if ((direction.x != 0 || direction.y != 0) && mStatus != 1)
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
        if (isJump)
        {
            if (jumpTimeFlag < jumpTime)
            {
                Vector3 temp = (transform.up + transform.forward * 2) * jumpSpeed * Time.deltaTime;
                characterController.Move(temp);
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
        Vector3 move = direction * mSpeed * Time.deltaTime;
        characterController.Move(move);
        IsMirror(direction);
        CheckFade();
        CheckDistance();
    }

    private void CheckDeath()
    {
        if (transform.position.y <= -0.5f && !mIsDeath)
        {
            mBalanceCtrl.Death();
            mIsDeath = true;
            StartCoroutine(WaitRestart());
        }
    }

    private void IsMirror(Vector3 direction)
    {
        if (mAnimator.GetCurrentAnimatorStateInfo(0).IsName("IP_Loop_Walk_Edge_Right_02") || mAnimator.GetCurrentAnimatorStateInfo(0).IsName("IP_Loop_Run_Right_Edge_01"))
        {
            if (mBalanceGroundCtrl != null)
            {
                if (mBalanceGroundCtrl.mGroundDirection == GroundDirection.Forward)
                {
                    if (direction.z < 0)
                    {
                        mAnimator.SetBool("isMirror", false);
                    }
                    else
                    {
                        mAnimator.SetBool("isMirror", true);
                    }
                } 
                else if (mBalanceGroundCtrl.mGroundDirection == GroundDirection.LeftToRight || mBalanceGroundCtrl.mGroundDirection == GroundDirection.RightToLeft)
                {
                    if (direction.x < 0)
                    {
                        mAnimator.SetBool("isMirror", false);
                    }
                    else
                    {
                        mAnimator.SetBool("isMirror", true);
                    }
                }
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
                SetMaterialsColor(_renderer, 0.2f);
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

    private Vector3 mCheckDistance = Vector3.zero;
    private void CheckDistance()
    {
        if (mBalanceGroundCtrl != null)
        {
            if (mBalanceGroundCtrl.mGroundDirection == GroundDirection.Forward)
            {
                float dis = transform.position.z - mCheckDistance.z; 
                mBalanceCtrl.Move(dis);
            } 
            else if (mBalanceGroundCtrl.mGroundDirection == GroundDirection.LeftToRight)
            {
                float dis = transform.position.x - mCheckDistance.x;
                mBalanceCtrl.Move(dis);
            }
            else if (mBalanceGroundCtrl.mGroundDirection == GroundDirection.RightToLeft)
            {
                float dis = transform.position.x - mCheckDistance.x;
                mBalanceCtrl.Move(-dis);
            }
            mCheckDistance = transform.position;
        } 
        else
        {
            mCheckDistance = Vector3.zero;
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
            _renderer.materials[i].SetColor("_BaseColor", color);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") && other.gameObject.name != "Start")
        {
            if (other.gameObject.name == "End")
            {
                Debug.Log("胜利");
            }
            //colliders.Add(other);
            BalanceGroundCtrl ctrl = other.GetComponentInParent<BalanceGroundCtrl>();
            if (ctrl != null)
            {
                Debug.Log(ctrl.mGroundLength);
                mBalanceGroundCtrl = ctrl;
            }
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Coin"))
        {
            other.gameObject.SetActive(false);
            SoundManager.Instance.EatCoin();
            mBalanceCtrl.EatCoin();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log(colliders.Count);
        //if (other.gameObject.layer == LayerMask.NameToLayer("Ground") && mIsDeath == false)
        //{
        //    colliders.Remove(other);
        //    if (colliders.Count <= 0 && !mIsDeath)
        //    {
        //        mIsDeath = true;
        //        StartCoroutine(WaitRestart());
        //    }
        //}
    }

    IEnumerator WaitRestart()
    {
        mAnimator.SetFloat("forward", 0);
        mBalanceGroundCtrl = null;
        yield return new WaitForSeconds(0.5f);
        GameObject.Find("Canvas").transform.Find("GameOver").gameObject.SetActive(true);
        //SceneManager.LoadScene("Balance");
        transform.position = Vector3.up * 2;
        transform.rotation = Quaternion.identity;
        yield return new WaitForSeconds(0.5f);
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
                if (mBalanceGroundCtrl != null)
                {
                    if (mBalanceGroundCtrl.mGroundDirection == GroundDirection.Forward)
                    {
                        transform.localEulerAngles = Vector3.zero;
                        transform.GetChild(0).localEulerAngles = Vector3.up * 90.0f;
                    }
                    else if (mBalanceGroundCtrl.mGroundDirection == GroundDirection.RightToLeft)
                    {
                        transform.localEulerAngles = Vector3.up * -90.0f;
                        transform.GetChild(0).localEulerAngles = Vector3.up * -90.0f;
                    }
                    else if (mBalanceGroundCtrl.mGroundDirection == GroundDirection.LeftToRight)
                    {
                        transform.localEulerAngles = Vector3.up * 90.0f;
                        transform.GetChild(0).localEulerAngles = Vector3.up * 90.0f;
                    }
                }
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
    }

    public void Run(string args)
    {
        mIsRuning = args == "1" ? true : false;
    }
}
