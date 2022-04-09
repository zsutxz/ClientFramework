--- ui 登陆窗体控制器


UILoginCtrl=UICtrl:New(UILoginForm.Instance())
local this =  UILoginCtrl

function UILoginCtrl.Start()
    this.OpenForm(this.form.name)
    local btnLogin = this.form.btnLogin;
    local btnRegister = this.form.btnRegister;
    this.AddButtonListener(btnLogin,this.OnLoginBtnCilck)
    this.AddButtonListener(btnRegister,this.OnRegsiterBtnCilck)
end

function UILoginCtrl.OnLoginBtnCilck()
    --获取输入框账号
    local account = this.form.IFAccount.text;
    --获取输入框密码
    local passwork = this.form.IFPassword.text;

    local protoClass =ProtocolByte()
    protoClass:AddString("Login")
    protoClass:AddString(account)
    protoClass:AddString(passwork)


 -- print("OnLoginBtnCilck")
end

function UILoginCtrl.OnRegsiterBtnCilck()
    print("OnRegsiterBtnCilck")
end




