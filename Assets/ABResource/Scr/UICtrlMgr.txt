--- ui控制器管理器
UICtrlMgr={}
local this = UICtrlMgr

function UICtrlMgr.Init()
   this.ImportCtrl()
   this.LoadCanvas()
end

function UICtrlMgr.GetCtrl(ctrlName)  -- 获取控制器
    this.CurCtrl =UICtrlMgr[ctrlName]
    return  this.CurCtrl
end

--引入控制器
function UICtrlMgr.ImportCtrl()
    require("UICtrl")

    for key,value in pairs(UICtrlNames) do   --引用模块
         require(tostring(value))
    end

    this[UICtrlNames.UILoginCtrl] = UILoginCtrl.Instance()
    this[UICtrlNames.UIDistrictCtrl] = UIDistrictCtrl.Instance()
    this[UICtrlNames.UIRegsiterCtrl] = UIRegsiterCtrl.Instance()
  --  print(  this[UICtrlNames.UILoginCtrl],5)
end

--加载UICanvas
function UICtrlMgr.LoadCanvas()
    UIMgr:LoadCanvas(this.OnFinishLoadCanvas) -- 启动unity ui框架
end

--UICanvas加载完成
function UICtrlMgr.OnFinishLoadCanvas(canvas)
   -- UILoginCtrl.Start()
    local ctrl = this.GetCtrl(UICtrlNames.UILoginCtrl)
    if(ctrl) then
        this.GetCtrl(UICtrlNames.UILoginCtrl).Start()
    end


end
