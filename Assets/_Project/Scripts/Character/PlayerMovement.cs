﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputSystem;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour {

    public BA_InputMapper InputMapper;

    public float SpeedForDirectMovement = 1.3f;




    [SerializeField]
    private float DesiredDistanceToTarget = 0.1f;
    [SerializeField]
    private float CurrentDistanceToTarget;

    private Vector3 _direction;
    private Vector3 _target;

    [SerializeField]
    private bool shouldMove;
    private NavMeshAgent _agent;
    private Rigidbody _rigid;

    [Header("Wall Check Data")]
    private int _frameCountToCache = 10;
    private int _frameCount;
    private Vector3 _posCache;
    public float MinDistanceForMovingDetection = 1;
    private BuildingInteraction _builder;

    void Start () {

        InputMapper.MoveInputVector2 += MoveVector2;
        InputMapper.MoveInputVector3 += MoveVector3;


        //FindObjectOfType<NavMeshSurface>().BuildNavMesh();
        _target = Vector3.negativeInfinity;
        _agent = GetComponent<NavMeshAgent>();
        _rigid = GetComponent<Rigidbody>();

        _builder = GetComponent<BuildingInteraction>();

    }

    
    private void MoveVector2(Vector2 inputPos)
    {

        _rigid.velocity = Vector3.zero;
        _rigid.angularVelocity = Vector3.zero;

        //No Input
        if (inputPos == Vector2.zero)
            return;

        _agent.enabled = false;

        Vector3 inputVectorOrtho = new Vector3(inputPos.x, 0, inputPos.y);

        _direction = transform.position + inputVectorOrtho;
        _rigid.MovePosition(transform.position + inputVectorOrtho * Time.deltaTime * SpeedForDirectMovement);

        if(transform.position + inputVectorOrtho * Time.deltaTime * SpeedForDirectMovement != transform.position)
            if (inputVectorOrtho.sqrMagnitude > 0.1f)                
                transform.LookAt(_direction);
        
        
    }

    public void MoveVector3(Vector3 inputPos)
    {

        if (_builder.BUILDING_IN_PROGESS)
        {
            //Debug.Log("wanted to move while building");
            return;
        }

        ///        Debug.Log("deltaTime: " + (Time.time - BA_RawInput.TickTime)); 
               //Debug.Log("move: " + Time.time);

        _agent.enabled = true;

        Ray ray = Camera.main.ScreenPointToRay(inputPos);

        RaycastHit hit;

        //if (Physics.Raycast(ray, out hit, 1000, 1 << 11))
        //{

        //    if (!hit.transform.GetComponent<Platform>().Activated)
        //        return;

        //    _target = hit.point;
        //    _direction = (_target - transform.position).normalized;
        //}

        if (Physics.Raycast(ray, out hit, 1000, 1 << 10))
        {

            _target = hit.point;

            Platform p = hit.transform.GetComponent<Platform>();

            if(p != null)
            {
                if (!p.Activated)
                    return;
            }

            _direction = (_target - transform.position).normalized;
        }

        _direction = (_target - transform.position).normalized;


        NavMeshHit navHit;

        NavMesh.SamplePosition(_target, out navHit, 1000,  NavMesh.AllAreas);

        _agent.SetDestination(navHit.position);

        //Debug.Log(navHit.position);

        shouldMove = true;

    }

    private void Update()
    {
        if (_target == Vector3.negativeInfinity || !shouldMove)
            return;



        CurrentDistanceToTarget = Vector3.Distance(transform.position, _target);

        if (CurrentDistanceToTarget < DesiredDistanceToTarget)
        {
            _target = Vector3.negativeInfinity;
            shouldMove = false;
            return;
        }

    }

   


}
