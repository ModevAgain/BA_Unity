using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class RessourceSpawner : MonoBehaviour {

    public GameObject RessourcePrefab;

    public float MaxSpawnDistance;
    public int MinDistanceToHQ;

    public float SpawnTime;
    public bool ShouldSpawn = true;
    private NavMeshTriangulation _navMeshData;
    public GridManager Grid;
    private List<Platform> _validPlatforms;

    private Vector3 origin;
    private Vector3 directionalTarget;
    private Vector3 direction;
    private Vector3 targetPos;
    private bool validPointFound = false;
    private int randomPlatformIndex;
    private int randomX;
    private int randomZ;
    

    // Use this for initialization
    void Start () {

        StartCoroutine(SpawnRoutine());
        _validPlatforms = new List<Platform>();
	}
	


    public IEnumerator SpawnRoutine()
    {
        GameObject tempRess;
        Vector3 targetPos;

        while (ShouldSpawn)
        {
            targetPos = GetRandomLocation();
            targetPos.y = 16;

            tempRess = Instantiate(RessourcePrefab, targetPos, RessourcePrefab.transform.rotation);


            tempRess.GetComponentInChildren<ParticleSystem>().Play();
            //tempRess.transform.DOMoveY(0.656f, 2f).SetEase(Ease.Linear);

            yield return new WaitForSeconds(SpawnTime);
        }
    }




    private Vector3 GetRandomLocation()
    {
        //_navMeshData = NavMesh.CalculateTriangulation();

        //// Pick the first indice of a random triangle in the nav mesh
        //int t = Random.Range(0, _navMeshData.indices.Length - 3);

        //// Select a random point on it
        //Vector3 point = Vector3.Lerp(_navMeshData.vertices[_navMeshData.indices[t]], _navMeshData.vertices[_navMeshData.indices[t + 1]], Random.value);
        //Vector3.Lerp(point, _navMeshData.vertices[_navMeshData.indices[t + 2]], Random.value);

        //return point;

        validPointFound = false;
        _validPlatforms = Grid.GetValidPlatforms();

        while (!validPointFound)
        {
            randomPlatformIndex = Random.Range(0, _validPlatforms.Count);
            origin = _validPlatforms[randomPlatformIndex].transform.position;

            randomX = Random.Range(-10000, 10000);
            randomZ = Random.Range(-10000, 10000);
            directionalTarget = new Vector3(randomX, 0, randomZ);

            direction = (directionalTarget - origin).normalized;

            targetPos = origin + direction * Random.Range(-MaxSpawnDistance, MaxSpawnDistance);

            //Debug.Log((Grid.GetHQ().transform.position - targetPos).sqrMagnitude);

            if((Grid.GetHQ().transform.position - targetPos).sqrMagnitude > MinDistanceToHQ)
            {
                validPointFound = true;
            }            

        }


        return targetPos;
    }
}
