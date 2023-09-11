using UnityEngine;
using Wazi;

public class PlayerBalanceCtrl : MonoBehaviour
{
    private Animator mAnimator;

    void Start()
    {
        mAnimator = gameObject.GetComponent<Animator>();
        InputManager.Instance.OpenGyroscope();
    }

    void Update()
    {
        float _x = InputManager.Instance.Horizontal;
        float _y = InputManager.Instance.Vertical;
        Vector3 direction = Vector3.right * _x + Vector3.forward * _y;
        float distance = Vector2.Distance(InputManager.Instance.Direction, Vector2.zero);
        mAnimator.SetFloat("forward", distance);
        direction = direction.normalized;
        direction.y = 0;
        mAnimator.SetFloat("right", _x);
        Debug.Log(InputManager.Instance.Gyroscope.rotationRate);
        if (direction.x != 0 || direction.y != 0)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 0.1f);
    }
}
