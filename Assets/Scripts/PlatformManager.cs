using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public class PlatformManager : MonoBehaviourPunCallbacks
{
    public Platform platformPrefab;
    public int totalPlatforms = 10;
    public List<Platform> platforms;
    PhotonView view;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        SpawnPlatforms(totalPlatforms);
        SetFirstPlatform();
        for (int i = 0; i < platforms.Count - 1; i++)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Vector3 aid = CalculateNewPos();
                view.RPC("SetPlatform", RpcTarget.All, aid);
            }
        }
    }


    //public void CreatePlatforms()
    //{
    //    platforms = new List<Platform>();

    //    for (int i = 0; i < totalPlatforms; i++)
    //    {
    //        var current = PhotonNetwork.InstantiateRoomObject("platform", transform.position, Quaternion.identity);
    //        platforms.Add(current.GetComponent<Platform>());
    //    }
    //}

    [PunRPC]
    public void SetPlatform(Vector3 newPos)
    {
        //float minPosX = Mathf.Clamp(platforms[platforms.Count - 1].transform.position.x - 4.2f, -6.38f, 6.26f);
        //float maxPosX = Mathf.Clamp(platforms[platforms.Count - 1].transform.position.x + 4.2f, -6.38f, 6.26f);

        //float posX = Random.Range(minPosX, maxPosX);
        //float posY = platforms[platforms.Count - 1].transform.position.y + 2.5f;

        if (!view.IsMine) return;

        var current = platforms[0];
        current.transform.position = newPos;
        platforms.RemoveAt(0);
        platforms.Add(current);
    }

    
    public Vector3 CalculateNewPos()
    {
        float minPosX = Mathf.Clamp(platforms[platforms.Count - 1].transform.position.x - 4.2f, -6.38f, 6.26f);
        float maxPosX = Mathf.Clamp(platforms[platforms.Count - 1].transform.position.x + 4.2f, -6.38f, 6.26f);

        float posX = Random.Range(minPosX, maxPosX);
        float posY = platforms[platforms.Count - 1].transform.position.y + 2.5f;

        return new Vector3(posX, posY, 0);
    }

    public void SetFirstPlatform()
    {
        if (!view.IsMine) return;
        var current = platforms[0];
        current.transform.position = new Vector3(0, -3.37f, 0);
        platforms.RemoveAt(0);
        platforms.Add(current);
    }

    [PunRPC]
    void SpawnPlatforms(int amount)
    {
        platforms = new List<Platform>();
        
        for (int i = 0; i < amount; i++)
        {
            if (view.IsMine)
            {

                var current = PhotonNetwork.Instantiate("platform", transform.position, Quaternion.identity);
                platforms.Add(current.GetComponent<Platform>());
            }
        }
    }
}
