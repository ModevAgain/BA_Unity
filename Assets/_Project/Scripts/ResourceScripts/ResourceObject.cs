using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class ResourceObject : MonoBehaviour {

    [Header("Data")]
    public int RessourceType;
    public float MinDistanceSqr;
    public float Speed;
    public float Ressource2LifeTime;

    [Header("References")]



    private Transform _target;
    [SerializeField]
    private float _distanceSqr;
    private Vector3 _direction;
    private bool _pickedUp;
    private ResourceManager _resourceMan;
    private ParticleSystem _trails;
    private CraterScript _crater;
    private AudioSource _audioSrc;
    private Coroutine _ressource2LifeRoutine;

    private void Awake()
    {
        _resourceMan = FindObjectOfType<ResourceManager>();
        _trails = GetComponentInChildren<ParticleSystem>();
        _crater = GetComponentInChildren<CraterScript>();
        _audioSrc = GetComponent<AudioSource>();
    }


    public void GetPickedUp(Transform target)
    {
        if (!_pickedUp)
        {
            _pickedUp = true;
            _target = target;
            if(RessourceType == 0)
            {
                _crater.transform.parent = null;
            }
            else
            {
                _trails.Stop();
            }
            _trails.transform.parent = null;

            transform.DOKill();

            if (_ressource2LifeRoutine != null)
                StopCoroutine(_ressource2LifeRoutine);

            StartCoroutine(PickUpAnimation());
        }
    }

    public IEnumerator PickUpAnimation()
    {
        transform.DOPunchScale(transform.localScale, 0.5f);


        yield return new WaitForSeconds(0.15f);


        if (_target == null)
        {
            if(RessourceType == 0)
            {
                _crater.StopCrater();
                Destroy(_crater.gameObject);
            }
            Destroy(_trails.gameObject);
            Destroy(gameObject);
            _resourceMan.IncrementResource(RessourceType, 1);
            yield break;
        }

        _direction = _target.position - transform.position;
        _distanceSqr = _direction.sqrMagnitude;


        while(_distanceSqr> MinDistanceSqr)
        {
            transform.position = Vector3.Lerp(transform.position, _target.position, Time.deltaTime * Speed);     
            yield return null;

            if (_target == null)
                break;

            _direction = _target.position - transform.position;
            _distanceSqr = _direction.sqrMagnitude;
        }
        transform.DOKill();
        _resourceMan.IncrementResource(RessourceType,1);

        GetComponent<MeshRenderer>().enabled = false;


        if(RessourceType == 0)
        {
            while (_crater._active)
                yield return null;
            _crater.StopCrater();
            Destroy(_crater.gameObject);
        }
        else
        {
            while (_trails.IsAlive())
                yield return null;
        }

        if(_audioSrc != null)
        {
            _audioSrc.Play();
            yield return new WaitForSeconds(_audioSrc.clip.length);
        }
        Destroy(_trails.gameObject);
        Destroy(gameObject);
    }

    public IEnumerator HitLava()
    {
        _trails.transform.parent = null;
        yield return new WaitForSeconds(2);

        Destroy(_trails.gameObject);
        Destroy(gameObject);
    }

    public void StartRessource2Life()
    {
        _ressource2LifeRoutine = StartCoroutine(Ressource2LifeLoop());
        
    }

    public IEnumerator Ressource2LifeLoop()
    {

        yield return transform.DOMoveY(0.38f, 0.8f).SetEase(Ease.OutBounce);

        _trails.Play();

        yield return new WaitForSeconds(Ressource2LifeTime);

        if (_pickedUp)
            yield break;

        else
        {
            yield return transform.DOMoveY(-1.2f, 1).WaitForCompletion();

            Destroy(gameObject);
        }

    }
}
