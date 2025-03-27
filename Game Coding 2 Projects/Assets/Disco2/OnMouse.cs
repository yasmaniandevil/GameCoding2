using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OnMouse : MonoBehaviour
{
    LayerMask mask;
    // Start is called before the first frame update
    void Start()
    {
        mask = LayerMask.NameToLayer("Click");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Debug.Log("mouse is down");
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition); //contrust a ray from current mouse cordinates
        //Raycast hit;
        //Raycast hit;
        /*if (Physics.Raycast(ray, hit))
        {
            if(hit.transform.gameObject.layer == mask)
            {
                Debug.Log("NPC");
            }
            else
            {
                Debug.Log("No Layer");
            }
        }*/


    }
}
