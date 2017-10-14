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

    public static KeyCode MoveForward  = KeyCode.Z;
    public static KeyCode MoveBackward = KeyCode.S;
    public static KeyCode StrafeLeft   = KeyCode.Q;
    public static KeyCode StrafeRight  = KeyCode.D;

    #endregion

    #region Actions

    public static KeyCode Activate = KeyCode.E;
    public static KeyCode SwitchCursorState = KeyCode.LeftAlt;
    #endregion

    #region Spells

    public static KeyCode ActiveSpell1 = KeyCode.Alpha1;
    public static KeyCode ActiveSpell2 = KeyCode.Alpha2;
    public static KeyCode ActiveSpell3 = KeyCode.Alpha3;
    public static KeyCode ActiveSpell4 = KeyCode.Alpha4;

    #endregion

}
