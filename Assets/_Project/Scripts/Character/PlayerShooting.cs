using BA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerShooting : MonoBehaviour {

    [Header("References")]
    public BA_InputMapper InputMapper;
    public GameObject Projectile;

    [Header("Data")]
    public float ProjectileDistance;
    public float ProjectileFlightTime;
    public float ProjectileStayTime;
    public float ProjectileSpeed;
    public float MinDistanceSqr;

    private Vector3 _projectileDirection;
    private float _distanceSqr;
    private Vector3 _backDirection;


    // Use this for initialization
    void Start () {

        InputMapper.ActionKey_2 += () => StartCoroutine(ShootProjectile());
        //InputMapper.mo


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator ShootProjectile()
    {
        GameObject tempProjectile = Instantiate(Projectile);
        tempProjectile.transform.position = transform.position;

        _projectileDirection = GetDirection();
        _projectileDirection.y = 0;

        //Debug.DrawLine(transform.position, transform.position + _projectileDirection * ProjectileDistance, Color.red, 5);
        
        yield return tempProjectile.transform.DOMove(transform.position + _projectileDirection * ProjectileDistance, ProjectileFlightTime).WaitForCompletion();

        yield return new WaitForSeconds(ProjectileStayTime);

        _backDirection = transform.position - tempProjectile.transform.position;
        _distanceSqr = _backDirection.sqrMagnitude;

        while (_distanceSqr > MinDistanceSqr)
        {
            tempProjectile.transform.position = Vector3.Lerp(tempProjectile.transform.position, transform.position, Time.deltaTime * ProjectileSpeed);
            yield return null;
            _backDirection = tempProjectile.transform.position - transform.position;
            _distanceSqr = _backDirection.sqrMagnitude;
        }

        Destroy(tempProjectile);
    }

    public Vector3 GetDirection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000, 1 << 10))
        {           
            return (hit.point - transform.position).normalized;
        }

        return Vector3.zero;
    }
}
