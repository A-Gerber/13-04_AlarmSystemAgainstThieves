using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MoverOnPoints : MonoBehaviour
{
    [SerializeField] private int _currentSpeed = 1;
    [SerializeField] private List<Transform> _waypoints;

    private int _startWaypoint = 0;
    private int _currentWaypoint;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _currentWaypoint = _startWaypoint;
    }

    private void Update()
    {
        if (_waypoints.Count != 0)
        {
            if (transform.position == _waypoints[_currentWaypoint].position)
            {
                _currentWaypoint = ++_currentWaypoint % _waypoints.Count;
            }

            transform.LookAt(_waypoints[_currentWaypoint]);
            transform.position = Vector3.MoveTowards(transform.position, _waypoints[_currentWaypoint].position, _currentSpeed * Time.deltaTime);

            _animator.SetFloat(PlayerAnimatorData.Params.Speed, _currentSpeed);
        }
    }
}

public static class PlayerAnimatorData
{
    public static class Params
    {
        public static readonly int Speed = Animator.StringToHash(nameof(Speed));
    }
}