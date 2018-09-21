using UnityEngine;
using UnityEngine.UI;

public class FloatingScoreController : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private FloatingScore _floatingScoreTextPrefab;
    [SerializeField] private float _minHorizontalDisplacement;
    [SerializeField] private float _maxHorizontalDisplacement;

    private void Awake()
    {
        if (this._playerTransform == null)
            Debug.LogWarning("_playerTransform not set in " + this.name);
        if (this._mainCamera == null)
        {
            Debug.Log("Camera not assigned, using Camera.main instead");
            this._mainCamera = Camera.main;
        }
    }

    public void NewFloatingScore()
    {
        Vector2 viewportPosition = this._mainCamera.WorldToViewportPoint(new Vector3(this._playerTransform.position.x + Random.Range(-.2f, .2f), this._playerTransform.position.y + Random.Range(-.2f, .2f), this._playerTransform.position.z + Random.Range(-.2f, .2f)));
        FloatingScore instance = Instantiate(this._floatingScoreTextPrefab);

        instance.transform.SetParent(this.transform, false);

        instance.GetComponent<RectTransform>().anchorMin = viewportPosition;
        instance.GetComponent<RectTransform>().anchorMax = viewportPosition;

        instance.Displacement = Random.Range(this._minHorizontalDisplacement, this._maxHorizontalDisplacement);
    }

    public void NewFloatingScoreUI()
    {
        FloatingScore instance = Instantiate(this._floatingScoreTextPrefab);

        instance.transform.SetParent(this.transform, false);

        instance.Displacement = Random.Range(this._minHorizontalDisplacement, this._maxHorizontalDisplacement);
    }
}
