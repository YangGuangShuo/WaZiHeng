using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YunShiAppear : MonoBehaviour
{
    public Transform YunShiParent;

    public GameObject[] mYunShi;

    public float AppearTime = 5;

    public Vector3 Direction;

    private float time;

    private List<Transform> list = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        Direction.y = Random.Range(-0.5f, 0.5f);
        Direction.x =  Direction.x > 0 ? Random.Range(0.5f, Direction.x) : Random.Range(Direction.x, -0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > AppearTime)
        {
            time = Random.Range(3, 6);
            Appear();
        }

        if (list.Count > 0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Translate(Direction * Time.deltaTime);
            }
        }
    }

    public void Appear()
    {
        int random = Random.Range(0, 3);
        GameObject go = Instantiate(mYunShi[random]);
        go.transform.SetParent(YunShiParent);
        go.transform.localPosition = Vector3.zero;
        list.Add(go.transform);
    }
}
