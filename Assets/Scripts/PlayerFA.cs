using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class PlayerFA : MonoBehaviourPunCallbacks
{
    PhotonView _myView;
    public float potencia;
    public float speed;
    Rigidbody2D rb;
    Animator anim;
    public bool canJump;

    void Start()
    {
        _myView = GetComponent<PhotonView>();
        anim = gameObject.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            Debug.Log("TRATO DE MOVERME");
            MyServer.Instance.RequestMove(PhotonNetwork.LocalPlayer, Input.GetAxis("Horizontal"));
        }
        else
        {
            MyServer.Instance.RequestIdle(PhotonNetwork.LocalPlayer);
        }
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            Debug.Log("TRATO DE SALTAR");
            MyServer.Instance.RequestJump(PhotonNetwork.LocalPlayer);
        }
    }

    public void Move(float horizontal)
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        if (horizontal != 0)
        {
            anim.SetBool("IsRunning", true);
        }
        if (horizontal == 0)
        {
            anim.SetBool("IsRunning", false);
        }

        if (horizontal < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            _myView.RPC("RPCCFLipSprite", RpcTarget.Others, true);

        }
        else if (horizontal > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            _myView.RPC("RPCCFLipSprite", RpcTarget.Others, false);
        }
    }

    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, potencia);
        canJump = false;
        anim.SetTrigger("Jump");
    }

    public PlayerFA SetInitialParameters(Player localPlayer)
    {
        photonView.RPC("SetLocalParams", localPlayer);
        return this;
    }

    public void Idle()
    {
        anim.SetBool("IsRunning", false);
    }
    public override void OnLeftLobby()
    {
        Debug.Log("LEFT LOBBY");
    }
    public override void OnLeftRoom()
    {
        Debug.Log("LEFT ROOM");
        if (!PhotonNetwork.IsMasterClient) SceneManager.LoadScene("Lose");
    }

    [PunRPC]
    void SetLocalParams()
    {
        GetComponent<SpriteRenderer>().color = Color.green;
    }

    public void RPCChangeColor(float r, float g, float b)
    {
        photonView.RPC("RPCChangeColor2", RpcTarget.AllBuffered, r, g, b);
    }

    [PunRPC]
    public void RPCChangeColor2(float r, float g, float b)
    {
        GetComponent<SpriteRenderer>().color = new Color(r, g, b, 1);
    }

    [PunRPC]
    public void RPCCFLipSprite(bool orientation)
    {
        GetComponent<SpriteRenderer>().flipX = orientation;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("murio " + this.name);
        if (!PhotonNetwork.IsMasterClient) return;
        MyServer.Instance.PlayerFADisconnect(this);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (otherPlayer != PhotonNetwork.LocalPlayer) return;
        if (PhotonNetwork.PlayerList.Length == 2)
        {
            SceneManager.LoadScene("Win");
        }
        Debug.Log("alguien se cayo");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Floor")
        {
            canJump = true;
        }
    }
}
