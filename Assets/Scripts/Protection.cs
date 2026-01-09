using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Protection : MonoBehaviour
{
    private Tween _flashTween;
    [SerializeField] float interval = 5;

    private Material _material;
    private Color _originalColor;

    private void Awake()
    {
        _material = GetComponent<SpriteRenderer>().material;
        if (_material != null)
        {
            _originalColor = _material.color;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(Protect());
    }

    private void OnDisable()
    {
        StopFlashing();
    }

    IEnumerator Protect()
    {
        float timer = interval;
        while (timer > 0)
        {
            timer -= Time.deltaTime;

            if (_material != null)
            {
                if (timer <= 2f && _flashTween == null)
                {
                    Color targetColor = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 0);

                    _flashTween = _material.DOColor(targetColor, 0.2f)
                                                 .SetLoops(-1, LoopType.Yoyo)
                                                 .SetEase(Ease.Linear);
                }
            }
            yield return null;
        }

        StopFlashing();
        gameObject.SetActive(false);
    }

    void StopFlashing()
    {
        if (_flashTween != null)
        {
            _flashTween.Kill();
            _flashTween = null;
        }

        if (_material != null)
        {
            _material.color = _originalColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile"))
        {
            collision.gameObject.SetActive(false);
        }
    }
}