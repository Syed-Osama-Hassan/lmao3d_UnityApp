using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MotionsTesting : MonoBehaviour
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
            skeleton.set(data, state, 0, mirror, move);
        }

    }

}
