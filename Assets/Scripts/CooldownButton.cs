using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CooldownButton : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float cooldownTime = 10f;

    [Header("UI References")]
    [SerializeField] private Image cooldownOverlay;
    [SerializeField] private Button targetButton;

    private bool _isCooldown = false;
    private Sequence _cooldownSequence;
    private Vector3 _initialScale; 

    void Awake()
    {
        if (targetButton == null) targetButton = GetComponent<Button>();
        _initialScale = targetButton.transform.localScale;
    }

    void Start()
    {
        if (cooldownOverlay != null)
        {
            cooldownOverlay.fillAmount = 0;
            cooldownOverlay.gameObject.SetActive(false);
        }

        targetButton.onClick.AddListener(StartCooldown);
    }

    public void StartCooldown()
    {
        if (_isCooldown) return;
        _isCooldown = true;

        cooldownOverlay.gameObject.SetActive(true);
        cooldownOverlay.fillAmount = 1;
        targetButton.interactable = false;

        targetButton.transform.DOKill(true);
        _cooldownSequence?.Kill();

        _cooldownSequence = DOTween.Sequence();

        _cooldownSequence
            .Append(cooldownOverlay.DOFillAmount(0, cooldownTime).SetEase(Ease.Linear))

            .Append(targetButton.image.DOColor(Color.white * 1.5f, 0.1f).SetLoops(2, LoopType.Yoyo))
            .Join(targetButton.transform.DOPunchScale(Vector3.one * 0.1f, 0.3f, 10, 1))

            .OnComplete(() => {
                ResetButtonState();
            });

        _cooldownSequence.SetTarget(this);
    }

    private void ResetButtonState()
    {
        _isCooldown = false;
        targetButton.interactable = true;
        cooldownOverlay.gameObject.SetActive(false);

        targetButton.transform.localScale = _initialScale;
        targetButton.image.color = Color.white;
    }

    private void OnDisable()
    {
        _cooldownSequence?.Kill();
        targetButton.transform.DOKill();
        DOTween.Kill(this);
    }
}