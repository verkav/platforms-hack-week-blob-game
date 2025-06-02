using UnityEngine;
using UnityEngine.U2D;

public class SoftBody : MonoBehaviour
{
    #region Constants

    private const float SplineOffset = 0.5f;

    #endregion
    #region Fields

    [SerializeField] public SpriteShapeController spriteShape;
    [SerializeField] public Transform[] points;

    #endregion

    #region MonoBehaviour Callbacks
    private void Awake()
    {
        UpdateVertices();
    }

    private void Update()
    {
        UpdateVertices();
    }
    #endregion

    #region privateMethods

    private void UpdateVertices()
    {
        for(int i = 0; i < points.Length -1; i++)
        {
            Vector2 vertex = points[i].localPosition;
            Vector2 towardsCenter = (Vector2.zero - vertex).normalized;
            //since the collider is a circle, we need to offset the location of the spline vertex by the radius of the collider
            float colliderRadius = points[i].gameObject.GetComponent<CircleCollider2D>().radius;
            
            //if vertices get too close, we might get an illegal argument exception
            try
            {
                spriteShape.spline.SetPosition(i, (vertex - towardsCenter * colliderRadius));

            }
            catch
            {
                Debug.Log("Spline vertices too close together. Recalculate...");
                spriteShape.spline.SetPosition(i, (vertex - towardsCenter * (colliderRadius + SplineOffset)));
            }

            Vector2 leftTangent = spriteShape.spline.GetLeftTangent(i);
            Vector2 newRightTangent = Vector2.Perpendicular(towardsCenter) * leftTangent.magnitude;
            Vector2 newLeftTangent = Vector2.zero - (newRightTangent);
            
            spriteShape.spline.SetRightTangent(i, newRightTangent);
            spriteShape.spline.SetLeftTangent(i, newLeftTangent);
        }
    }
    

    #endregion
}
