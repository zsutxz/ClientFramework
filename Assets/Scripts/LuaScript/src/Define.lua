---项目常数类
require("UIDefine")
ABMgr = CS.AssetBundleFramework.AssetBundleMgr.Instance
UIMgr =CS.UIFramework.UIManager.Instance
TransformHelper = CS.Helper.TransformHelper
GameObject = CS.UnityEngine.GameObject
UIHelper = CS.UIFramework.UIHelper
UnityUI = CS.UnityEngine.UI
ProtocolByte = CS.SocketClient.ProtocolByte
--随机数
function Random(min,max)
   --得到时间字符串
    local strTime = tostring(os.time())
   -- print(strTime)
    --得到一个字符串  如 abc ->>  cba
    local strRev = string.reverse(strTime)
  --  print(strRev)
    --得到前6位
    local strRandomTime = string.sub(strRev,1,6)
   -- print(strRandomTime)
    math.randomseed(strRandomTime)
    return math.random(min,max)
end