using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class TrucksAgent : Agent
{
    public GameObject City1;
    public GameObject City2;
    public GameObject City3;
    public GameObject City4;
    public GameObject City5;
    public GameObject Telephely;

    (GameObject Start, GameObject Finish) utvonal1;
    (GameObject Start, GameObject Finish) utvonal2;
    (GameObject Start, GameObject Finish) utvonal3;
    (GameObject Start, GameObject Finish) utvonal4;
    (GameObject Start, GameObject Finish) utvonal5;
    (GameObject Start, GameObject Finish) utvonal6;
    (GameObject Start, GameObject Finish) utvonal7;
    (GameObject Start, GameObject Finish) utvonal8;
    (GameObject Start, GameObject Finish) utvonal9;
    (GameObject Start, GameObject Finish) utvonal10;
    (GameObject Start, GameObject Finish) utvonal11;
    (GameObject Start, GameObject Finish) utvonal12;
    (GameObject Start, GameObject Finish) utvonal13;
    (GameObject Start, GameObject Finish) utvonal14;
    (GameObject Start, GameObject Finish) utvonal15;
    (GameObject Start, GameObject Finish) utvonal16;
    (GameObject Start, GameObject Finish) utvonal17;
    (GameObject Start, GameObject Finish) utvonal18;
    (GameObject Start, GameObject Finish) utvonal19;
    (GameObject Start, GameObject Finish) utvonal20;

    GameObject choosenCity;

    Vector3[] truckTranslation = { new Vector3(0, 0, 0), new Vector3(15, 0, 0), new Vector3(-15, 0, 0) };
    List<List<float>> distances = new List<List<float>>();

    private ArrayList cities;

    private int numberOfChildren;
    private List<int> currentPosition;

    List<(GameObject, GameObject)> Goal;

    // Start is called before the first frame update
    void Start()
    {
        cities = new ArrayList() { City1, City2, City3, City4, City5, Telephely };

        utvonal1 = (City1, City2);
        utvonal2 = (City1, City3);
        utvonal3 = (City1, City4);
        utvonal4 = (City1, City5);
        utvonal5 = (City2, City1);
        utvonal6 = (City2, City3);
        utvonal7 = (City2, City4);
        utvonal8 = (City2, City5);
        utvonal9 = (City3, City1);
        utvonal10 = (City3, City2);
        utvonal11 = (City3, City4);
        utvonal12 = (City3, City5);
        utvonal13 = (City4, City1);
        utvonal14 = (City4, City2);
        utvonal15 = (City4, City3);
        utvonal16 = (City4, City5);
        utvonal17 = (City5, City1);
        utvonal18 = (City5, City2);
        utvonal19 = (City5, City3);
        utvonal20 = (City5, City4);

        for (int i = 0; i < 6; i++)
        {
            var varosonkent = new List<float>();
            for (int j = 0; j < 6; j++)
            {
                GameObject indulasi = (GameObject)cities[i];
                GameObject erkezesi = (GameObject)cities[j];
                varosonkent.Add(Vector3.Distance(indulasi.transform.localPosition, erkezesi.transform.localPosition));
            }
            distances.Add(varosonkent);
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        numberOfChildren = transform.childCount;

        for (int i = 0; i < numberOfChildren; i++)
        {
            // The position of the agents
            sensor.AddObservation(transform.GetChild(i).localPosition);
        }
        for (int i = 0; i < 6; i++)
        {
            sensor.AddObservation(distances[i]);
        }
    }

    public override void OnEpisodeBegin()
    {
        Goal = new List<(GameObject, GameObject)>()
        {
            utvonal1, utvonal1, utvonal1, utvonal1, utvonal1,
            utvonal2, utvonal2, utvonal5, utvonal5, utvonal5,
            utvonal7, utvonal7, utvonal8, utvonal9, utvonal9,
            utvonal9, utvonal9, utvonal12, utvonal13, utvonal13,
            utvonal13, utvonal14, utvonal15, utvonal15, utvonal16,
            utvonal17, utvonal19, utvonal19, utvonal19, utvonal20
        };
        //Debug.Log("a");
        transform.GetChild(0).localPosition = new Vector3(0, 0, 0);
        transform.GetChild(1).localPosition = new Vector3(30, 0, 0);
        transform.GetChild(2).localPosition = new Vector3(-30, 0, 0);
        currentPosition = new List<int>() { 5, 5, 5 };
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        numberOfChildren = transform.childCount;

        var discreteActions = actions.DiscreteActions;

        for (int i = 0; i < numberOfChildren; i++) 
        {
            //Debug.Log(discreteActions[i]);

            choosenCity = (GameObject)cities[discreteActions[i]];
            var cityLocalPosition = choosenCity.transform.localPosition;
            transform.GetChild(i).localPosition = new Vector3(cityLocalPosition.x, cityLocalPosition.y, cityLocalPosition.z) + truckTranslation[i];

            CalculateReward(currentPosition[i], discreteActions[i]);

            currentPosition[i] = discreteActions[i];
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;
        for (int i = 0; i < discreteActions.Length; i++)
        {
            discreteActions[i] = 5;
        }

        if (Input.GetKey("a"))
        {
            discreteActions[0] = 0;
        }
        if (Input.GetKey("b"))
        {
            discreteActions[0] = 1;
        }
        if (Input.GetKey("c"))
        {
            discreteActions[0] = 2;
        }
        if (Input.GetKey("d"))
        {
            discreteActions[0] = 3;
        }
        if (Input.GetKey("e"))
        {
            discreteActions[0] = 4;
        }
    }

    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
    }

    public void CalculateReward(int start, int arrival)
    {
        //Debug.Log(start + " " + arrival);
        //Debug.Log(-1*distances[start][arrival]);
        AddReward(-1 * distances[start][arrival]);

        var utvonal = ((GameObject)cities[start], (GameObject)cities[arrival]);

        if (Goal.Count > 0 && arrival == 5)
        {
            //Debug.Log(-100);
            AddReward(-100);
        }

        if (start == arrival)
        {

            //Debug.Log(-10);
            AddReward(-10000);
        }

        if (Goal.Contains(utvonal))
        {

            //Debug.Log(750);
            AddReward(1000);
            Goal.Remove(utvonal);
        }

        if (Goal.Count <= 0)
        {
            EndEpisode();
        }
    }
}
