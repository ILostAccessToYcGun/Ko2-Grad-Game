using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class RhythmStylus : WeaponBase
{
    [SerializeField] GameObject m1Projectile;
    [SerializeField] GameObject m2Projectile;
    [Header("Mouse 1")]
    [SerializeField] float m1DmgMult = 1.0f;
    [SerializeField] float m1Knockback;
    public GameObject lastHit;
    bool m1Pressed = false;

    [Header("Mouse 2")]
    [SerializeField] float m2DmgMult = 2.0f;
    [SerializeField] float maxDragTime = 5.0f;
    [SerializeField] float currentDragTime = 5.0f;

    [SerializeField] float explosionTimer = 0.0f;
    public List<StylusDrag> explosions;
    public List<Vector3> explosionPos;
    bool m2Held = false;
    [SerializeField] float dragPlaceCD = 0.05f;
    [SerializeField] GameObject lastExplosive;
    [SerializeField] LineRenderer dragLine;

    [SerializeField] GameObject MouseTrail;
    [SerializeField] GameObject currentTrail;

    bool dragSound = false;


    private new void Start()
    {
        base.Start();
        UIManager.instance.ToggleStylusUI();
        UIManager.instance.UpdateStylusReserve(currentDragTime / maxDragTime);
        currentTrail = Instantiate(MouseTrail, GameManager.instance.cam.screenToWorld, Quaternion.identity);
        MusicManage.instance.SelectOSU();
    }
    protected override void OnUpdate()
    {
        currentTrail.transform.position = GameManager.instance.cam.screenToWorld;
        if (explosions.Count <= 0)
        {
            explosionPos.Clear();
            UpdateLine();
        }

        if (m2Held)
        {
            explosionTimer += Time.deltaTime;
            UIManager.instance.UpdateExplosionReserve(explosionTimer / currentDragTime);
            if (explosionTimer >= currentDragTime)
            {
                //expload everything
                ExploadDragNotes();
            }
            else if (m2.ReadValue<float>() == 0.0f)
            {
                ExploadDragNotes();
            }
        }
        else //we arent holding right click
        {
            currentDragTime += Time.deltaTime * 0.5f;
            if (currentDragTime > maxDragTime) currentDragTime = maxDragTime;
            UIManager.instance.UpdateStylusReserve(currentDragTime / maxDragTime);
        }

        if (m1.ReadValue<float>() == 0.0f)
        {
            m1Pressed = false;
        }

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime * GameManager.instance.playerStats.ATKSPD;
        }
        else
        {
            if (GameManager.instance.gameState != GameManager.States.Playing) return;
            Vector2 forwardVector = (GameManager.instance.cam.screenToWorld - (Vector2)playerMovement.transform.position).normalized;
            if (m1.ReadValue<float>() > 0.0f && !m1Pressed)
            {
                m1Pressed = true;
                //Stylus tap + alternate
                //
                StylusTap tap =
                            Instantiate(m1Projectile, GameManager.instance.cam.screenToWorld, Quaternion.identity)
                            .GetComponentInChildren<StylusTap>();

                tap.damage = GameManager.instance.playerStats.ATK * m1DmgMult;
                tap.knockback = m1Knockback;
                tap.parent = this;
                attackTimer = m1CD;

                AudioManager.instance.Play("OsuHit", 0.95f, 1.05f);
            }
            else if (m2.ReadValue<float>() > 0.0f)
            {
                
                //stulys drag + earthwake explosion
                m2Held = true;

                StylusDrag drag =
                            Instantiate(m2Projectile, GameManager.instance.cam.screenToWorld, Quaternion.identity)
                            .GetComponentInChildren<StylusDrag>();
                explosions.Add(drag);
                explosionPos.Add(drag.transform.position);
                drag.damage = GameManager.instance.playerStats.ATK * m2DmgMult;
                drag.triggerTime = explosionTimer;
                drag.parent = this;
                if (lastExplosive != null)
                   drag.front = lastExplosive;
                attackTimer = dragPlaceCD;
                lastExplosive = drag.gameObject;

                if (!dragSound)
                {
                    dragSound = true;
                    AudioManager.instance.Play("OsuHit", 0.75f, 0.85f);
                }
                UpdateLine();
            }
        }
    }

    void UpdateLine()
    {
        dragLine.positionCount = explosions.Count;
        dragLine.SetPositions(explosionPos.ToArray());
    }

    void ExploadDragNotes()
    {
        currentDragTime -= explosionTimer;
        if (currentDragTime < 0) currentDragTime = 0.0f;
        m2Held = false;
        attackTimer = explosionTimer + m2CD;
        explosionTimer = 0.0f;
        UIManager.instance.UpdateExplosionReserve(explosionTimer / currentDragTime);
        UIManager.instance.UpdateStylusReserve(currentDragTime / maxDragTime);
        AudioManager.instance.Play("OsuHit", 0.75f, 0.85f);
        dragSound = false;

        foreach (var drag in explosions)
        {
            drag.canTrigger = true;
            drag.TimeExplosion();
        }
    }
}
