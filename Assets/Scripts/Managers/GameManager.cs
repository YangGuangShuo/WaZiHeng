using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Transform mSelfPlayer;
    public Transform mPlayersRoot;
    public GameObject mPlayerPrefab;
    public Text mTextCount;

    public GameObject Bones;
    public SkinnedMeshRenderer mRendererPart;
    public SkinnedMeshRenderer mRendererTarget;
    public SkinnedMeshRenderer mRendererPart1;
    public SkinnedMeshRenderer mRendererTarget1;

    public Transform mCamera;

    public GameObject mBusImg;

    public Material mTextGunDong;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    // Start is called before the first frame update
    void Start()
    {
        //UpdateCount();
        //ChangePart(Bones, mRendererPart, mRendererTarget);
        //ChangePart(Bones, mRendererPart1, mRendererTarget1);
    }

    // Update is called once per frame
    float x = 1;
    void Update()
    {
        if (mTextGunDong)
        {
            x += Time.deltaTime;
            mTextGunDong.SetTextureOffset("_BaseMap", new Vector2(x, 1));
        }
    }

    public void EnterMain()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }

    public void UpdatePlayer(string args)
    {
        if (args == "add")
        {
            for (int i = 0; i < mPlayersRoot.childCount; i++)
            {
                if (mPlayersRoot.GetChild(i).gameObject.activeSelf == false)
                {
                    mPlayersRoot.GetChild(i).gameObject.SetActive(true);
                    this.UpdateCount();
                    return;
                }
            }

            GameObject player = Instantiate(mPlayerPrefab);
            player.transform.SetParent(mPlayersRoot);
            player.transform.position = GetPlayerRandomPos();
            player.SetActive(true);
        } 
        else if (args == "sub")
        {
            if (mPlayersRoot.childCount > 0)
            {
                for (int i = mPlayersRoot.childCount - 1; i >= 0; i--)
                {
                    if (mPlayersRoot.GetChild(i).gameObject.activeSelf == true)
                    {
                        mPlayersRoot.GetChild(i).gameObject.SetActive(false);
                        this.UpdateCount();
                        return;
                    }
                }
            }
        }
        else if (args == "clear")
        {
            for (int i = 0; i < mPlayersRoot.childCount; i++)
            {
                mPlayersRoot.GetChild(i).gameObject.SetActive(false);
            }
        }
        UpdateCount();
    }

    private void UpdateCount()
    {
        int count = 1;
        int meshCount = GetMeshTriangles(mSelfPlayer);
        for (int i = 0; i < mPlayersRoot.childCount; i++)
        {
            if (mPlayersRoot.GetChild(i).gameObject.activeSelf == true)
            {
                meshCount += GetMeshTriangles(mPlayersRoot.GetChild(i));
                count++;
            }
        }
        mTextCount.text = "数量=" + count.ToString() + "/面数=" + meshCount;
    }

    private int GetMeshTriangles(Transform player)
    {
        int meshCount = 0;
        //Transform meshes = player.Find("Meshes");
        for (int j = 0; j < player.childCount; j++)
        {
            SkinnedMeshRenderer skinned = player.GetChild(j).GetComponent<SkinnedMeshRenderer>();
            if (skinned != null)
                meshCount += skinned.sharedMesh.triangles.Length / 3;
            else
            {
                MeshFilter meshFilter = player.GetChild(j).GetComponent<MeshFilter>();
                if (meshFilter != null)
                {
                    meshCount += meshFilter.mesh.triangles.Length / 3;
                }
            }
            
        }
        return meshCount;
    }

    private Vector3 GetPlayerRandomPos()
    {
        float posX = Random.Range(-2.5f, 2.5f);
        float posZ = Random.Range(2.0f, -0.5f) - 3.0f;
        float posY = 0;
        //Debug.Log("posX " + posX);
        //Debug.Log("posZ " + posZ);
        return new Vector3(posX, posY, posZ);
    }

    private void ChangePart(GameObject bonesRoot, SkinnedMeshRenderer partSkm, SkinnedMeshRenderer targetSkm)
    {
        List<Transform> bones = new List<Transform>();
        Transform rootBone = null;
        foreach (var trans in targetSkm.bones)
        {
            foreach (var bone in bonesRoot.GetComponentsInChildren<Transform>())
            {
                if (bone.name == trans.name)
                {
                    bones.Add(bone);
                    if (targetSkm.rootBone.name == bone.name)
                    {
                        rootBone = bone;
                    }
                    break;
                }
            }
        }
        //partSkm.sharedMesh = targetSkm.sharedMesh;
        partSkm.bones = bones.ToArray();
        //partSkm.sharedMaterial = targetSkm.sharedMaterial;
        partSkm.rootBone = rootBone;
        //partSkm.localBounds = targetSkm.localBounds;
    }

    public void CameraUpdate(float value)
    {
        float min = -10.0f;
        float v = value * (min + 4.5f) - 4.5f;
        mCamera.localPosition = new Vector3(0, 1, Mathf.Clamp(v, min, -4.5f));
    }

    public void OnBusBtn()
    {
        mBusImg.SetActive(!mBusImg.activeSelf);
        UnityEngine.SceneManagement.SceneManager.LoadScene("ActionBus");
    }
}
