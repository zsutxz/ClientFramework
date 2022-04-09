--- 热补丁模块
HotFix={}
local this = HotFix
local isOpenHotFix = false --是否开启热补丁

--注册热补丁
function HotFix.HotFixUpdate()

        if(not isOpenHotFix) then
            return
        end
        print("启动热补丁")

          xlua.private_accessible(CS.Test_HotFix) --可以方法类中私有字段及方法
          xlua.hotfix
          (CS.Test_HotFix,
                  'Init',
                   function(self)  --self 代表Csharp中的类对象
                      self.m_image.color = CS.UnityEngine.Color.green
                   end
           )
end


--移除全部注册了的热补丁
function HotFix.RemoveAllHotFix()

    if(not isOpenHotFix) then
        return
    end
    print("注销热补丁")
    xlua.hotfix(CS.Test_HotFix,'Init',nil)
end