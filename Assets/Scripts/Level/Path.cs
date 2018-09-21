using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    [Header("Properties")]
    [Tooltip("Object on the end of the path. PathSpawner use this to place the new path at the end of the current path.")]
    [SerializeField] private Transform _endPath;

    private bool _shouldDestroy = false;

    public PathSpawner Spawner
    {
        get;
        set;
    }

    public Transform EndPath
    {
        get { return (this._endPath); }
    }

    public Collider DestroyCollider
    {
        private get;
        set;
    }

    public bool ShouldDestroy
    {
        get { return (this._shouldDestroy); }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            this.Spawner.AddNewPath();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == this.DestroyCollider)
        {
            this.gameObject.SetActive(false);
            this._shouldDestroy = true;
        }
    }
}
