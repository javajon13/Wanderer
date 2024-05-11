using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTrigger : MonoBehaviour
{
    public bool isTriggered;
    public string collisionTag;

    private int collidedObjects = 0;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == collisionTag)
        {
            collidedObjects += 1;
            isTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == collisionTag)
        {
            collidedObjects -= 1;
            if(collidedObjects <= 0)
            {
                isTriggered = false;
            }
        }
    }
}
