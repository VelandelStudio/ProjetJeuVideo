using UnityEngine;

/** InputsProperties static class
 * This class regroups all differents shortcuts. 
 * In other classes, you should always use these static variables instead of raw Inputs.Keydown.
 * In this way, the player will be able to set his own shortcuts without breaking the link between the other scripts.
 * If you are looking for a shortcut which is not in this class, please create it in the good region.
 * You should comment the variable names is they are not explicit enough.
 **/
public static class InputsProperties {

    #region Movements

    public static KeyCode moveForward  = KeyCode.Z;
    public static KeyCode moveBackward = KeyCode.S;
    public static KeyCode strafeLeft   = KeyCode.Q;
    public static KeyCode strafeRight  = KeyCode.D;

    #endregion

    #region Actions

    public static KeyCode activate = KeyCode.E;

    #endregion

    #region Spells

    public static KeyCode spell1 = KeyCode.Alpha1;
    public static KeyCode spell2 = KeyCode.Alpha2;
    public static KeyCode spell3 = KeyCode.Alpha3;
    public static KeyCode spell4 = KeyCode.Alpha4;

    #endregion

}
