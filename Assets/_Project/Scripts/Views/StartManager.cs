using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour {

    [Header("Data")]
    public GameData GameData;
    [Header("References")]
    public GameObject Resource1Object;
    public Collider PlaneCol;

    public float SpawnTimeRessorce1;
    private bool _shouldSpawn;

    private float maxX;
    private float minX;
    private float maxZ;
    private float minZ;

    // Use this for initialization
    void Start () {

        Application.targetFrameRate = 60;

        maxX = PlaneCol.bounds.max.x;
        minX = PlaneCol.bounds.min.x;

        maxZ = PlaneCol.bounds.max.z;
        minZ = PlaneCol.bounds.min.z;

        StartCoroutine(SpawnResource());

        Cursor.visible = true;
    }


    public IEnumerator SpawnResource()
    {


        GameObject tempRess;
        Vector3 targetPos;

        _shouldSpawn = true;

        while (_shouldSpawn)
        {
            targetPos = GetRandomLocation_Ressource1();
            targetPos.y = 16;

            tempRess = Instantiate(Resource1Object, targetPos, Resource1Object.transform.rotation);


            tempRess.GetComponentInChildren<ParticleSystem>().Play();
            //tempRess.transform.DOMoveY(0.656f, 2f).SetEase(Ease.Linear);

            yield return new WaitForSeconds(SpawnTimeRessorce1);


        }
    }

    public Vector3 GetRandomLocation_Ressource1()
    {
        float x = Random.Range(minX, maxX);
        float z = Random.Range(minZ, maxZ);

        return new Vector3(x, 0, z);
    }

    public void StartGame()
    {
        GameData.IsTest = false;
        SceneManager.LoadScene("Main");
    }

    public void TestGame()
    {
        GameData.IsTest = true;
        SceneManager.LoadScene("Main");
    }
}
