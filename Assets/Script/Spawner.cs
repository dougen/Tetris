using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour 
{
    public static int cubeType;
    public static int nextType;
    public GameObject[] groups;
    public GameObject nextCube;

    private void Start()
    {
        nextType = Random.Range(0, groups.Length);
        SpawnNext();
    }

	public void SpawnNext()
    {  
        cubeType = nextType;
        nextType = Random.Range(0, groups.Length);
        ShowCube(nextType);
        Instantiate(groups[cubeType], transform.position, Quaternion.identity);
    }

    private void ShowCube(int type)
    {
        foreach (Transform child in nextCube.transform)
        {
            Destroy(child.gameObject);
        }
        GameObject next = (GameObject)Instantiate(groups[type]);
        next.GetComponent<Cube>().enabled = false;
        next.transform.position = nextCube.transform.position;
        next.transform.parent = nextCube.transform;
    }
}
