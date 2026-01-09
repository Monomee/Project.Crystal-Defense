using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float maxDistance = 100f; 
    [SerializeField] private LayerMask obstacleLayer;  // Layer của các vật cản 
    [SerializeField] private Transform firePoint;      

    [Header("Components")]
    [SerializeField] private LineRenderer lineRenderer;

    [Header("Fire Laser Setting")]
    [SerializeField] ParticleSystem focus;
    [SerializeField] SpriteRenderer body;
    [SerializeField] float minDuration = 1;
    [SerializeField] float maxDuration = 2;
    [SerializeField] float interval = 2;
    private bool _isFiring = false;

    [Header("List of move")]
    [SerializeField] List<Transform> targets;

    private Tween _flashTween;

    private bool isHitting = false;
    void Start()
    {
        MoveAround();
        StartCoroutine(LaserCycle());
    }

    IEnumerator LaserCycle()
    {
        while (true)
        {
            _isFiring = false;
            lineRenderer.enabled = false;
            isHitting = false;
            focus.Play();
            //yield return new WaitForSeconds(interval);
            float timer = interval;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                if (body != null)
                {
                    if (timer <= 1f && _flashTween == null)
                    {
                        _flashTween = body.DOColor(Color.red, 0.2f)
                            .SetLoops(-1, LoopType.Yoyo);
                    }
                }
                yield return null;
            }

            // --- (FIRING) ---
            StopFlashing(); 
            if (body != null) body.color = Color.white;

            focus.Stop();
            _isFiring = true;
            lineRenderer.enabled = true;
            

            float duration = Random.Range(minDuration, maxDuration + 1);
            yield return new WaitForSeconds(duration);
        }
    }
    void StopFlashing()
    {
        if (_flashTween != null)
        {
            _flashTween.Kill();
            _flashTween = null;
        }
        if (body != null) body.color = Color.white;
    }
    void Update()
    {
        if (_isFiring)
        {
            ShootLaser();
        }
        Vector3 Look = transform.InverseTransformPoint(Crystal.instance.transform.position);
        float angle = Mathf.Atan2(Look.y, Look.x) * Mathf.Rad2Deg;

        transform.Rotate(0, 0, angle);
    }

    void ShootLaser()
    {
        Vector2 startPos = firePoint != null ? firePoint.position : transform.position;

        Vector2 direction = transform.right;

        // 1. Bắn Raycast
        // Raycast(điểm bắt đầu, hướng, độ dài, layer cần check)
        RaycastHit2D hit = Physics2D.Raycast(startPos, direction, maxDistance, obstacleLayer);

        // 2. Cập nhật điểm đầu của LineRenderer
        lineRenderer.SetPosition(0, startPos);

        if (hit.collider != null)
        {
            // hit
            // Laser dừng lại ngay tại điểm va chạm (hit.point)
            lineRenderer.SetPosition(1, hit.point);

            if (hit.collider.gameObject.layer == 3 && !isHitting)
            {
                Debug.Log("lose HP");
                Crystal.instance.TakeDamage(20f);
                isHitting = true;
            }
        }
        else
        {
            //not hit
            lineRenderer.SetPosition(1, startPos + direction * maxDistance);
            isHitting = false;
        }
    }
    void MoveAround()
    {
        Sequence pathSequence = DOTween.Sequence();
        pathSequence.Append(transform.DOMove(targets[1].position, 5)); 
        pathSequence.Append(transform.DOMove(targets[2].position, 5));
        pathSequence.Append(transform.DOMove(targets[3].position, 5));
        pathSequence.Append(transform.DOMove(targets[0].position, 5));

        pathSequence.SetEase(Ease.Linear); 
        pathSequence.SetLoops(-1);
    }

}