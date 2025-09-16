using UnityEngine;
using UnityEngine.InputSystem;

public class cameraController : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject selectedTurret;

    void Update()
    {
        if(Mouse.current.leftButton.isPressed && selectedTurret == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if(Physics.Raycast(ray, out RaycastHit hitInfo, 1000f))
            {
                if (hitInfo.collider.CompareTag("Turret"))
                {
                    selectedTurret = hitInfo.transform.gameObject;
                    selectedTurret.GetComponent<turretController>().turretCamera.SetActive(true);
                    selectedTurret.GetComponent<turretController>().controlledByPlayer = true;

                    mainCamera.SetActive(false);
                }
            }
        }
        else if (selectedTurret != null && Keyboard.current.escapeKey.isPressed)
        {
            mainCamera.SetActive(true);
            selectedTurret.GetComponent<turretController>().turretCamera.SetActive(false);
            selectedTurret.GetComponent<turretController>().controlledByPlayer = false;
            selectedTurret = null;
        }
    }
}
