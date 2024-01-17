using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 20f;

    private int _randomDirection;
    private Vector3 _direction;

    private void Start()
    {
        _randomDirection = Random.Range(0, 2);

        if (_randomDirection == 0)
            _direction = Vector3.right;
        else
            _direction = Vector3.left;
    }

    private void Update()
    {

        transform.Rotate(_direction * _rotationSpeed * Time.deltaTime);
    }

}
