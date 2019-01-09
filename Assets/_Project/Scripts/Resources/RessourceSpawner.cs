using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class RessourceSpawner : MonoBehaviour {

    [Header("Ressource 1")]
    public GameObject Ressource1Prefab;

    public float MaxSpawnDistance;
    public int MinDistanceToHQ;

    public float SpawnTimeRessorce1;
    public bool ShouldSpawn_Ressource1 = true;

    [Header("Ressource 2")]
    public GameObject Ressource2Prefab;
    public int MinSpawnCount;
    public int MaxSpawnCount;
    public float SpawnTimeRessorce2;
    public float MinSpawnDistance_Ressource2Free = 0.5f;
    public float MaxSpawnDistance_Ressource2Free = 3f;
    public float RotationalVariationInOrigin = 1.8f;
    public bool ShouldSpawn_Ressource2 = true;

    [Header("General")]
    public GridManager Grid;
    private List<Platform> _validPlatforms;

    private Vector3 origin_0;
    private Vector3 directionalTarget_0;
    private Vector3 direction_0;
    private Vector3 targetPos_0;

    private Vector3 origin_1;
    private Vector3 directionalTarget_1_platform;
    private Vector3 direction_1_platform;
    private Vector3 targetPos_1_platform;

    private Vector3 origin_1_free;
    private Vector3 directionalTarget_1_free;
    private Vector3 direction_1_free;
    private Vector3 targetPos_1_free;

    private bool validPointFound = false;
    private int randomPlatformIndex0;
    private int randomPlatformIndex1;
    private int randomX_0;
    private int randomZ_0;
    private int randomX_1;
    private int randomZ_1;


    // Use this for initialization
    void Start () {

        _validPlatforms = new List<Platform>();
        StartCoroutine(Ressource1_SpawnRoutine());
        StartCoroutine(Ressource2_SpawnFreeRoutine());
	}

   

    #region Ressource 1

    public IEnumerator Ressource1_SpawnRoutine()
    {
        GameObject tempRess;
        Vector3 targetPos;

        while (ShouldSpawn_Ressource1)
        {
            targetPos = GetRandomLocation_Ressource1();
            targetPos.y = 16;

            tempRess = Instantiate(Ressource1Prefab, targetPos, Ressource1Prefab.transform.rotation);


            tempRess.GetComponentInChildren<ParticleSystem>().Play();
            //tempRess.transform.DOMoveY(0.656f, 2f).SetEase(Ease.Linear);

            yield return new WaitForSeconds(SpawnTimeRessorce1);
        }
    }

    private Vector3 GetRandomLocation_Ressource1()
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
            randomPlatformIndex0 = Random.Range(0, _validPlatforms.Count);
            origin_0 = _validPlatforms[randomPlatformIndex0].transform.position;

            randomX_0 = Random.Range(-10000, 10000);
            randomZ_0 = Random.Range(-10000, 10000);
            directionalTarget_0 = new Vector3(randomX_0, 0, randomZ_0);

            direction_0 = (directionalTarget_0 - origin_0).normalized;

            targetPos_0 = origin_0 + direction_0 * Random.Range(-MaxSpawnDistance, MaxSpawnDistance);

            //Debug.Log((Grid.GetHQ().transform.position - targetPos).sqrMagnitude);

            if((Grid.GetHQ().transform.position - targetPos_0).sqrMagnitude > MinDistanceToHQ)
            {
                validPointFound = true;
            }            

        }


        return targetPos_0;
    }

    #endregion  

    #region Ressource 2

    public IEnumerator Ressource2_SpawnOnPlatform(Platform p)
    {
        yield return null;

        Vector3 spawnLocation;

        int spawnCount = Random.Range(MinSpawnCount, MaxSpawnCount);

        for (int i = 0; i < spawnCount; i++)
        {
            spawnLocation = GetRandomLocationOnPlatform_Ressource2(p.transform.position);

            Instantiate(Ressource2Prefab, spawnLocation, Quaternion.Euler(Vector3.zero));

            yield return new WaitForSeconds(0.5f);
        }

    }

    public Vector3 GetRandomLocationOnPlatform_Ressource2(Vector3 origin, float variation = 1f)
    {
        randomX_1 = Random.Range(-10000, 10000);
        randomZ_1 = Random.Range(-10000, 10000);
        directionalTarget_1_platform = new Vector3(randomX_1, 0, randomZ_1);

        direction_1_platform = (directionalTarget_0 - origin).normalized;

        targetPos_1_platform = origin + direction_1_platform * Random.Range(0.1f, 1.4f * variation);
        targetPos_1_platform.y = 0.68f;

        return targetPos_1_platform;
    }

    public IEnumerator Ressource2_SpawnFreeRoutine()
    {
        

        Vector3 spawnLocation;

        while (ShouldSpawn_Ressource2)
        {
            spawnLocation = GetRandomLocation_Ressource2();

            GameObject tempRes2 = Instantiate(Ressource2Prefab);
            tempRes2.transform.position = spawnLocation;

            tempRes2.GetComponentInChildren<ResourceObject>().StartRessource2Life();

            yield return new WaitForSeconds(SpawnTimeRessorce2);
        }

        

    }

    public Vector3 GetRandomLocation_Ressource2()
    {

        validPointFound = false;
        _validPlatforms = Grid.GetValidPlatforms();

        while (!validPointFound)
        {
            randomPlatformIndex1 = Random.Range(0, _validPlatforms.Count);
            directionalTarget_1_free = _validPlatforms[randomPlatformIndex1].GetRandomActiveWall();

            if (directionalTarget_1_free == Vector3.zero)
                continue;

            origin_1_free = GetRandomLocationOnPlatform_Ressource2(_validPlatforms[randomPlatformIndex1].transform.position, RotationalVariationInOrigin);

            direction_1_free = (directionalTarget_1_free - origin_1_free).normalized;

            targetPos_1_free = directionalTarget_1_free + direction_1_free * Random.Range(MinSpawnDistance_Ressource2Free, MaxSpawnDistance_Ressource2Free);

            targetPos_1_free.y = -1.2f;

            validPointFound = true;

        }

        return targetPos_1_free;
    }

    #endregion  
}
