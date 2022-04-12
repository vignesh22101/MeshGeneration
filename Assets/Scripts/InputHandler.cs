using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
    #region Variables

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Material polygonMaterial;

    private Polygon recentPolygon, copiedPolygon;
    private Camera mainCamera;

    #endregion

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && EventSystem.current.currentSelectedGameObject == null)
        {
            if (copiedPolygon != null)
            {
                copiedPolygon.moveWithMouse = false;
                recentPolygon = copiedPolygon = null;
            }

            if (copiedPolygon == null)
            {
                if (recentPolygon == null)
                    recentPolygon = new GameObject().AddComponent<Polygon>();

                Vector3 mousePos = Input.mousePosition;
                mousePos.z = 1;
                Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
                recentPolygon.AddVertice(worldPos);

                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, worldPos);
            }
        }
    }

    #region Invoked functions via buttons

    public void Clicked_CompleteButton()
    {
        if (recentPolygon != null)
            recentPolygon.ConstructPolygon(polygonMaterial);
    }

    public void Clicked_CopyButton()
    {
        copiedPolygon = Instantiate(recentPolygon.gameObject).GetComponent<Polygon>();
        copiedPolygon.moveWithMouse = true;
        lineRenderer.positionCount = 0;
    }

    #endregion
}
