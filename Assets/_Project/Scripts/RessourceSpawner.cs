using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class RessourceSpawner : MonoBehaviour {

    public GameObject RessourcePrefab;

    public float SpawnTime;

	// Use this for initialization
	void Start () {

        StartCoroutine(SpawnRoutine());

	}
	


    public IEnumerator SpawnRoutine()
    {
        Vector3 targetPos = GetRandomLocation();
        targetPos.y = 16;

        GameObject tempRess = Instantiate(RessourcePrefab, targetPos, RessourcePrefab.transform.rotation);


        tempRess.GetComponentInChildren<ParticleSystem>().Play();
        tempRess.transform.DOMoveY(0.25f, 2f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(SpawnTime);
    }





    Vector3 GetRandomLocation()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        // Pick the first indice of a random triangle in the nav mesh
        int t = Random.Range(0, navMeshData.indices.Length - 3);

        // Select a random point on it
        Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t + 1]], Random.value);
        Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value);

        return point;
    }
}
