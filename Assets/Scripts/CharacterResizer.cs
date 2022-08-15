using System.Collections;
using TMPro;
using UnityEngine;

public class CharacterResizer : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _startCharSize;

    private float _baseCharSize = 1;
    private float _stepChangeCharSize = 0.1f;
    private float _countVerticesInChar = 4;

    private void OnEnable()
    {
        //ResizeAllCharacters();
    }

    private void ResizeAllCharacters()
    {
        for (int i = 0; i < _text.textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = _text.textInfo.characterInfo[i];
            if (charInfo.isVisible)
                ResizeCharacter(i);
        }
    }

    public void ResizeCharacter(int index)
    {
        StartCoroutine(IncreaseCharacter(index));
    }

    private IEnumerator IncreaseCharacter(int index)
    {
        _text.ForceMeshUpdate();

        TMP_TextInfo textInfo = _text.textInfo;
        TMP_MeshInfo[] cachedMeshInfoVertexData = textInfo.CopyMeshInfoVertexData();
        cachedMeshInfoVertexData = textInfo.CopyMeshInfoVertexData();
        int materialIndex = textInfo.characterInfo[index].materialReferenceIndex;
        int vertexIndex = textInfo.characterInfo[index].vertexIndex;
        Vector3[] sourceVertices = cachedMeshInfoVertexData[materialIndex].vertices;
        Vector3 offset = (sourceVertices[vertexIndex + 0] + sourceVertices[vertexIndex + 2]) / 2;
        Matrix4x4 matrix;
        float charSize = _startCharSize;

        while (charSize != _baseCharSize)
        {

            charSize = Mathf.MoveTowards(charSize, _baseCharSize, _stepChangeCharSize);
            //Debug.Log(charSize);
            //Debug.Log(index);


            Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;
            matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, Vector3.one * charSize);

            for (int i = 0; i < _countVerticesInChar; i++)
            {
                destinationVertices[vertexIndex + i] = sourceVertices[vertexIndex + i] - offset;
                destinationVertices[vertexIndex + i] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + i]);
                destinationVertices[vertexIndex + i] += offset;
            }

            textInfo.meshInfo[0].mesh.vertices = textInfo.meshInfo[0].vertices;
            _text.UpdateGeometry(textInfo.meshInfo[0].mesh, 0);


            yield return new WaitForSeconds(0.1f);
        }
    }
}
