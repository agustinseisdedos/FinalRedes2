using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    void Start()
    {
        PhotonNetwork.Instantiate("Player", transform.position, Quaternion.identity);
    }

}
