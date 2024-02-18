using System.Collections;
using System.Collections.Generic;
// using Unity.Tutorials.Core.Editor;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{   
    // The objectives are all stored in a box. get that box to count number of objectives in it.
    // public GameObject objectiveContainer;
    
    // The objective prefab
    public GameObject objectivePrefab;
    public GameObject objectiveWrapper;

    
    public int numObjectives = 0; 
    public int numCompleted = 0;
    private List<Objective> objectives = new List<Objective>();
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // for every objective in the scene, randomly complete it. 
        for (int i = 0; i < numObjectives; i++)
        {
            if (Random.Range(0, 2000) == 1)
            {
                completeObjective(objectives[i].objectiveName);
            }
        }
    }

    public void AddObjective(string name, string description)
    {
        if (numObjectives == 0)
            objectiveWrapper.GetComponent<CanvasGroup>().alpha = 1;

        Debug.Log("Adding objective " + name + " to the objective box");
        // Create a new objective
        GameObject newObjective = Instantiate(objectivePrefab, objectiveWrapper.transform);
        newObjective.name = name;
        // Write out the child
        newObjective.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = description;
        objectives.Add(new Objective(newObjective));
        numObjectives++;
    }

    public void completeObjective(string name)
    {
        for (int i = 0; i < numObjectives; i++)
        {
            if (objectives[i].objectiveName == name)
            {
                if (objectives[i].IsCompleted())
                {
                    Debug.Log("Objective " + name + " is already completed");
                    return;
                } else {
                    objectives[i].Complete();
                    numCompleted++;
                }
            }
        }
        // pirnt number of objectives completed
        Debug.Log("Number of objectives completed: " + numCompleted);
        Debug.Log("Number of objectives: " + numObjectives);
        if (numObjectives == numCompleted)
        {
            Debug.Log("All objectives are completed");
            //TODO: Find some way to celebrate the completion of all objectives
        }
    }
}

// Create a struct to hold the objective data
public class Objective
{
    public Objective(GameObject objective)
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
