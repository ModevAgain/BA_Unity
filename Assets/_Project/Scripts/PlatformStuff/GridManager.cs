using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GridManager", menuName = "Data/GridManager", order = 1)]
public class GridManager : ScriptableObject {

    [Header("PlatformData")]
    public int PlatformEdgeSize;

    [Header("PlatformReferences")]
    public GameObject PlatformObject;
    public GameObject PlatformHolder;

    private Platform[] StartingGrid;

    public Dictionary<Vector3, Platform> Grid;

   

    public void Setup()
    {
        Grid = new Dictionary<Vector3, Platform>();
        StartingGrid = FindObjectsOfType<Platform>();

        foreach (var item in StartingGrid)
        {
            Grid.Add(item.transform.localPosition, item);
        }
    }

   
    public Platform GetPlatformForHighlight(Vector3 pos, Vector3 dir)
    {
        Platform outPlatform;
        
        Vector3 direction = OrthoDirections.GetDirFromLookDirection(pos ,dir);

        direction *= PlatformEdgeSize;

        //Debug.Log("newPlat:" + (pos + direction));

        //foreach (var item in Grid)
        //{
        //    //Debug.Log(item.Key);
        //}

        if(Grid.ContainsKey(pos + direction))
        {
            outPlatform = Grid[pos + direction];
        }
        else
        {
            outPlatform = null;
        }

        if(outPlatform == null)
        {
            GameObject tempPlat = Instantiate<GameObject>(PlatformObject, DataPipe.instance.PlatformHolder.transform);
            outPlatform = tempPlat.GetComponent<Platform>();
            Grid.Add(pos + direction, outPlatform);
            tempPlat.transform.localPosition = pos + direction;
            Vector3 tempVec = tempPlat.transform.localPosition / 5;
            tempPlat.name = "Platform ( " + tempVec.x + " | " + tempVec.z + " )" ;
        }

        return outPlatform;
    }


    //HelperClass for Directions in OrthoGrid
    public class OrthoDirections
    {


        public static Vector3[] TopLeft = { new Vector3(-1, 0, 0), new Vector3(-1,0,1)};
        public static Vector3[] BottomLeft = { new Vector3(0, 0, -1), new Vector3(-1,0,-1)};
        public static Vector3[] TopRight = { new Vector3(0, 0, 1), new Vector3(1,0,1)};
        public static Vector3[] BottomRight = { new Vector3(1, 0, 0), new Vector3(1,0,-1)};

        public static Vector3[][] Directions = { TopLeft, BottomLeft, TopRight, BottomRight };

        public static Vector3 GetDirFromLookDirection(Vector3 pos, Vector3 lookDirection)
        {
            Vector3 direction = Vector3.zero;
            float maxDot = float.MinValue;


            foreach (var dir in Directions)
            {
                Vector3 lookDir = pos + dir[1] - pos;


                float currentDot = Vector3.Dot(lookDir, lookDirection);
                //Debug.Log("PlayerDir: " + lookDirection + " PlatformDir: " + lookDir + " Dot: " + currentDot);
                if (currentDot > maxDot)
                {
                    maxDot = currentDot;
                    direction = dir[0];
                }
            }
            return direction;
        }
    }
         
}
