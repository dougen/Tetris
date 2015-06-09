using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour 
{
    public static int cubeType;
    public GameObject[] groups;

    private void Start()
    {
        SpawnNext();
    }

	public void SpawnNext()
    {
        int i = Random.Range(0, groups.Length);
        cubeType = i;
        Instantiate(groups[i], transform.position, Quaternion.identity);
    }
}
