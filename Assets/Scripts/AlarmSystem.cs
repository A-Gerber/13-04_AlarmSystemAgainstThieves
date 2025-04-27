using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AlarmSystem : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _clip;
    [SerializeField] private SecurityZone _securityZone;

    private Coroutine _coroutine;
    private WaitForSeconds _wait;
    private float _delay = 0.2f;
    private float _step = 10.0f;
    private float _maxValueVolue = 1;
    private float _minValueVolue = 0;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _wait = new WaitForSeconds(_delay);

        _audioSource.clip = _clip;
        _audioSource.loop = true;
        _audioSource.volume = 0;
    }

    private void Start()
    {
        _audioSource.Play();
    }

    private void OnEnable()
    {
        _securityZone.EnteredThief += EnableSiren;
        _securityZone.ExitedThief += DisableSiren;
    }

    private void OnDisable()
    {
        _securityZone.EnteredThief -= EnableSiren;
        _securityZone.ExitedThief -= DisableSiren;
    }

    private void EnableSiren()
    {
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(ChangeVolumeOfSiren(_maxValueVolue));
    }

    private void DisableSiren()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(ChangeVolumeOfSiren(_minValueVolue));
    }

    private IEnumerator ChangeVolumeOfSiren(float targetValue)
    {
        while (_audioSource.volume != targetValue)
        {
            yield return _wait;
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, targetValue, _step * Time.deltaTime);
        }
    }
}