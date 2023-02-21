using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class MyServer : MonoBehaviourPunCallbacks
{
    public static MyServer Instance;

    Player _server;

    public PlayerFA characterPrefab;

    Dictionary<Player, PlayerFA> _dicModels = new Dictionary<Player, PlayerFA>();
    public int PackagesPerSecond { get; private set; }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        if (Instance == null)
        {
            if (photonView.IsMine)
            {
                photonView.RPC("SetServer", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer);
            }
        }
    }

    [PunRPC]
    void SetServer(Player serverPlayer)
    {
        if (Instance)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        _server = serverPlayer;

        PackagesPerSecond = 60;

        var playerLocal = PhotonNetwork.LocalPlayer;
    }

    // pun rpcs

    public void RequestIdle(Player player)
    {
        photonView.RPC("Idle", _server, player);
    }

    public void RequestMove(Player player, float horizontal)
    {
        photonView.RPC("Move", _server, player, horizontal);
    }

    public void RequestJump(Player player)
    {
        photonView.RPC("Jump", _server, player);
    }

    public void PlayerFADisconnect(PlayerFA playerfa)
    {
        var playerToDisconnect = _dicModels.FirstOrDefault(x => x.Value.Equals(playerfa)).Key;

        if (!_dicModels.ContainsKey(playerToDisconnect)) return;

        photonView.RPC("DisconnectPlayer", playerToDisconnect);

        Destroy(_dicModels[playerToDisconnect].gameObject);
        _dicModels.Remove(playerToDisconnect);


    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (_dicModels.ContainsKey(otherPlayer))
        {
            Destroy(_dicModels[otherPlayer].gameObject);
            _dicModels.Remove(otherPlayer);
        }
        if (_dicModels.Count > 1) return;
        var winningPlayer = _dicModels.FirstOrDefault().Key;
        photonView.RPC("GoToWinScreen", winningPlayer);

    }


    /* FUNCIONES DEL SERVER ORIGINAL QUE LE LLEGAN DEL AVATAR */

    [PunRPC]
    void GoToWinScreen()
    {
        Debug.Log("GO WIN");
        PhotonNetwork.LoadLevel(3);
    }

    [PunRPC]
    void DisconnectPlayer()
    {
        Debug.Log("PUN LEAVE ROOM");
        PhotonNetwork.LeaveRoom();
    }

    [PunRPC]
    void Move(Player player, float horizontal)
    {
        if (_dicModels.ContainsKey(player))
        {
            _dicModels[player].Move(horizontal);
        }
    }

    [PunRPC]
    void Jump(Player player)
    {
        if (_dicModels.ContainsKey(player))
        {
            _dicModels[player].Jump();
        }
    }

    [PunRPC]
    void Idle(Player player)
    {
        if (_dicModels.ContainsKey(player))
        {
            _dicModels[player].Idle();
        }
    }

    [PunRPC]
    void ChangePlayersColours()
    {
        Debug.Log("CHANGE PLAYERS COLOURS");
        var list = new List<Color> { Color.red, Color.blue, Color.green, Color.yellow };
        for (int i = 0; i < _dicModels.Count; i++)
        {
            _dicModels.ElementAt(i).Value.GetComponent<SpriteRenderer>().color = list[i];
            _dicModels.ElementAt(i).Value.name = "player" + i + 1;
            _dicModels.ElementAt(i).Value.RPCChangeColor(list[i].r, list[i].g, list[i].b);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level != 1) return;
        if (!PhotonNetwork.IsMasterClient) return;

        foreach (var item in PhotonNetwork.PlayerList)
        {
            if (item == PhotonNetwork.LocalPlayer) continue;
            var newPlayer = PhotonNetwork.Instantiate(characterPrefab.name, Vector3.zero, Quaternion.identity);
            _dicModels.Add(item, newPlayer.GetComponent<PlayerFA>());
        }
        Debug.Log("aca deberia correr el pun rpc");
        photonView.RPC("ChangePlayersColours", RpcTarget.AllBuffered);
    }
}
