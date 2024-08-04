using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeObject : MonoBehaviour
{
    [SerializeField]
    public float ShakeDuration;
    [SerializeField]
    public float _distance = 0.05f;
    

    private Vector3 _originalPos;
    private bool _isShaking = false;

    public void Shake()
    {
        if (!_isShaking)
        {
            _originalPos = transform.localPosition;
            StartCoroutine(RunShake());
        }
    }

    private IEnumerator RunShake()
    {
        float elapsedTime = 0f;
        _isShaking = true;
        while (elapsedTime < ShakeDuration)
        {
            Vector2 randomPosition = Random.insideUnitCircle * _distance;
            //transform.localPosition = Vector3.Lerp(_originalPos, _originalPos + new Vector3(randomPosition.x, randomPosition.y, 0f), _shakeSmoothness) * _shakeStrength;
            transform.localPosition = _originalPos + new Vector3(randomPosition.x, randomPosition.y, 0f);



            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = _originalPos;
        _isShaking = false;
    }
}

