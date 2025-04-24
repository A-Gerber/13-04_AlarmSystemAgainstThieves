using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AlarmSystem : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _clip;
    [SerializeField] private SecurityZone _securityZone;

    private Coroutine _coroutineIncrease;
    private Coroutine _coroutineReduce;
    private WaitForSeconds _wait;
    private float _delay = 0.4f;
    private float _step = 0.05f;
    private float _maxValueVolue = 1.0f;
    private float _currentValueVolue = 0;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _wait = new WaitForSeconds(_delay);

        _audioSource.clip = _clip;
        _audioSource.loop = true;
        _audioSource.volume = _currentValueVolue;
    }

    private void OnEnable()
    {
        _securityZone.EnteredThief += EnableSiren;
        _securityZone.ExitedThief += DisableSiren;
    }

    private void OnDisable()
    {
        if (_coroutineIncrease != null)
        {
            StopCoroutine(_coroutineIncrease);
        }

        if (_coroutineReduce != null)
        {
            StopCoroutine(_coroutineReduce);
        }

        _securityZone.EnteredThief -= EnableSiren;
        _securityZone.ExitedThief -= DisableSiren;
    }

    private void EnableSiren()
    {
        if (_coroutineReduce != null)
        {
            StopCoroutine(_coroutineReduce);
        }

        _coroutineIncrease = StartCoroutine(IncreaseVolumeOfSiren());
        _audioSource.Play();
    }

    private void DisableSiren()
    {
        if (_coroutineIncrease != null)
        {
            StopCoroutine(_coroutineIncrease);
        }

        _coroutineIncrease = StartCoroutine(ReduceVolumeOfSiren());
    }

    private IEnumerator IncreaseVolumeOfSiren()
    {
        while (_currentValueVolue <= _maxValueVolue)
        {
            yield return _wait;
            _currentValueVolue += _step;
            _audioSource.volume = _currentValueVolue;
        }
    }

    private IEnumerator ReduceVolumeOfSiren()
    {
        while (_currentValueVolue >= 0)
        {
            yield return _wait;
            _currentValueVolue -= _step;
            _audioSource.volume = _currentValueVolue;
        }

        _audioSource.Stop();
    }
}