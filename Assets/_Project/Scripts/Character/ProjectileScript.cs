using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {

    public void OnTriggerEnter(Collider other)
    {

        other.GetComponent<ResourceObject>().GetPickedUp(transform);
    }
}
