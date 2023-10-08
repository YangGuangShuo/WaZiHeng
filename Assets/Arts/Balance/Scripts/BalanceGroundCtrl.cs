using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GroundDirection
{
    Forward,
    LeftToRight,
    RightToLeft,
}

public enum GroundType
{
    Default,
    Ege,
}

public class BalanceGroundCtrl : MonoBehaviour
{
    public GroundDirection mGroundDirection = GroundDirection.Forward;

    public float mGroundLength = 0f;

    public GroundType mGroundType = GroundType.Default;
}
