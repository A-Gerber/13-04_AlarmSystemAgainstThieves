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
    private float _delay = 0.4f;
    private float _step = 0.06f;
    private int _maxValueVolue = 1;
    private int _minValueVolue = 0;
    private float _currentValueVolue = 0;
    private bool _isEnableSiren = false;

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
        _securityZone.EnteredThief -= EnableSiren;
        _securityZone.ExitedThief -= DisableSiren;
    }

    private void EnableSiren()
    {
        _audioSource.Play();
        _isEnableSiren = true;
        _coroutine = StartCoroutine(ChangeVolumeOfSiren());
    }

    private void DisableSiren()
    {
        _isEnableSiren = false;
    }

    private IEnumerator ChangeVolumeOfSiren()
    {
        bool isWork = enabled;

        while (isWork)
        {
            yield return _wait;

            if (_isEnableSiren)
            {
                _currentValueVolue = Mathf.Min(_currentValueVolue + _step, _maxValueVolue);
            }
            else
            {
                _currentValueVolue = Mathf.Max(_currentValueVolue - _step, _minValueVolue);
            }

            _audioSource.volume = _currentValueVolue;

            if (_currentValueVolue == 0)
            {
                _audioSource.Stop();
                StopCoroutine(_coroutine);
            }
        }
    }
}