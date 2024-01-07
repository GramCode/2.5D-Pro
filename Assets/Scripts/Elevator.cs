using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField]
    private Transform[] Waypoints;
    [SerializeField]
    private float _speed = 4f;

    private int _index = 1;
    private bool _wait = false;

    private void Update()
    {
        Operate();
    }

    private void Operate()
    {
        float distance = Vector3.Distance(transform.position, Waypoints[_index].transform.position);
        
        if (distance < 0.1f)
        {
            _index++;

            if (_index > Waypoints.Length - 1)
                _index = 0;

            StartCoroutine(SwitchFloorRoutine());
        }
        Debug.Log("Current Index: " + _index);
        if (_wait == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, Waypoints[_index].transform.position, _speed * Time.deltaTime);
        }

        Debug.Log("Distance: " + distance);
    }

    IEnumerator SwitchFloorRoutine()
    {
        _wait = true;
        yield return new WaitForSeconds(5.0f);
        _wait = false;
    }
}
