using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTracker : MonoBehaviour
{
   public  Transform targetTr;

    void Update()
    {
        transform.position = targetTr.position;
        if(transform.position.x>0.42)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
            transform.GetChild(0).GetChild(0).GetChild(1).localRotation = new Quaternion(0, 180, 0, 0);
        }
        else if(transform.position.x<-0.42)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
            transform.GetChild(0).GetChild(0).GetChild(1).localRotation = new Quaternion(0, 0, 0, 0);
        }
    }
}
