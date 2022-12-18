using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnife : Knife
{
    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerKnife>(out PlayerKnife knife))
            _gameObject.SetActive(false);
        base.OnCollisionEnter2D(collision);
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerUnit>(out PlayerUnit unit))
        {
            ////gameManager.surikenSound.Play();
            //if (megaBallEffect.isPlaying)
            //{
            //    megaBallEffect.Clear();
            //    megaBallEffect.Stop();
            //}
            _gameObject.SetActive(false);
            base.OnTriggerEnter2D(collision);
        }
    }
}
