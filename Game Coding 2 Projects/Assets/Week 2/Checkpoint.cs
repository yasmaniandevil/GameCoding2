using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private PlatformPlayer playerScriptRB;

    // Start is called before the first frame update
    void Start()
    {
        playerScriptRB = GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformPlayer>();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //playerScriptRB.respawnPos = this.gameObject;
            GameManager.Instance.UpdateRespawnPoint(this.gameObject);
        }
    }
}
