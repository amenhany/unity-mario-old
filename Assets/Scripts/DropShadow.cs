using UnityEngine;

public class DropShadow : MonoBehaviour
{
    private Vector2 offset = new Vector2(0.15f, -0.15f);

    private SpriteRenderer spriteCaster;
    private SpriteRenderer spriteShadow;

    private Transform _caster;
    private Transform _shadow;

    public Material shadowMaterial;

	void Start() 
    {
        _caster = transform;
        _shadow = new GameObject().transform;
        _shadow.parent = _caster;
        _shadow.gameObject.name = "Shadow";
        _shadow.localRotation = Quaternion.identity;

        spriteCaster = GetComponent<SpriteRenderer>();
        spriteShadow = _shadow.gameObject.AddComponent<SpriteRenderer>();

        spriteShadow.material = shadowMaterial;

        spriteShadow.sortingLayerName = spriteCaster.sortingLayerName;
	}

	void LateUpdate() 
    {
        _shadow.position = new Vector2(_caster.position.x + offset.x, _caster.position.y + offset.y);

        spriteShadow.sprite = spriteCaster.sprite;
        spriteShadow.sortingOrder = spriteCaster.sortingOrder - 1;

        if (!spriteCaster.enabled) spriteShadow.enabled = false;
        else spriteShadow.enabled = true;
    }
}
