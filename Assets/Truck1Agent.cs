using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class Truck1Agent : MyAgent
{
    private void Update()
    {
        /*RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
        {
            if (hit.transform.CompareTag("Route"))
            {
               // Debug.Log(hit.distance);
            }
        }*/
    }


    // Update is called once per frame
    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
        //Debug.Log("a");
        transform.localPosition = new Vector3(0, 0, 0);
    }

}
