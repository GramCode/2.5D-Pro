using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private Transform[] _waypoints;
    [SerializeField]
    private float _speed = 5f;

    private int _index;

    private void FixedUpdate()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        float distance = Vector3.Distance(transform.position, _waypoints[_index].transform.position);

        if (distance < 0.1f)
        {
            _index++;

            if (_index > _waypoints.Length - 1)
                _index = 0;
        }

        transform.position = Vector3.MoveTowards(transform.position, _waypoints[_index].transform.position, _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entered Trigger");
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Exit Trigger");
            other.transform.parent = null;
        }
    }
}
