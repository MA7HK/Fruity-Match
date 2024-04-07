using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BackgroundTile : MonoBehaviour
{
	public int _hitPoints;
	public Sprite[] _brakeTiles;
	public GameObject _brakeEffect;
	public AudioClip _destorySound;
	private SpriteRenderer _spriteRenderer;

    void Start() => _spriteRenderer = GetComponent<SpriteRenderer>();

    private void Update()
	{
		if(_hitPoints <= 0) { Destroy(this.gameObject); }
	}
	public void TakeDamage(int _damage)
	{
		_hitPoints -= _damage;
		print(_hitPoints);
		//MakeLighter();
		switch (_hitPoints)
		{
			case 1: _spriteRenderer.sprite = _brakeTiles[2]; break;
			case 2: _spriteRenderer.sprite = _brakeTiles[1]; break;
			case 3: _spriteRenderer.sprite = _brakeTiles[0]; break;
		}
		GameObject _destoryParticle = Instantiate(_brakeEffect, transform.position, Quaternion.identity);
		if(this.gameObject.tag == "Breakable")
		{
			SoundManager.soundfx.PlayRockBreakingSound(_destorySound);
		}
		else SoundManager.soundfx.PlayWoodBreakingSound(_destorySound);
		Destroy(_destoryParticle, 0.5f);
	}

	// void MakeLighter()
	// {
	// 	Color _color = _spriteRenderer.color;
	// 	float _newAlpha = _color.a * 0.5f;
	// 	_spriteRenderer.color = new Color(_color.r, _color.g, _color.b, _newAlpha);
	// }

}