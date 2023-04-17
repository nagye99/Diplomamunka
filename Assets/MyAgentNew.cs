using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class MyAgentNew : Agent
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
        int numberOfChildren = transform.childCount;

        for (int i = 0; i < numberOfChildren; i++)
        {
            // The position of the agents
            sensor.AddObservation(transform.GetChild(i).localPosition);
        }

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
        //sensor.AddObservation(TargetTransform.localPosition.y);*/

        // The distance between the agent and the treasure
        //sensor.AddObservation(Vector3.Distance(TargetTransform.localPosition, transform.localPosition));
    }

    // Update is called once per frame
    public override void OnEpisodeBegin()
    {
        (GameObject Start, GameObject Finish) utvonal1 = (City1, City2);
        (GameObject Start, GameObject Finish) utvonal2 = (City1, City3);
        (GameObject Start, GameObject Finish) utvonal3 = (City1, City4);
        (GameObject Start, GameObject Finish) utvonal4 = (City1, City5);
        (GameObject Start, GameObject Finish) utvonal5 = (City2, City1);
        (GameObject Start, GameObject Finish) utvonal6 = (City2, City3);
        (GameObject Start, GameObject Finish) utvonal7 = (City2, City4);
        (GameObject Start, GameObject Finish) utvonal8 = (City2, City5);
        (GameObject Start, GameObject Finish) utvonal9 = (City3, City1);
        (GameObject Start, GameObject Finish) utvonal10 = (City3, City2);
        (GameObject Start, GameObject Finish) utvonal11 = (City3, City4);
        (GameObject Start, GameObject Finish) utvonal12 = (City3, City5);
        (GameObject Start, GameObject Finish) utvonal13 = (City4, City1);
        (GameObject Start, GameObject Finish) utvonal14 = (City4, City2);
        (GameObject Start, GameObject Finish) utvonal15 = (City4, City3);
        (GameObject Start, GameObject Finish) utvonal16 = (City4, City5);
        (GameObject Start, GameObject Finish) utvonal17 = (City5, City1);
        (GameObject Start, GameObject Finish) utvonal18 = (City5, City2);
        (GameObject Start, GameObject Finish) utvonal19 = (City5, City3);
        (GameObject Start, GameObject Finish) utvonal20 = (City5, City4);

        var Goal = new ArrayList(){
            utvonal1, utvonal1, utvonal1, utvonal1, utvonal1,
            utvonal2, utvonal2, utvonal5, utvonal5, utvonal5,
            utvonal7, utvonal7, utvonal8, utvonal9, utvonal9,
            utvonal9, utvonal9, utvonal12, utvonal13, utvonal13,
            utvonal13, utvonal14, utvonal15, utvonal15, utvonal16,
            utvonal17, utvonal19, utvonal19, utvonal19, utvonal20
        };
        //Debug.Log("a");
        //transform.localPosition = new Vector3(0, 0, 0);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int numberOfChildren = transform.childCount;

        var actionTaken = actions.ContinuousActions;
        //Debug.Log(actionTaken[0]);
        float[] actionSpeed = new float[3];
        float[] actionSteering = new float[3];

        for (int i = 0; i < numberOfChildren; i++)
        {
            actionSpeed[i] = (actionTaken[i*2] + 1) / 2; // [0, +1]
            actionSteering[i] = actionTaken[i*2+1];
        }
            float actionSpeed1 = (actionTaken[0] + 1) / 2; // [0, +1]
        float actionSteering1 = actionTaken[1]; // [-1, +1]

       /* if (actionSpeed[1] == 0)
        {
            AddReward(-10);
        }
        if (actionSpeed[2] == 0)
        {
            AddReward(-10);
        }
        if (actionSpeed[0] == 0)
        {
            AddReward(-10);
        }*/

        

        for (int i = 0; i < numberOfChildren; i++)
        {
            transform.GetChild(i).Translate(actionSpeed[i] * Vector3.forward * speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(new Vector3(0, actionSteering[i] * 180, 0));
        }
        //Debug.Log("speed: " + actionSpeed);
        //Debug.Log("actionSteering: " + actionSteering);
        //transform.Translate(actionSpeed * Vector3.forward * speed * Time.deltaTime);
        //transform.rotation = Quaternion.Euler(new Vector3(0, actionSteering * 180, 0));

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

