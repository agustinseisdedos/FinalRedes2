using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Lobby : MonoBehaviourPunCallbacks
{
    public InputField createInputField, joinInputField;
    public MyServer serverPrefab;
    public PlayerFA playerPrefab;

    public void BtnCreateRoom()
    {
        RoomOptions options = new RoomOptions();

        options.MaxPlayers = 5;

        PhotonNetwork.CreateRoom(createInputField.text, options);
    }

    public void BtnJoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInputField.text);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room created");
        Debug.Log("Sos el servidor jeje");
        PhotonNetwork.Instantiate(serverPrefab.name, Vector3.zero, Quaternion.identity);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.PlayerList.Length >= 3)
        {
            Debug.Log("Mas de 4 jugadores");
            PhotonNetwork.LoadLevel("Juego");

        }
        else
        {
            Debug.Log("OPE Esperando otros jugadores");
        }
    }
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.PlayerList.Length >= 3)
        {
            Debug.Log("Mas de 5 jugadores");
            PhotonNetwork.LoadLevel("Juego");

        }
        else
        {
            Debug.Log(" OJR Esperando otros jugadores");
        }
        /*if (!PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
        }*/

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed join room " + returnCode + " Message: " + message);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed create room " + returnCode + " Message: " + message);
    }

    public void Quitear()
    {
        Application.Quit();
    }
}
