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


    public IEnumerator ShootProjectile(Vector2 direction)
    {
        if (_projectileInProgress)
            yield break;

        //direction.y -= 50;

        _projectileInProgress = true;

        GameObject tempProjectile = Instantiate(Projectile);
        tempProjectile.transform.position = transform.position;

        _projectileDirection = GetDirection(direction);
        _projectileDirection.y = 0;

        //Debug.DrawLine(transform.position, transform.position + _projectileDirection * ProjectileDistance, Color.red, 5);

        tempProjectile.transform.DOLookAt(transform.position + _projectileDirection,0, up: tempProjectile.transform.up);
        yield return tempProjectile.transform.DOMove(transform.position + _projectileDirection * ProjectileDistance, ProjectileFlightTime).SetEase(Ease.OutQuint).WaitForCompletion();

        yield return new WaitForSeconds(ProjectileStayTime);

        _backDirection = transform.position - tempProjectile.transform.position;
        _distanceSqr = _backDirection.sqrMagnitude;

        float tempProjectileSpeed = ProjectileSpeed;

        Vector3 startPos = tempProjectile.transform.position;
        

        while (_distanceSqr > MinDistanceSqr)
        {
            //tempProjectile.transform.position = Vector3.Lerp(tempProjectile.transform.position, transform.position, Time.fixedDeltaTime * ProjectileSpeed);

            tempProjectile.transform.position = Vector3.MoveTowards(tempProjectile.transform.position, transform.position, Time.fixedDeltaTime * ProjectileSpeed);

            yield return new WaitForFixedUpdate();
            tempProjectile.transform.LookAt(transform.position);
            _backDirection = tempProjectile.transform.position - transform.position;
            _distanceSqr = _backDirection.sqrMagnitude;
            tempProjectileSpeed *= 2;
        }

        Debug.Log(tempProjectile.name);
        Destroy(tempProjectile);


        _projectileInProgress = false;
    }

    public Vector3 GetDirection(Vector2 direction)
    {
        Ray ray = Camera.main.ScreenPointToRay(direction);

        //Debug.Log(direction);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000, 1 << 17))
        {
            return (hit.point - transform.position).normalized;
        }

        return Vector3.zero;
    }
}
