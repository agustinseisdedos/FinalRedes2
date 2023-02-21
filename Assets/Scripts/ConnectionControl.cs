using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ConnectionControl : MonoBehaviourPunCallbacks
{
    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(2);
    }
}
