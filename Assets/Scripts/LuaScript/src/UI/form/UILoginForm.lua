--- 登陆窗体


UILoginForm= UIForm :New(UIFormNames.UILoginForm)
local this = UILoginForm

function UILoginForm.Awake(uGameObject)
    this.gameobject = uGameObject
    this.transform = uGameObject.transform
    this. btnLogin = this.GetChildByName("BtnLogin") :GetComponent("Button")
    this. btnRegister = this.GetChildByName("BtnRegister") :GetComponent("Button")
    this. IFAccount = this.GetChildByName("account") :GetComponent("InputField")
    this. IFPassword = this.GetChildByName("password") :GetComponent("InputField")
end



