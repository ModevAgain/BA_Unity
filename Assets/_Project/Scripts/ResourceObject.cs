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
        //Sequence seq = DOTween.Sequence();

        //seq.Append(transform.DOPunchScale(transform.localScale * 2, 0.3f));

        transform.DOPunchScale(transform.localScale * 2, 0.3f);


        yield return new WaitForSeconds(0.15f);


        _direction = _target.position - transform.position;
        _distanceSqr = _direction.sqrMagnitude;

        Debug.Log(_distanceSqr);

        while(_distanceSqr> MinDistanceSqr)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + _direction.normalized * Speed, 0f);
            //yield return transform.DOMove(transform.position + _direction.normalized * Speed, 0.1f).WaitForCompletion();

            _direction = _target.position - transform.position;
            _distanceSqr = _direction.sqrMagnitude;

            //yield return new WaitForSeconds(0.5f);
        }
        transform.DOKill();
        Destroy(gameObject);
    }
}
