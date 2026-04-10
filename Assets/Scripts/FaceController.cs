using UnityEngine;
using System.Collections.Generic;

public class FaceController : MonoBehaviour
{
    [Header("References")]
    public SkinnedMeshRenderer faceRenderer;

    [Header("Transition Settings")]
    public float transitionSpeed = 120f; // higher = faster transition

    private Mesh faceMesh;
    private float[] targetWeights;

    void Start()
    {
        if (faceRenderer == null)
        {
            Debug.LogError("FaceController: No SkinnedMeshRenderer assigned.");
            return;
        }

        faceMesh = faceRenderer.sharedMesh;

        if (faceMesh == null)
        {
            Debug.LogError("FaceController: No mesh found on the SkinnedMeshRenderer.");
            return;
        }

        targetWeights = new float[faceMesh.blendShapeCount];

        // Start with current weights as targets
        for (int i = 0; i < faceMesh.blendShapeCount; i++)
        {
            targetWeights[i] = faceRenderer.GetBlendShapeWeight(i);
        }
    }

    void Update()
    {
        if (faceRenderer == null || faceMesh == null) return;

        HandleInput();
        UpdateBlendShapesSmoothly();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ResetTargets();
            SetTargetShape("EyebrowDown_L", 47.5f);
            SetTargetShape("EyebrowDown_R", 47.5f);
            SetTargetShape("Blink_L", 20f);
            SetTargetShape("Blink_R", 20f);
            SetTargetShape("Smile_L", 80f);
            SetTargetShape("Smile_R", 80f);
            SetTargetShape("Closed Mouth", 100f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ResetTargets();
            SetTargetShape("EyebrowDown_L", 35f);
            SetTargetShape("EyebrowDown_R", 35f);
            SetTargetShape("Blink_L", 100f);
            SetTargetShape("Blink_R", 100f);
            SetTargetShape("Pursed Mouth", 100f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ResetTargets();
            SetTargetShape("EyebrowUp_L", 70f);
            SetTargetShape("EyebrowUp_R", 20f);
            SetTargetShape("Smile", 40f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ResetTargets();
        }
    }

    void UpdateBlendShapesSmoothly()
    {
        for (int i = 0; i < faceMesh.blendShapeCount; i++)
        {
            float currentWeight = faceRenderer.GetBlendShapeWeight(i);
            float newWeight = Mathf.MoveTowards(
                currentWeight,
                targetWeights[i],
                transitionSpeed * Time.deltaTime
            );

            faceRenderer.SetBlendShapeWeight(i, newWeight);
        }
    }

    void SetTargetShape(string shapeName, float weight)
    {
        int index = faceMesh.GetBlendShapeIndex(shapeName);

        if (index >= 0)
        {
            targetWeights[index] = weight;
        }
        else
        {
            Debug.LogWarning("Blend shape not found: " + shapeName);
        }
    }

    void ResetTargets()
    {
        for (int i = 0; i < targetWeights.Length; i++)
        {
            targetWeights[i] = 0f;
        }
    }
}