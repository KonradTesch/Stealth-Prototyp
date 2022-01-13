using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private LayerMask raycastLayer;
    public float viewDistance = 6;
    public float fieldOfView = 80;

    [Header("Fill Animation")]

    public Texture2D[] fillAnimation;
    public float fillSpeed = 50;

    private MeshRenderer rend;
    private float timer;

    private Mesh mesh;
    private Vector3 origin = Vector3.zero;
    private float startingAngle;

    private float playerAngle;
    private float fillProgress = 0;


    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        rend = GetComponent<MeshRenderer>();

        rend.material = new Material(Shader.Find("Standard"));
        rend.material.color = new Color(1, 0, 0, 0.7f);
        rend.material.mainTexture = fillAnimation[0];
        ChangeMatRenderModeToTransparent();
    }

    private void LateUpdate()
    {
        int rayCount = Mathf.RoundToInt(fieldOfView);
        float angle = startingAngle;
        float angleIncrease = fieldOfView / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 2];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;
        uv[0] = new Vector2(0.5f, 0);

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            
            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, GetVectorFromAngle(angle), viewDistance, raycastLayer);
            if (raycastHit2D.collider == null)
            {
                // no Hit
                vertex = origin + GetVectorFromAngle(angle) * viewDistance;

                if (i > 0)
                {
                    float xValue = i / (float)rayCount;
                    uv[vertexIndex] = new Vector2(xValue, 1);
                }
                else
                {
                    uv[vertexIndex] = new Vector2(0, 1);
                }

            }
            else
            {
                //hit Object

                vertex = raycastHit2D.point - (Vector2)transform.position;
                if (i > 0)
                {
                    float xValue = i / (float)rayCount;
                    float yValue = Vector2.Distance((Vector2)transform.position, raycastHit2D.point) / viewDistance;
                    uv[vertexIndex] = new Vector2(xValue, yValue);
                }
                else
                {
                    float yValue = Vector2.Distance((Vector2)transform.position, raycastHit2D.point) / viewDistance;
                    uv[vertexIndex] = new Vector2(0, yValue);
                }

            }

            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;

            angle -= angleIncrease;

        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(origin, Vector3.one * 1000f);

        transform.rotation = Quaternion.Euler(Vector3.zero);

        Vector2 playerVector = (Vector2)GameBehavior.player.transform.position - (Vector2)transform.position;
        playerAngle = GetAngleFromVectorFloat(playerVector);
        float playerDistance = Vector2.Distance(transform.position, GameBehavior.player.transform.position);
        if (playerDistance < viewDistance && playerAngle <= startingAngle && playerAngle >= startingAngle - fieldOfView)
        {
            RaycastHit2D raycastPloayerHit2D = Physics2D.Raycast(transform.position, GetVectorFromAngle(playerAngle), playerDistance, raycastLayer);
            if (raycastPloayerHit2D.collider == null)
            {
                FillAnimation();

                if (fillProgress >= playerDistance && !GameBehavior.win)
                    GameBehavior.gameOver = true;
            }
            else
            {
                timer = 0;
                rend.material.mainTexture = fillAnimation[0];

            }
        }
        else
        {
            timer = 0;
            rend.material.mainTexture = fillAnimation[0];
        }

    }

    
    public void SetViewDirection( Vector3 viewDirection)
    {
        startingAngle = GetAngleFromVectorFloat(viewDirection) + fieldOfView / 2f;
    }

    private void FillAnimation()
    {
        timer += Time.deltaTime;

        int textureIndex = Mathf.RoundToInt(timer * fillSpeed);
        if (textureIndex > (fillAnimation.Length - 1))
            textureIndex = fillAnimation.Length - 1;

        rend.material.mainTexture = fillAnimation[textureIndex];

        fillProgress = viewDistance * (float)textureIndex / (float)fillAnimation.Length;
    }

    private Vector3 GetVectorFromAngle(float angle)
    {
        // angle = 0 -> 360
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    private static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0)
            n += 360;

        return n;
    }
    private void ChangeMatRenderModeToTransparent()
    {
        rend.material.SetOverrideTag("RenderType", "Transparent");
        rend.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        rend.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        rend.material.SetInt("_ZWrite", 0);
        rend.material.DisableKeyword("_ALPHATEST_ON");
        rend.material.DisableKeyword("_ALPHABLEND_ON");
        rend.material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        rend.material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }


}
