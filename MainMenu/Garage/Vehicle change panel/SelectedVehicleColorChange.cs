// using UnityEngine;

// public class SelectedVehicleColorChange : MonoBehaviour
// {

//     private Image[] backgrounds;

//     void Awake()
//     {
//         GetGameObjectsWithImage();
//     }
//     private void OnGUI()
//     {
//         for (int i = 0; i < backgrounds.Length; i++)
//         {
//             if (backgrounds[i].gameObject.name == PlayerPrefs.GetString("currentCar"))
//             {
//                 backgrounds[i].color = new Color32(60, 243, 49, 255);
//             }
//             else
//             {
//                 backgroundColor.color = new Color32(170, 255, 245, 107);
//             }
//         }
//     }

//     private GameObject[] GetGameObjectsWithImage()
//     {

//         Image[] backgrounds = GetComponentsInChildren<Image>();

//     }
// }