
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ToggleButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Color")]
    [SerializeField] protected Color unselectedColor;
    [SerializeField] protected Color selectedColor;

    [Header("Background Color")]
    [SerializeField] protected Color unselectedBackgroundColor;
    [SerializeField] protected Color selectedBackgroundColor;

    [Header("Animation controls")]
    [SerializeField] protected AnimationCurve selectionCurve;
    [SerializeField] protected float animationTime;

    [Header("Events")]
    [SerializeField] protected UnityEvent onClickEvent;

	protected float currentTime = 0f;
	protected int direction = 0;

	protected Image image;
	protected TextMeshProUGUI text;

	[HideInInspector] public int id;

    private void Start()
    {
        Init();
    }

    /// <summary>
    /// Método redirecionada do Start para que possa ser chamado nas classes derivadas
    /// </summary>
    protected void Init()
    {
		image = GetComponent<Image>();
		text = Utils.SearchObjectWithComponent<TextMeshProUGUI>(transform, "Text");

		text.color = unselectedColor;
		image.color = unselectedBackgroundColor;
	}

    /// <summary>
    /// Seta o estado de seleção do botão
    /// </summary>
    /// <param name="isSelected">estado de seleção (selecionado ou não selecionado)</param>
    public void SetSelected(bool isSelected)
    {
        direction = isSelected ? 1 : -1;
	}

    public void Update()
    {
        if (direction != 0)
        {
            currentTime += Time.deltaTime * direction;
            float percent = currentTime / animationTime;
            if ((percent > 1f && direction == 1) || (percent < 0f && direction == -1))
            {
                direction = 0;
            }
			percent = Mathf.Clamp(percent, 0f, 1f);
			float value = selectionCurve.Evaluate(percent);

            ApplyAnimations(value);
		}
    }

    protected virtual void ApplyAnimations(float value)
    {
		text.color = new(
				Mathf.Lerp(unselectedColor.r, selectedColor.r, value),
				Mathf.Lerp(unselectedColor.g, selectedColor.g, value),
				Mathf.Lerp(unselectedColor.b, selectedColor.b, value),
		        1f
		);

		image.color = new(
			Mathf.Lerp(unselectedBackgroundColor.r, selectedBackgroundColor.r, value),
			Mathf.Lerp(unselectedBackgroundColor.g, selectedBackgroundColor.g, value),
			Mathf.Lerp(unselectedBackgroundColor.b, selectedBackgroundColor.b, value),
			Mathf.Lerp(unselectedBackgroundColor.a, selectedBackgroundColor.a, value)
		);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.parent.parent.GetComponent<MenuController>().ForceSelect(id);
        CursorController.SetCursor(CursorSprite.Grab);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
		transform.parent.parent.GetComponent<MenuController>().ForceSelect(-1);
		CursorController.SetCursor(CursorSprite.Default);
	}

    public virtual void Interact()
    {
		if (!transform.parent.parent.gameObject.activeInHierarchy)
        {
            return;
        }
		onClickEvent.Invoke();
	}
}
