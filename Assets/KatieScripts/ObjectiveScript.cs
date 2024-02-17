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
    public int numCompleted = 0;
    
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
        // for every objective in the scene, randomly complete it. 
        for (int i = 0; i < numObjectives; i++)
        {
            if (Random.Range(0, 2000) == 1)
            {
                Debug.Log("Objective " + objectives[i].objective.name + " is completed");
                objectives[i].Complete();
            }
        }
    }
}

// Create a struct to hold the objective data
public class ObjectiveData
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

    public void Complete()
    {
        completed = true;
        objective.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
    }

    public void Incomplete()
    {
        completed = false;
        objective.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
    }

    public bool IsCompleted()
    {
        return completed;
    }
}
