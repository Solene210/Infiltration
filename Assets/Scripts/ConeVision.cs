using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using Unity.VisualScripting;
using UnityEngine;

public class ConeVision : MonoBehaviour
{
    #region Expose
    [SerializeField] private LayerMask _playerLayer;
    [HideInInspector] public GameObject _target;
    #endregion

    #region Unity Life Cycle
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 rayDirection = other.transform.position - transform.position;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, rayDirection, out hit, Mathf.Infinity, _playerLayer))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("Halte! Je te vois, tu sais!");
                    _target = other.gameObject;
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _target = null;
        }
    }
    #endregion
}
