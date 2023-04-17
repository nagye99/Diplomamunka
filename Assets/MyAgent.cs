using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class MyAgent : Agent
{
    Rigidbody rigidBody;
    public float speed = 100;
    public GameObject City1;
    public GameObject City2;
    public GameObject City3;
    public GameObject City4;
    public GameObject City5;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        // The position of the agent
        sensor.AddObservation(transform.localPosition);

        sensor.AddObservation(City1.transform.localPosition.x);
        sensor.AddObservation(City1.transform.localPosition.z);

        sensor.AddObservation(City2.transform.localPosition.x);
        sensor.AddObservation(City2.transform.localPosition.z);

        sensor.AddObservation(City3.transform.localPosition.x);
        sensor.AddObservation(City3.transform.localPosition.z);

        sensor.AddObservation(City4.transform.localPosition.x);
        sensor.AddObservation(City4.transform.localPosition.z);

        sensor.AddObservation(City5.transform.localPosition.x);
        sensor.AddObservation(City5.transform.localPosition.z);
        // The position of the treasure prefab
        //sensor.AddObservation(TargetTransform.localPosition);
        //sensor.AddObservation(TargetTransform.localPosition.y);

        // The distance between the agent and the treasure
        //sensor.AddObservation(Vector3.Distance(TargetTransform.localPosition, transform.localPosition));
    }

    // Update is called once per frame
    public override void OnEpisodeBegin()
    {
        var Goal = new ArrayList();
        //Debug.Log("a");
        //transform.localPosition = new Vector3(0, 0, 0);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var actionTaken = actions.ContinuousActions;
        //Debug.Log(actionTaken[0]);

        float actionSpeed = (actionTaken[0] + 1) / 2; // [0, +1]
        float actionSteering = actionTaken[1]; // [-1, +1]

        if (actionSpeed == 0)
        {
            AddReward(-10);
        }
        //Debug.Log("speed: " + actionSpeed);
        //Debug.Log("actionSteering: " + actionSteering);
        transform.Translate(actionSpeed * Vector3.forward * speed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(new Vector3(0, actionSteering * 180, 0));

        AddReward(-0.01f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //Debug.Log("h");
        ActionSegment<float> actions = actionsOut.ContinuousActions;

        actions[0] = -1;
        actions[1] = 0;

        if (Input.GetKey("w"))
            actions[0] = 1;

        if (Input.GetKey("d"))
            actions[1] = +0.5f;

        if (Input.GetKey("a"))
            actions[1] = -0.5f;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("trigger: "+other.tag);
        if (other.tag == "Wall") {
            AddReward(-1000);
            EndEpisode();
        }
        if (other.tag == "City")
        {
            Debug.Log("Itt van");
            AddReward(5000);
        }
        if (other.tag == "Route")
        {
            //Debug.Log("hahj");
            AddReward(100);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Route")
        {
            //Debug.Log("ahj");
            AddReward(10);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Route")
        {
            //Debug.Log("jaj");
            AddReward(-100);
        }
    }
}
