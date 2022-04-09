UIDistrictCtrl=UICtrl:New(UIDistrictForm.Instance())
local this =  UIDistrictCtrl

function UIDistrictCtrl.Start()
    -- print("UILoginCtrl 开始")
    this.OpenForm(this.form.name)
    local btnTeach = this.form.btnTeach
    local btnDrill= this.form.btnDrill
    local btnExam = this.form.btnExam

    this.AddButtonListener(btnTeach,this.OnTeachBtnCilck)
    this.AddButtonListener(btnDrill,this.OnDrillBtnCilck)
    this.AddButtonListener(btnExam,this.OnExamBtnCilck)
end

function UIDistrictCtrl.OnTeachBtnCilck()
    this.RemoveButtonAllListener(this.form.Button)
    print("OnTeachBtnCilck")
end

function UIDistrictCtrl.OnDrillBtnCilck()
    this.RemoveButtonAllListener(this.form.Button)
    print("OnTeachBtnCilck")
end

function UIDistrictCtrl.OnExamBtnCilck()
    this.RemoveButtonAllListener(this.form.Button)
    print("OnTeachBtnCilck")
end