using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class ResourceObject : MonoBehaviour {

    public float MinDistanceSqr;
    public float Speed;

    private Transform _target;
    [SerializeField]
    private float _distanceSqr;
    private Vector3 _direction;
    private bool _pickedUp;

    public void GetPickedUp(Transform target)
    {
        if (!_pickedUp)
        {
            _pickedUp = true;
            _target = target;
            StartCoroutine(PickUpAnimation());
        }
    }

    public IEnumerator PickUpAnimation()
    {
        transform.DOPunchScale(transform.localScale, 0.5f);


        yield return new WaitForSeconds(0.15f);


        _direction = _target.position - transform.position;
        _distanceSqr = _direction.sqrMagnitude;


        while(_distanceSqr> MinDistanceSqr)
        {
            transform.position = Vector3.Lerp(transform.position, _target.position, Time.deltaTime * Speed);     
            yield return null;
            _direction = _target.position - transform.position;
            _distanceSqr = _direction.sqrMagnitude;
        }
        transform.DOKill();
        Destroy(gameObject);
    }
}
