using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Platform : MonoBehaviour
{
    public PlatformManager manager;
    Rigidbody2D rb;
    public Platform next;
    PhotonView view;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        view = GetComponent<PhotonView>();
    }
    private void Start()
    {
        manager = FindObjectOfType<PlatformManager>();
        rb.velocity = Vector2.up * -1;
    }

    public void SetSpeed(float speed)
    {
        rb.velocity = Vector2.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Vector3 aid = manager.CalculateNewPos();
            view.RPC("ResetPos", RpcTarget.All, aid);
        }
    }

    [PunRPC]
    private void ResetPos(Vector3 pos)
    {
        manager.SetPlatform(pos);
    }
}
