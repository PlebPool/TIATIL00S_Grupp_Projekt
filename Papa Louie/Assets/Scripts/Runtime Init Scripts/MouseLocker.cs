using UnityEngine;

namespace Runtime_Init_Scripts
{
    public static class MouseLocker
    {
        [RuntimeInitializeOnLoadMethod]
        public static void LockMouse()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}

