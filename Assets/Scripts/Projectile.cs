using DG.Tweening;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Indicator Setup")]
    [SerializeField] private GameObject indicatorPrefab;

    [SerializeField] private float edgePadding = 50f;

    private GameObject _myIndicator;       
    private RectTransform _indicatorRect;  
    private Canvas _mainCanvas;           
    private Camera _mainCam;
    private TrailRenderer _trail;

    private void Awake()
    {
        _mainCam = Camera.main;
        _trail = GetComponent<TrailRenderer>();

        _mainCanvas = FindObjectOfType<Canvas>();

        if (indicatorPrefab != null && _mainCanvas != null)
        {
            _myIndicator = Instantiate(indicatorPrefab, _mainCanvas.transform);
            _indicatorRect = _myIndicator.GetComponent<RectTransform>();

            _myIndicator.SetActive(false);
        }
    }

    private void OnEnable()
    {
        //if (_trail)
        //{
        //    _trail.Clear();
        //    _trail.enabled = true;
        //}
        if (_myIndicator != null) _myIndicator.SetActive(false);

        //MoveToCrystal();
    }

    private void OnDisable()
    {
        transform.DOKill();

        if (_trail) _trail.enabled = false;

        if (_myIndicator != null)
        {
            _myIndicator.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (_myIndicator != null) Destroy(_myIndicator);
    }

    void Update()
    {
        if (_myIndicator != null)
        {
            HandleIndicator();
        }
    }

    public void Fire(Vector3 startPos)
    {
        transform.position = startPos;

        if (_trail)
        {
            _trail.Clear();
            _trail.enabled = true;
        }

        MoveToCrystal();
    }

    void MoveToCrystal()
    {
        if (Crystal.instance != null)
        {
            transform.DOMove(Crystal.instance.transform.position, 2f)
                     .SetEase(Ease.Linear); //Random.Range(1f, 3f)
        }
    }

    void HandleIndicator()
    {
        Vector3 screenPos = _mainCam.WorldToScreenPoint(transform.position);

        bool isOnScreen = screenPos.x > 0 && screenPos.x < Screen.width &&
                          screenPos.y > 0 && screenPos.y < Screen.height;

        if (isOnScreen)
        {
            if (_myIndicator.activeSelf) _myIndicator.SetActive(false);
        }
        else
        {
            if (!_myIndicator.activeSelf) _myIndicator.SetActive(true);

            float x = Mathf.Clamp(screenPos.x, edgePadding, Screen.width - edgePadding);
            float y = Mathf.Clamp(screenPos.y, edgePadding, Screen.height - edgePadding);

            _indicatorRect.position = new Vector3(x, y, 0);

            //RotateIndicator(screenPos);
        }
    }

    void RotateIndicator(Vector3 targetScreenPos)
    {
        Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 dir = targetScreenPos - center;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        _indicatorRect.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
}