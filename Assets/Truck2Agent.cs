using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class Truck2Agent : MyAgent
{

    // Update is called once per frame
    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
        //Debug.Log("a");
        transform.localPosition = new Vector3(-30, 0, 0);
    }

}
