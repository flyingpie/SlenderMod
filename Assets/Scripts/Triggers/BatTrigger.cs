using UnityEngine;
using System.Collections;

public class BatTrigger : MonoBehaviour
{
	private void Start()
    {
	
	}
	
	private void Update()
    {
	
	}

    private void OnTriggerEnter()
    {
        //Debug.Log("OnTriggerEnter()");

        BatController.instance.DoBatTrigger(transform.position);
    }
}
