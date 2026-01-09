using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    public static Crystal instance;
    private void OnEnable()
    {
        instance = this;
        _currentHP = maxHP;
    }
    private void OnDisable()
    {
        instance = null;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile"))
        {
            TakeDamage(10f);
            collision.gameObject.SetActive(false);
        }
    }

    [SerializeField] private float maxHP = 100f;
    private float _currentHP;

    public float CurrentHP => _currentHP;

    public void TakeDamage(float amount)
    {
        _currentHP -= amount;
        _currentHP = Mathf.Max(_currentHP, 0);

        EventHub.ChangeHP();

        if (_currentHP <= 0)
        {
            EventHub.RaiseLose();
        }
    }
}
