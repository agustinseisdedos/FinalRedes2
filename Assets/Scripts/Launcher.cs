using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    public GameObject mainScreen, connectedScreen;


    public void BtnConnect()
    {
        PhotonNetwork.ConnectUsingSettings(); //Connecta a Photon como esta configurado en PhotonServerSettings
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(); // Estan en el Master Server, te joinea al default Lobby
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Connection failed: " + cause.ToString());
    }

    public override void OnJoinedLobby()
    {
        if (!mainScreen) return;
        if (!connectedScreen) return;
        mainScreen.SetActive(false);
        connectedScreen.SetActive(true);
    }

    public void goMenu()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("Lobby");
    }

}
