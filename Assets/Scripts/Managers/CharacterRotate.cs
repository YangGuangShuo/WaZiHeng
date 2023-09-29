using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterRotate : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool _isDragging;
    private Vector3 _prevPosition;
    private float continuousDirection;
    [SerializeField]
    private Transform mPlayer;

    void Start()
    {
        
    }

    void Update()
    {
        if (!_isDragging) return;
        Vector2 mouseDelta = Input.mousePosition - _prevPosition;
        _prevPosition = Input.mousePosition;

        continuousDirection = -mouseDelta.x * 1;

        if (continuousDirection.Equals(0))
        {

        }
        else
        {
            InternalRotate(0, 10 * continuousDirection, 0);
        }
    }

    private void InternalRotate(float x, float y, float z)
    {
        mPlayer.Rotate(x * Time.deltaTime, y * Time.deltaTime, z * Time.deltaTime);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp");
        _isDragging = false;
        continuousDirection = 0;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        _prevPosition = Input.mousePosition;
        _isDragging = true;
    }
}
