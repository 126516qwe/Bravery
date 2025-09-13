using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObject_DomainExpansion : SkillObject_Base
{
    private Skill_DomainExpansion domainManager;

    private float expandSpeed = 2;
    private float duration;

    private float slowDownPercent = .9f;

    private Vector3 targetScale;
    private bool isShringking;
    public void SetupDomain(Skill_DomainExpansion domainExpansion)
    {
        this.domainManager = domainExpansion;

        duration = domainManager.GetDomainDuration();
        slowDownPercent = domainManager.GetSlowPercentage();
        expandSpeed = domainManager.expandSpeed;
        float maxSize= domainManager.maxDomainSize;

        targetScale = Vector3.one * maxSize;
        Invoke(nameof(ShrinkDomain), duration);
    }
    private void Update()
    {
        HandleScaling();
    }
    private void HandleScaling()
    {
        float sizeDiffrence = Mathf.Abs(transform.localScale.x - targetScale.x);
        bool shouldChangeScale = sizeDiffrence > 0.1f;

        if (shouldChangeScale)
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, expandSpeed * Time.deltaTime);

        if (isShringking && sizeDiffrence < 0.1f)
        {
            TerminateDomain();
        }
    }

    private void TerminateDomain()
    {
        domainManager.ClearTargets();
        Destroy(gameObject);
    }

    private void ShrinkDomain()
    {
        targetScale = Vector3.zero;
        isShringking = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if(enemy == null)
            return;

        domainManager.AddTarget(enemy);
        enemy.SlowDownEntity(duration,slowDownPercent,true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if(enemy == null)
            return;

        enemy.StopSlowDown();
    }
}
