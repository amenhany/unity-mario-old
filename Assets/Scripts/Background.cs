using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] GameObject overworld1;
    [SerializeField] GameObject clouds1;
    [SerializeField] GameObject underground1;

    public void SwapBackgrounds(string world) {
        switch (world) {
            case "Overworld":
                overworld1.SetActive(true);
                clouds1.SetActive(true);
                underground1.SetActive(false);
                break;
            case "Underground":
                overworld1.SetActive(false);
                clouds1.SetActive(false);
                underground1.SetActive(true);
                break;
		}
	}
}
