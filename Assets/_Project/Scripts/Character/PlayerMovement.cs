using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BA;
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

    private bool shouldMove;
    private NavMeshAgent _agent;
    private Rigidbody _rigid;

    void Start () {

        InputMapper.MoveInputVector2 += MoveVector2;
        InputMapper.MoveInputVector3 += MoveVector3;


        //FindObjectOfType<NavMeshSurface>().BuildNavMesh();
        _target = Vector3.negativeInfinity;
        _agent = GetComponent<NavMeshAgent>();
        _rigid = GetComponent<Rigidbody>();
    }

    
    private void MoveVector2(Vector2 inputPos)
    {

        //No Input
        if (inputPos == Vector2.zero)
            return;

        _agent.enabled = false;

        Vector3 inputVectorOrtho = new Vector3(inputPos.x, 0, inputPos.y);

        _direction = transform.position + inputVectorOrtho;
        _rigid.MovePosition(transform.position + inputVectorOrtho * Time.deltaTime * SpeedForDirectMovement);

        if(transform.position + inputVectorOrtho * Time.deltaTime * SpeedForDirectMovement != transform.position)
            transform.LookAt(_direction);
        

    }

    private void MoveVector3(Vector3 inputPos)
    {

        _agent.enabled = true;

        Ray ray = Camera.main.ScreenPointToRay(inputPos);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000, 1 << 11))
        {

            if (!hit.transform.GetComponent<Platform>().Activated)
                return;

            _target = hit.point;
            _direction = (_target - transform.position).normalized;
        }

        _direction = (_target - transform.position).normalized;

        _agent.SetDestination(_target);

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
