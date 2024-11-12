using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEvents : MonoBehaviour
{
    public GameObject lance;
    private BoxCollider attackCollider;
    void Start()
    {
        attackCollider = lance.GetComponent<BoxCollider>();
    }

   public void StartAttack(){
        attackCollider.enabled = true;
    }
    public void EndAttack(){
        attackCollider.enabled = false;
    }
}
