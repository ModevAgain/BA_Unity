using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class ResourceObject : MonoBehaviour {

    [Header("Data")]
    public float MinDistanceSqr;
    public float Speed;
    [Header("References")]



    private Transform _target;
    [SerializeField]
    private float _distanceSqr;
    private Vector3 _direction;
    private bool _pickedUp;
    private ResourceManager _resourceMan;
    private ParticleSystem _trails;
    private CraterScript _crater;

    private void Awake()
    {
        _resourceMan = FindObjectOfType<ResourceManager>();
        _trails = GetComponentInChildren<ParticleSystem>();
        _crater = GetComponentInChildren<CraterScript>();
    }


    public void GetPickedUp(Transform target)
    {
        if (!_pickedUp)
        {
            _pickedUp = true;
            _target = target;
            _crater.transform.parent = null;
            _trails.transform.parent = null;
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
        _resourceMan.IncrementResource(1);

        GetComponent<MeshRenderer>().enabled = false;

        while (_crater._active)
            yield return null;

        _crater.StopCrater();
        Destroy(_trails.gameObject);
        Destroy(_crater.gameObject);
        Destroy(gameObject);
    }
}
