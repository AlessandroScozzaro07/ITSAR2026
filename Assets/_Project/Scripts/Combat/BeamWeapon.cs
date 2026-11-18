using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class BeamWeapon : Weapon
{
    [SerializeField] LineRenderer LineRenderer;

    [SerializeField] Transform muzzle;

    [SerializeField] Color startColor = Color.red;

    [SerializeField] Color endColor = Color.purple;

    [SerializeField] float LineThickness = .25f;

    protected override void Awake()
    {
        base.Awake();
        if (LineRenderer) LineRenderer = GetComponentInParent<LineRenderer>();
    }
    protected override void Fire()
    {
        targeter?.CurrentTarget.TakeDamage(damage); // il ? vuol dire che chekka il targetet
    }
    protected override void Update()
    {
        base.Update();

        /* timer -= Time.deltaTime;
         if (timer > 0f) return;
         if (NeedsTarget && targeter.CurrentTarget == null) return;
         timer = cooldown;
         Fire();   // quale Fire? Lo decide la classe vera */
        if (LineRenderer)
        {
            if (targeter.CurrentTarget != null)
            {
                Vector3 LineStart = muzzle.position;
                Vector3 LineEnd = targeter.TargetPoint;

                LineRenderer.positionCount = 2;
                LineRenderer.SetPosition(0, LineStart);
                LineRenderer.SetPosition(1, LineEnd);

                LineRenderer.startColor = startColor;
                LineRenderer.endColor = endColor;
            }
            else
            {
                LineRenderer.positionCount = 0;
                LineRenderer.startColor = new Color(0, 0, 0, 0);
                LineRenderer.endColor = new Color(0, 0, 0, 0);
            }

        }

    }
}
