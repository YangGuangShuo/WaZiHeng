using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtroller : MonoBehaviour
{
    [SerializeField]
    private Transform mPlayer;

    private Vector3 mDeltaOffset;

    public void Init(Transform player)
    {
        mPlayer = player;
        mDeltaOffset = transform.position - player.position;
    }

    void LateUpdate()
    {
        if (!mPlayer) return;
        Vector3 targetPosition = mPlayer.position;// + mPlayer.TransformDirection(mDeltaOffset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 3);
    }
}
