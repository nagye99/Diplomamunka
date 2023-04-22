using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using System;

public class TrucksAgent : Agent
{
    public int elteres = 0;
    static int sorszam = 1; //0;

    public GameObject City1;
    public GameObject City2;
    public GameObject City3;
    public GameObject City4;
    public GameObject City5;
    public GameObject Telephely;

    (int Start, int Finish) utvonal1;
    (int Start, int Finish) utvonal2;
    (int Start, int Finish) utvonal3;
    (int Start, int Finish) utvonal4;
    (int Start, int Finish) utvonal5;
    (int Start, int Finish) utvonal6;
    (int Start, int Finish) utvonal7;
    (int Start, int Finish) utvonal8;
    (int Start, int Finish) utvonal9;
    (int Start, int Finish) utvonal10;
    (int Start, int Finish) utvonal11;
    (int Start, int Finish) utvonal12;
    (int Start, int Finish) utvonal13;
    (int Start, int Finish) utvonal14;


    GameObject choosenCity;

    Vector3[] truckTranslation = { new Vector3(0, 0, 0), new Vector3(15, 0, 0), new Vector3(-15, 0, 0) };
    List<List<float>> distances = new List<List<float>>();

    private ArrayList cities;

    private int numberOfChildren;
    private List<int> currentPosition;

    //List<(GameObject, GameObject)> goals_old;
    List<(int, int)> goals_num = new List<(int, int)>();

    void Start()
    {
        cities = new ArrayList() { City1, City2, City3, City4, City5, Telephely };

        
        utvonal1 = (0, 3);
        utvonal2 = (0, 4);
        utvonal3 = (1, 2);
        utvonal4 = (1, 3);
        utvonal5 = (1, 4);
        utvonal6 = (2, 1);
        utvonal7 = (2, 4);
        utvonal8 = (3, 0);
        utvonal9 = (3, 1);
        utvonal10 = (3, 4);
        utvonal11 = (4, 0);
        utvonal12 = (4, 1);
        utvonal13 = (4, 2);
        utvonal14 = (4, 3);

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
            sensor.AddObservation(currentPosition[i]);
        }
        for (int i = 0; i < 6; i++)
        {
            sensor.AddObservation(distances[i]);
        }
        foreach (var goal in goals_num)
        {
            sensor.AddObservation(goal.Item1);
            sensor.AddObservation(goal.Item2);
        }
    }

    public override void OnEpisodeBegin()
    {
        goals_num = new List<(int, int)>();
        for (int i = 0; i < 15; i++)
        {
            goals_num.Add(GenerateSzallitmany());
        }

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
            choosenCity = (GameObject)cities[discreteActions[i]];
            var cityLocalPosition = choosenCity.transform.localPosition;
            transform.GetChild(i).localPosition = new Vector3(cityLocalPosition.x, cityLocalPosition.y, cityLocalPosition.z) + truckTranslation[i];

            CalculateReward(currentPosition[i], discreteActions[i]);

            currentPosition[i] = discreteActions[i];
        }

        if (goals_num.Count <= 0)
        {
            EndEpisode();
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
        for (int i = 0; i < 3; i++)
        {
            if (currentPosition[i] == 0)
            {
                actionMask.SetActionEnabled(i, 1, false);
                actionMask.SetActionEnabled(i, 2, false);
            }
            if (currentPosition[i] == 1)
            {
                actionMask.SetActionEnabled(i, 0, false);
            }
            if (currentPosition[i] == 2)
            {
                actionMask.SetActionEnabled(i, 0, false);
                actionMask.SetActionEnabled(i, 3, false);
            }
            if (currentPosition[i] == 3)
            {
                actionMask.SetActionEnabled(i, 2, false);
            }
        }
    }

    private void CalculateReward(int start, int arrival)
    {
        AddReward(-1 * distances[start][arrival]);

        var utvonal = (start, arrival);

        if (start == arrival)
        {
            AddReward(-100);
        }

        if (goals_num.Contains(utvonal))
        {
            AddReward(1000);
            goals_num.Remove(utvonal);
            goals_num.Add(GenerateSzallitmany());
        }
    }

    private (int, int) GenerateSzallitmany()
    {
        var rnd = new System.Random();
        //if (elteres == 0 || 15 - elteres > sorszam)
        if (elteres == 0 || sorszam % elteres != 0)
        {
            sorszam++;
            if (elteres != 0) {
                sorszam = sorszam % elteres;
            }

            var valasztas = rnd.Next(100);

            if (valasztas < 9)
            {
                return utvonal1;
            }
            else if (valasztas < 16)
            {
                return utvonal2;
            }
            else if (valasztas < 29)
            {
                return utvonal3;
            }
            else if (valasztas < 32)
            {
                return utvonal4;
            }
            else if (valasztas < 41)
            {
                return utvonal5;
            }
            else if (valasztas < 46)
            {
                return utvonal6;
            }
            else if (valasztas < 51)
            {
                return utvonal7;
            }
            else if (valasztas < 68)
            {
                return utvonal8;
            }
            else if (valasztas < 72)
            {
                return utvonal9;
            }
            else if (valasztas < 77)
            {
                return utvonal10;
            }
            else if (valasztas < 86)
            {
                return utvonal11;
            }
            else if (valasztas < 90)
            {
                return utvonal12;
            }
            else if (valasztas < 94)
            {
                return utvonal13;
            }
            else
            {
                return utvonal14;
            }
        }
        else
        {
            /*sorszam++;
            sorszam = sorszam % 15;*/
            sorszam = 1;
            var valasztott = rnd.Next(14);

            switch (valasztott)
            {
                case 0:
                    return utvonal1;
                case 1:
                    return utvonal2;
                case 2:
                    return utvonal3;
                case 3:
                    return utvonal4;
                case 4:
                    return utvonal5;
                case 5:
                    return utvonal6;
                case 6:
                    return utvonal7;
                case 7:
                    return utvonal8;
                case 8:
                    return utvonal9;
                case 9:
                    return utvonal10;
                case 10:
                    return utvonal11;
                case 11:
                    return utvonal12;
                case 12:
                    return utvonal13;
                case 13:
                    return utvonal14;
                default:
                    return utvonal14;
            }
        }
    }
}
