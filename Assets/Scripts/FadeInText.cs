using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadeInText : MonoBehaviour {

    public float AngleMultiplier = 1.0f;
    public float SpeedMultiplier = 1.0f;
    public float CurveScale = 1.0f;

    private TMP_Text m_TextComponent;
    private bool _hasTextChanged;

    // Use this for initialization
    void Start()
    {
        m_TextComponent = GetComponent<TMP_Text>();
        // StartCoroutine(AnimateVertex());
    }

    /// <summary>
    /// Structure to hold pre-computed animation data.
    /// </summary>
    private struct VertexAnim
    {
        public float speed;
    }

    void OnEnable()
    {
        // Subscribe to event fired when text object has been regenerated.
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);
    }

    void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ON_TEXT_CHANGED);
    }

    void ON_TEXT_CHANGED(Object obj)
    {
        if (obj == m_TextComponent)
            _hasTextChanged = true;
    }


    /// <summary>
    /// Method to animate vertex colors of a TMP Text object.
    /// </summary>
    /// <returns></returns>
    public IEnumerator AnimateVertexColors()
    {
        int visibleCount = 0;
        TMP_TextInfo textInfo = m_TextComponent.textInfo;

        Color32[] newVertexColors;
        Color32 c0 = m_TextComponent.color;

        while (visibleCount < textInfo.characterCount)
        {
            int characterCount = textInfo.characterCount;

            // If No Characters then just yield and wait for some text to be added
            if (characterCount == 0)
            {
                yield return new WaitForSeconds(0.25f);
                continue;
            }

            // Get the index of the material used by the current character.
            int materialIndex = textInfo.characterInfo[visibleCount].materialReferenceIndex;

            // Get the vertex colors of the mesh used by this text element (character or sprite).
            newVertexColors = textInfo.meshInfo[materialIndex].colors32;

            // Get the index of the first vertex used by this text element.
            int vertexIndex = textInfo.characterInfo[visibleCount].vertexIndex;

            // Only change the vertex color if the text element is visible.
            if (textInfo.characterInfo[visibleCount].isVisible)
            {
                //c0 = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
                c0 = new Color32(255, 255, 255, 255);

                newVertexColors[vertexIndex + 0] = c0;
                newVertexColors[vertexIndex + 1] = c0;
                newVertexColors[vertexIndex + 2] = c0;
                newVertexColors[vertexIndex + 3] = c0;

                m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            }

            yield return null;
            visibleCount += 1;
        }
    }



    /// <summary>
    /// Method to animate vertex colors of a TMP Text object.
    /// </summary>
    /// <returns></returns>
    public IEnumerator AnimateVertex()
    {
        // We force an update of the text object since it would only be updated at the end of the frame. Ie. before this code is executed on the first frame.
        // Alternatively, we could yield and wait until the end of the frame when the text object will be generated.
        m_TextComponent.ForceMeshUpdate();

        TMP_TextInfo textInfo = m_TextComponent.textInfo;

        Matrix4x4 matrix;

        int loopCount = 0;

        // Create an Array which contains pre-computed Angle Ranges and Speeds for a bunch of characters.
        VertexAnim[] vertexAnim = new VertexAnim[1024];
        for (int i = 0; i < 1024; i++)
        {
            vertexAnim[i].speed = Random.Range(1f, 3f);
        }

        // Cache the vertex data of the text object as the Jitter FX is applied to the original position of the characters.
        TMP_MeshInfo[] cachedMeshInfo = textInfo.CopyMeshInfoVertexData();

        while (true)
        {
            // Get new copy of vertex data if the text has changed.
            if (_hasTextChanged)
            {
                // Update the copy of the vertex data for the text object.
                cachedMeshInfo = textInfo.CopyMeshInfoVertexData();

                _hasTextChanged = false;
            }

            int characterCount = textInfo.characterCount;

            // If No Characters then just yield and wait for some text to be added
            while (characterCount == 0)
            {
                characterCount = textInfo.characterCount;
                yield return null;
            }

            for (int i = 0; i < characterCount; i++)
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                // Skip characters that are not visible and thus have no geometry to manipulate.
                if (!charInfo.isVisible)
                    continue;

                // Retrieve the pre-computed animation data for the given character.
                VertexAnim vertAnim = vertexAnim[i];

                // Get the index of the material used by the current character.
                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

                // Get the index of the first vertex used by this text element.
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                // Get the cached vertices of the mesh used by this text element (character or sprite).
                Vector3[] sourceVertices = cachedMeshInfo[materialIndex].vertices;

                // Determine the center point of each character.
                Vector2 charMidBasline = (sourceVertices[vertexIndex + 0] + sourceVertices[vertexIndex + 2]) / 2;

                // Need to translate all 4 vertices of each quad to aligned with middle of character / baseline.
                // This is needed so the matrix TRS is applied at the origin for each character.
                Vector3 offset = charMidBasline;

                Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;

                destinationVertices[vertexIndex + 0] = sourceVertices[vertexIndex + 0] - offset;
                destinationVertices[vertexIndex + 1] = sourceVertices[vertexIndex + 1] - offset;
                destinationVertices[vertexIndex + 2] = sourceVertices[vertexIndex + 2] - offset;
                destinationVertices[vertexIndex + 3] = sourceVertices[vertexIndex + 3] - offset;

                Vector3 jitterOffset = new Vector3(0, Mathf.Sin(Time.time * 5f + i), 0);

                matrix = Matrix4x4.TRS(jitterOffset * CurveScale, Quaternion.Euler(0, 0, 0), Vector3.one);

                destinationVertices[vertexIndex + 0] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 0]);
                destinationVertices[vertexIndex + 1] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 1]);
                destinationVertices[vertexIndex + 2] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 2]);
                destinationVertices[vertexIndex + 3] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 3]);

                destinationVertices[vertexIndex + 0] += offset;
                destinationVertices[vertexIndex + 1] += offset;
                destinationVertices[vertexIndex + 2] += offset;
                destinationVertices[vertexIndex + 3] += offset;

                vertexAnim[i] = vertAnim;
            }

            // Push changes into meshes
            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                m_TextComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }

            loopCount += 1;

            yield return new WaitForSeconds(0.1f);
        }
    }
}
