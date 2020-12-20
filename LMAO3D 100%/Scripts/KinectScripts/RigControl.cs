using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Threading;

public class RigControl : MonoBehaviour
{
    [DllImport("NtKinectDll")] private static extern System.IntPtr getKinect();
    [DllImport("NtKinectDll")] private static extern int setSkeleton(System.IntPtr kinect, System.IntPtr data, System.IntPtr state, System.IntPtr id);
    int bodyCount = 6;
    int jointCount = 25;
    private System.IntPtr kinect;

    public GameObject humanoid;
    public bool mirror = true;
    public bool move = true;
    public CharacterSkeleton skeleton;
    public CharacterSkeleton skeleton1;
    const byte CHANGE = 0;
    short condition = 1;

    // Start is called before the first frame update
    void Start()
    {
        kinect = getKinect();
        skeleton = new CharacterSkeleton(humanoid);
    }

    // Update is called once per frame
    void Update()
    {
        float[] data = new float[bodyCount * jointCount * 3];
        int[] state = new int[bodyCount * jointCount];
        int[] id = new int[bodyCount];
        GCHandle gch = GCHandle.Alloc(data, GCHandleType.Pinned);
        GCHandle gch2 = GCHandle.Alloc(state, GCHandleType.Pinned);
        GCHandle gch3 = GCHandle.Alloc(id, GCHandleType.Pinned);
        int n = setSkeleton(kinect, gch.AddrOfPinnedObject(), gch2.AddrOfPinnedObject(), gch3.AddrOfPinnedObject());
        gch.Free();
        gch2.Free();
        gch3.Free();
        if (n > 0)
        {
            if(PhotonNetwork.IsMasterClient)
                skeleton.set(data, state, 0, mirror, move);
            object[] datas = new object[] { data, state, 0, mirror, move };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent(CHANGE, datas, raiseEventOptions, SendOptions.SendUnreliable);
        }
    
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    private void NetworkingClient_EventReceived(EventData obj)
    {
        if(obj.Code == CHANGE)
        {
            object[] datas = (object[])obj.CustomData;
            float[] data = (float[])datas[0];
            int[] state = (int[])datas[1];
            bool mirror = (bool)datas[3];
            bool move = (bool)datas[4];
            if(condition == 1)
            {
                skeleton1 = new CharacterSkeleton(humanoid);
                skeleton1.set(data, state, 0, mirror, move);
                condition = 0;
            }
            else
                skeleton1.set(data, state, 0, mirror, move);
        }
    }

   
}
