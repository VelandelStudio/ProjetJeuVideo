using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** ArtifactReceptacleMechanism, public class
 * this script is associated with the ArtifactReceptacle prefab
 * It is used to display the ChampionSelection panel
 **/
public class ArtifactReceptacleMechanism : ActivableMechanism
{
    [SerializeField] private GameObject _activableMenus;
    [SerializeField] private MeshRenderer _material;

    private GameObject _artifact;
    private GameObject _artifactInstance;
    private Color originalColor;
    private Vector3 originalArtifactposition;

    private Champion linkedChampion;
    public Champion LinkedChampion
    {
        get { return linkedChampion; }
        protected set { }
    }

    private void Start()
    {
        originalColor = _material.materials[0].color;
        _artifact = (GameObject)Resources.Load("Mechanisms/ArtifactOnReceptacle");
    }

    /** ActivateInterractable Method
     * This Method overrides the parent one.
     * It detects if the mechanism as not been activated yet.
     * After activation it launches the dunjon and then Destroyes itself to provide multiple launches.
     * Warning ! Only the script will be Destroyed, not the GameObject
     */
    public override void ActivateInterractable(Collider other)
    {
        if(_artifactInstance)
        {
            Destroy(_artifactInstance);
        }

        _artifactInstance = Instantiate(_artifact, transform);
        _activableMenus.SetActive(true);
        Color color = new Color(1f, 1f, 1f, 1f);
        _material.materials[0].color = color;
    }

    public override void CancelTextOfInterractable(Collider other)
    {
        _activableMenus.SetActive(false);
        if (!other.GetComponent<Champion>())
        {
            _material.materials[0].color = originalColor;
            Destroy(_artifactInstance);
        }
        else
        {
            linkedChampion = other.gameObject.GetComponent<Champion>();
        }
    }

    public void DestroyArtifactPrefab()
    {
        Destroy(_artifactInstance);
        linkedChampion = null;
        _material.materials[0].color = originalColor;
    }
}
