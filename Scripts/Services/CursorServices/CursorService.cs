using UnityEngine;

namespace Services.CursorServices
{
  public class CursorService
  {
    public bool IsVisible { get; private set; }

    public CursorService()
    {
      IsVisible = true;
      Cursor.visible = true;
      Cursor.lockState = CursorLockMode.Confined;
    }

    public void ShowCursor()
    {
      if (IsVisible)
        return;

      Cursor.visible = true;
      Cursor.lockState = CursorLockMode.Confined;

      IsVisible = true;
    }

    public void HideCursor()
    {
      if (!IsVisible)
        return;

      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Locked;

      IsVisible = false;
    }
  }
}