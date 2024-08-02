using System.ComponentModel;

namespace Demomann.common.enums;

public enum ProfileActionTypeEnum {
    [Description("Login")]
    Login = 1,
    [Description("Register")]
    Register = 2,
    [Description("Change Password")]
    ResetPassword = 3,
    [Description("Update Profile")]
    UpdateProfile =4,
    [Description("Change Email Account")]
    ChangeEmail = 5

}

public class ProfileActionType{
    public int ActionId { get; set; }

    public string ActionName {get; set;}

    public string ActionDescription {get; set;}
}