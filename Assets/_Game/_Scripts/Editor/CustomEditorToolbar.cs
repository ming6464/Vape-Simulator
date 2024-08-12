// using UnityEditor;
// using UnityEngine;
//
// [InitializeOnLoad]
// public static class CustomToolbarButton
// {
//     static CustomToolbarButton()
//     {
//         // Đăng ký phương thức OnToolbarGUI để vẽ GUI trên thanh công cụ
//         EditorApplication.update += OnUpdate;
//     }
//
//     private static void OnUpdate()
//     {
//         // Đảm bảo rằng chúng ta đang ở chế độ Editor hoặc Playmode và chỉ vẽ một lần
//         if (EditorApplication.isPlayingOrWillChangePlaymode)
//         {
//             return;
//         }
//
//         // Vẽ GUI khi ở mọi chế độ
//         DrawToolbar();
//     }
//
//     private static void DrawToolbar()
//     {
//         // Thiết lập vị trí và kích thước của button
//         Rect rect = new Rect(Screen.width / 2, 0, 100, 20);
//
//         // Vẽ button
//         if (GUI.Button(rect, "My Button"))
//         {
//             // Hành động khi nhấn nút
//             Debug.Log("Button Pressed!");
//         }
//
//         // Đảm bảo layout được cập nhật
//         HandleUtility.Repaint();
//     }
// }