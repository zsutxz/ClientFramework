---选区窗体

UIDistrictForm = UIForm :New(UIFormNames.UIDistrictForm)
local this = UIDistrictForm
function UIDistrictForm.Awake(uGameObject)
    this.gameobject = uGameObject
    this.transform = uGameObject.transform
    this. btnTeach = this.GetChildByName("BtnTeach") :GetComponent("Button")
    this. btnDrill= this.GetChildByName("BtnDrill") :GetComponent("Button")
    this. btnExam = this.GetChildByName("BtnExam") :GetComponent("Button")
end