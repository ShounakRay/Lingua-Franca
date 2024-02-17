using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objectives : MonoBehaviour
{   
    // The objectives are all stored in a box. get that box to count number of objectives in it.
    public GameObject objectiveBox;
    // Make array of objectives
    static ObjectiveData[] objectives;
    int numObjectives; 
    
    // Start is called before the first frame update
    void Start()
    {
        numObjectives = objectiveBox.transform.childCount;
        objectives = new ObjectiveData[numObjectives];

        for (int i = 0; i < numObjectives; i++)
        {
            objectives[i] = new ObjectiveData(objectiveBox.transform.GetChild(i).gameObject);
            objectives[i].objective.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        }  
    }

    // Update is called once per frame
    void Update()
    {
        // for every objective in the scene, check if it is completed
        for (int i = 0; i < numObjectives; i++)
        {
            // If the objective is completed, then make the image child of the objective visible. 
            if (objectives[i].completed)
            {
                objectives[i].objective.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            }
            if (Random.Range(0, 4000) == 1)
            {
                Debug.Log("Objective " + objectives[i].objective.name + " is completed");
                objectives[i].completed = true;
            }
        }
    }
}

// Create a struct to hold the objective data
public struct ObjectiveData
{
    public ObjectiveData(GameObject objective)
    {
        completed = false;
        objectiveName = objective.name;
        this.objective = objective;
    }
    public bool completed { get; set; }
    public string objectiveName { get; set; }
    public GameObject objective { get; set; }

}
