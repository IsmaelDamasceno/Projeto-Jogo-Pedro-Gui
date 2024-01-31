using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CursorSprite
{
    Default,
    Grab,
    Hidden
}
public static class CursorController
{

    public static void SetCursor(CursorSprite cursor)
    {
        Cursor.visible = true;
        switch(cursor)
        {
            case CursorSprite.Default:
                {
                    Texture2D defaultCursor = Resources.Load<Texture2D>("Cursor/mouse default");
                    Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
				}
				break;
			case CursorSprite.Grab:
				{
					Texture2D grabCursor = Resources.Load<Texture2D>("Cursor/mouse grab");
					Cursor.SetCursor(grabCursor, Vector2.zero, CursorMode.Auto);
				}
				break;
            case CursorSprite.Hidden:
                {
					Cursor.visible = false;
				}
				break;
		}
    }
}
