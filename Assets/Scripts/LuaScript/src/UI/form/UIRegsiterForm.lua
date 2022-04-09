--- 注册窗体
UIRegsiterForm = UIForm :New(UIFormNames.UIRegsiterForm)
local this = UIRegsiterForm
function UIRegsiterForm.Awake(uGameObject)
    this.gameobject = uGameObject
    this.transform = uGameObject.transform
    this. btnTeach = this.GetChildByName("BtnTeach") :GetComponent("Button")
    this. btnDrill= this.GetChildByName("BtnDrill") :GetComponent("Button")
    this. btnExam = this.GetChildByName("BtnExam") :GetComponent("Button")
end
