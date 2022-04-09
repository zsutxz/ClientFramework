--- 注册窗体控制器

UIRegsiterCtrl=UICtrl:New(UILoginForm.Instance())
local this =  UIRegsiterCtrl

function UIRegsiterCtrl.Start()
    -- print("UILoginCtrl 开始")
    this.OpenForm(this.form.name)
    local btnBase = this.form.btnBase
    local btnSub2 = this.form.btnSub2
    local btnSub3 = this.form.btnSub3

    this.AddButtonListener(btnBase,this.OnBaseBtnCilck)
    this.AddButtonListener(btnSub2,this.OnSub2BtnCilck)
    this.AddButtonListener(btnSub3,this.OnSub3BtnCilck)
end

function UIRegsiterCtrl.OnBaseBtnCilck()
    this.CloseForm()
    this.OpenForm(UIFormNames.UITypeForm)
    UICtrlMgr.GetCtrl(UICtrlNames.UITypeCtrl).Start() -- 设置控制权
    ----  this.RemoveButtonListener(this.form.Button,this.OnLoginBtnClick)
    this.RemoveButtonAllListener(this.form.Button)
    print("OnBaseBtnCilck")
end

function UIRegsiterCtrl.OnSub2BtnCilck()
    this.CloseForm()
    this.OpenForm(UIFormNames.UITypeForm)
    UICtrlMgr.GetCtrl(UICtrlNames.UITypeCtrl).Start() -- 设置控制权
    ----  this.RemoveButtonListener(this.form.Button,this.OnLoginBtnClick)
    this.RemoveButtonAllListener(this.form.Button)
    print("OnSub2BtnCilck")
end

function UIRegsiterCtrl.OnSub3BtnCilck()
    this.CloseForm()
    this.OpenForm(UIFormNames.UITypeForm)
    UICtrlMgr.GetCtrl(UICtrlNames.UITypeCtrl).Start() -- 设置控制权
    ----  this.RemoveButtonListener(this.form.Button,this.OnLoginBtnClick)
    this.RemoveButtonAllListener(this.form.Button)
    print("OnSub3BtnCilck")
end


