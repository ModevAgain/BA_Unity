using System;
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

    public Dictionary<HashableVector, Platform> Grid;

   

    public void Setup()
    {
        Grid = new Dictionary<HashableVector, Platform>();
        StartingGrid = FindObjectsOfType<Platform>();

        foreach (var item in StartingGrid)
        {
            Grid.Add(new HashableVector(item.transform.localPosition), item);
        }
    }

   
    public Platform GetPlatformForHighlight(Vector3 pos, Vector3 dir, int buildingOp)
    {
        Platform outPlatform;
        
        Vector3 direction = OrthoDirections.GetDirFromLookDirection(pos ,dir);

        direction *= PlatformEdgeSize;

        //Debug.Log("newPlat:" + (pos + direction));

        //foreach (var item in Grid)
        //{
        //    //Debug.Log(item.Key);
        //}
        HashableVector key = new HashableVector(pos + direction);

        Grid.TryGetValue(key, out outPlatform);

        //if(Grid.ContainsKey(key))
        //{
        //    outPlatform = Grid[key];
        //}
        //else
        //{
        //    outPlatform = null;
        //}

        if(outPlatform == null)
        {
            GameObject tempPlat = Instantiate<GameObject>(PlatformObject, DataPipe.instance.PlatformHolder.transform);
            outPlatform = tempPlat.GetComponent<Platform>();
            Grid.Add(key, outPlatform);
            tempPlat.transform.localPosition = key.GetVector();
            Vector3 tempVec = tempPlat.transform.localPosition / PlatformEdgeSize;
            tempPlat.name = "Platform ( " + tempVec.x + " | " + tempVec.z + " )  Type: " + buildingOp;
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


public class HashableVector : System.Object
{
    public int x;
    public int y;
    public int z;

    public override int GetHashCode()
    {
        return this.x.GetHashCode() ^ this.y.GetHashCode() << 2 ^ this.z.GetHashCode() >> 2;
    }

    public HashableVector(Vector3 v)
    {
        this.x = (int)v.x;
        this.y = (int)v.y;
        this.z = (int)v.z;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(this, obj))
            return true;
 
        HashableVector other = obj as HashableVector;

        if (ReferenceEquals(null, other))
            return false;

        if (other.x == this.x && other.y == this.y && other.z == this.z)
            return true;

        return false;
    }

    public Vector3 GetVector()
    {
        return new Vector3(x, y, z);
    }
}
