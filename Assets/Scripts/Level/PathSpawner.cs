using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSpawner : MonoBehaviour
{
    [Header("Path")]
    [SerializeField] private GameObject _pathContainer;
    [SerializeField] private Collider _activePathCollider;
    [SerializeField] private Path _startPath;
    [SerializeField] private List<Path> _paths;

    [Header("Properties")]
    [Tooltip("How many paths to generate in advance")]
    [SerializeField] private int _generateForwardCount = 2;
    [SerializeField] private float _optimalMoveSpeed;
    [SerializeField] private bool _invert = false;

    [Header("Speed variables")]
    [SerializeField] private FloatVariable _userSpeedVariable;
    [SerializeField] private FloatVariable _userOptimalSpeedVariable;

    private List<Path> _inUsePaths;
    private List<Path> _toDestroyPaths;
    private bool _play;
    private float _moveSpeed;

    private void Awake()
    {
        if (this._pathContainer == null)
            this._pathContainer = this.gameObject;
        this._inUsePaths = new List<Path>();
        while (this._inUsePaths.Count < this._generateForwardCount + 1)
            this.AddNewPath();
        this._toDestroyPaths= new List<Path>();
        this._play = false;
        this._moveSpeed = this._optimalMoveSpeed;
    }

    private void FixedUpdate()
    {
        if (!this._play)
            return;

        foreach (Path path in this._inUsePaths)
        {
            if (path.ShouldDestroy)
                this._toDestroyPaths.Add(path);
            else
                path.transform.Translate(0, 0, - this._moveSpeed * Time.fixedDeltaTime);
        }

        foreach (Path path in this._toDestroyPaths)
        {
            this._inUsePaths.Remove(path);
            Destroy(path.gameObject);
        }
        this._toDestroyPaths.Clear();
    }

    public void OnUserSpeedChange()
    {
        this._moveSpeed = this._userSpeedVariable.Value / (this._userOptimalSpeedVariable.Value / this._optimalMoveSpeed);
    }

    public void Resume()
    {
        this._play = true;
    }

    public void Stop()
    {
        this._play = false;
    }

    public void AddNewPath()
    {
        if (this._inUsePaths.Count >= this._generateForwardCount + 1)
            return;
        Path tmp;
        Vector3 pos;
        if (this._inUsePaths.Count == 0)
        {
            tmp = Instantiate(this._startPath, this._pathContainer.transform);
            pos = this._pathContainer.transform.position;
        }
        else
        {
            tmp = Instantiate(this._paths[Random.Range(0, this._paths.Count)], this._pathContainer.transform);
            pos = this._inUsePaths[this._inUsePaths.Count - 1].EndPath.position;
        }
        tmp.transform.position = pos;
        if (this._invert)
        {
            Vector3 scale = tmp.transform.localScale;
            scale.x = -scale.x;
            tmp.transform.localScale = scale;
        }
        tmp.DestroyCollider = this._activePathCollider;
        tmp.Spawner = this;
        this._inUsePaths.Add(tmp);
    }
}
