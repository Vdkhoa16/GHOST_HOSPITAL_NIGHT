using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{

    public int keyIDs;  // Lưu trữ ID chìa khóa đã có

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void UseKey(int keyID)
    {

    }

    public bool HasKey(int keyID)
    {
        return keyIDs == keyID;
    }
}
