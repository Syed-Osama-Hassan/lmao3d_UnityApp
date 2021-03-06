﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class TestConnect : MonoBehaviourPunCallbacks
{
    public InputField _name;

    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("Connecting to server");
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = _name.text;
        PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server.");
        Debug.Log(PhotonNetwork.LocalPlayer.NickName);
        if(!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from server for reason" + cause.ToString(), this);
    }

  
}
