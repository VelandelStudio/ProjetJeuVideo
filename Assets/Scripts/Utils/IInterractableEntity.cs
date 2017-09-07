/** IInterractableEntity Interface
 * This Interface was created to make a gameObject interractable.
 * All gameobjects of the IInterractableEntity family are detected by the GameObjectDetector Script.
 * This Interface contains 2 Method : 
 * The ActivateInterractable(); should be used to interract with the gameobject.
 * The DisplayTextOfInterractable(); should be used to show to the player that an interraction is possible whith the gameObject.
 **/
public interface IInterractableEntity {
    void ActivateInterractable();
    void DisplayTextOfInterractable();
}
