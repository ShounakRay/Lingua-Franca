using System.Collections;
using System.Collections.Generic;
using Unity.Tutorials.Core.Editor;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{   
    // The objectives are all stored in a box. get that box to count number of objectives in it.
    // public GameObject objectiveContainer;
    
    // The objective prefab
    public GameObject objectivePrefab;
    public GameObject objectiveBox;

    
    public int numObjectives = 0; 
    public int numCompleted = 0;
    private List<Objective> objectives = new List<Objective>();
    
    // Start is called before the first frame update
    void Start()
    {
        objectiveBox.SetActive(false);
        
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

    public void AddObjective(string name, string description)
    {
        // Create a new objective
        GameObject newObjective = Instantiate(objectivePrefab, objectiveBox.transform);
        newObjective.name = name;
        Debug.Log("Adding objective " + newObjective.name + " to the objective box");
        // Write out the child
        newObjective.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = description;
        objectives.Add(new Objective(newObjective));
        numObjectives++;
        if (numObjectives == 1)
        {
            Debug.Log("First objective added");
            objectiveBox.SetActive(true);
        }
    }

    public void completeObjective(string name)
    {
        for (int i = 0; i < numObjectives; i++)
        {
            if (objectives[i].objectiveName == name)
            {
                objectives[i].Complete();
                numCompleted++;
            }
        }
        if (numObjectives == numCompleted)
        {
            Debug.Log("All objectives are completed");
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
