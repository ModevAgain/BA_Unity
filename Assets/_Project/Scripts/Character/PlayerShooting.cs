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
    private bool _projectileInProgress;

    // Use this for initialization
    void Start () {

        InputMapper.ActionKey_2 += (dir) => StartCoroutine(ShootProjectile(dir));
        //InputMapper.mo


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator ShootProjectile(Vector2 direction)
    {
        if (_projectileInProgress)
            yield break;

        _projectileInProgress = true;

        GameObject tempProjectile = Instantiate(Projectile);
        tempProjectile.transform.position = transform.position;

        _projectileDirection = GetDirection(direction);
        _projectileDirection.y = 0;

        //Debug.DrawLine(transform.position, transform.position + _projectileDirection * ProjectileDistance, Color.red, 5);

        tempProjectile.transform.DOLookAt(transform.position + _projectileDirection,0, up: tempProjectile.transform.up);
        yield return tempProjectile.transform.DOMove(transform.position + _projectileDirection * ProjectileDistance, ProjectileFlightTime).WaitForCompletion();

        yield return new WaitForSeconds(ProjectileStayTime);

        _backDirection = transform.position - tempProjectile.transform.position;
        _distanceSqr = _backDirection.sqrMagnitude;

        while (_distanceSqr > MinDistanceSqr)
        {
            tempProjectile.transform.position = Vector3.Lerp(tempProjectile.transform.position, transform.position, Time.deltaTime * ProjectileSpeed);
            yield return null;
            tempProjectile.transform.LookAt(transform.position);
            _backDirection = tempProjectile.transform.position - transform.position;
            _distanceSqr = _backDirection.sqrMagnitude;
            ProjectileSpeed *= 1.01f;
        }

        Destroy(tempProjectile);


        _projectileInProgress = false;
    }

    public Vector3 GetDirection(Vector2 direction)
    {
        Ray ray = Camera.main.ScreenPointToRay(direction);

        //Debug.Log(direction);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000, 1 << 10))
        {
            return (hit.point - transform.position).normalized;
        }

        return Vector3.zero;
    }
}
