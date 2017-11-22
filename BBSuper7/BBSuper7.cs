using System;
using System.Text;
using System.Reflection;

using System.Threading;
using Newtonsoft.Json;
using WebSocket4Net;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;

[assembly: Obfuscation(Feature = "encrypt resources [compress]", Exclude = false)]
namespace BBSuper7
{
    ///------------------------------------------------
    /// <summary>连环夺宝客户端类</summary>
    ///------------------------------------------------
    [Obfuscation(Feature = "Apply to member * when method or constructor: virtualization", Exclude = false)]
    class BBSuper7
    {
        /// <summary>websocket客户端对象</summary>
        private WebSocket websocket = null;
        /// <summary>连接状态</summary>
        public bool ConnectStatus { get { return websocket != null && websocket.State == WebSocketState.Open; } }
        /// <summary>用户登录名</summary>
        public string UserLoginName { get; private set; } = string.Empty;
        /// <summary>用户ID</summary>
        public int UserId { get; private set; } = 0;
        /// <summary>大厅ID</summary>
        public string HallId { get; private set; } = string.Empty;
        /// <summary>用户金钱</summary>
        public double UserBalance { get; private set; } = 0;
        /// <summary>用户分数 初始设定500</summary>
        public int UserCredit { get; private set; } = 500;
        /// <summary>等级ID</summary>
        public int LevelID { get; private set; } = 0;
        /// <summary>投注总次数</summary>
        public int BetSum { get; private set; } = 0;
        /// <summary>流水总和</summary>
        public int WaterSum { get; private set; } = 0;
        /// <summary>西瓜数量</summary>
        public int WatermelonCount { get; private set; } = 0;
        /// <summary>BAR数量</summary>
        public int BarCount { get; private set; } = 0;
        /// <summary>西瓜奖金</summary>
        public double WatermelonAward { get; private set; } = 0;
        /// <summary>当前投注比例</summary>
        public string CurrentBetRatio { get; private set; } = Program.BetRatioA;
        /// <summary>当前投注注数</summary>
        public int CurrentBetCount { get; private set; } = Program.BetCount;
        /// <summary>当前每线投注</summary>
        public int CurrentLineBet { get; private set; } = Program.LineBet;


        ///------------------------------------------------
        /// <summary>发送消息给远程服务器</summary>
        ///------------------------------------------------
        ///
        [Obfuscation(Feature = "virtualization", Exclude = false)]
        private void Call(JObject command)
        {
            string text = JsonConvert.SerializeObject(command);
            websocket.Send(text);
        }

        ///------------------------------------------------
        /// <summary>接收服务器数据</summary>
        ///------------------------------------------------
        ///
        private void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            JObject message = JObject.Parse(e.Message);
            if (message["NetStatusEvent"].ToString() == "NetConnect.Success")
            {
                RequestLoginBySid("3a2428402ed932c5a33c0dccacaa031e11f3b3b1");
            }
            else if (!string.IsNullOrEmpty((string)message["key"]))
            {
                switch ((string)message["key"])
                {
                    case "onLoginBySid": ResponseLogin(message); break;
                    //case "onGetMachineDetail": ResponseGetMachine(message); break;
                    //case "onCreditExchange": ResponseCreditExchange(message); break;
                    //case "onCreateFish": ResponseonCreateFish(message); break;
                    //case "onKillShowgroup": ResponseonKillShowgroup(message); break;
                    //case "onBulletFire": ResponseBulletFire(message); break;
                    //case "onKillFish": ResponseonKillFish(message); break;
                }
            }
        }
        public void RequestLoginBySid(string sid)
        {
            JObject loginRequest = new JObject();
            loginRequest["key"] = "loginBySid";
            loginRequest["data"] = new JObject();
            loginRequest["data"]["sid"] = sid;
            loginRequest["data"]["deviceInfo"] = new JObject();
            Call(loginRequest);
        }

        ///------------------------------------------------
        /// <summary>连接服务器登录游戏</summary>
        ///------------------------------------------------
        public void LoginGame()
        {
            Program.PrintMessage("开始连接服务器……");
            websocket = new WebSocket("ws://yhgj.com/fxgm/fxLB?gameType=30599_1");
            websocket.AllowUnstrustedCertificate = true;
            websocket.Opened += (object sender, EventArgs e) => { Console.WriteLine("连接服务器成功"); };
            websocket.Closed += (object sender, EventArgs e) => { Console.WriteLine("连接关闭"); };
            websocket.Error += (object sender, SuperSocket.ClientEngine.ErrorEventArgs e) => { Console.WriteLine(e.Exception.ToString()); };
            websocket.MessageReceived += websocket_MessageReceived;
            websocket.Open();
        }

        ///------------------------------------------------
        /// <summary>请求通过会话ID登录游戏</summary>
        ///------------------------------------------------
        //public void RequestLoginBySid(string sid)
        //{
        //    JObject loginRequest = new JObject();
        //    loginRequest["command"] = "loginBySid";
        //    loginRequest["args"] = new JArray();
        //    (loginRequest["args"]as JArray).Add(sid);
        //    (loginRequest["args"] as JArray).Add("5061");
        //    (loginRequest["args"] as JArray).Add("{\"rd\":\"rd1\",\"ua\":\"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.89 Safari/537.36\",\"os\":\"Windows 7\",\"srs\":\"2048x1152\",\"wrs\":\"1044x778\",\"dpr\":1.25,\"pl\":\"H5\",\"pf\":\"Chrome 62.0.3202.89\",\"wv\":\"false\",\"aio\":false,\"vga\":\"ANGLE (AMD Radeon (TM) R9 Fury Series Direct3D11 vs_5_0 ps_5_0)\",\"tablet\":false,\"cts\":1510546111064,\"mua\":\"\",\"dtp\":\"\"}"); 
        //    Call(loginRequest);
        //}

        ///------------------------------------------------
        /// <summary>登录游戏响应</summary>
        ///------------------------------------------------
        public void ResponseLogin(JObject message)
        {
            // {"command":"takeMachine","args":["0"]}
            JObject loadInfoRequest = new JObject();
            loadInfoRequest["command"] = "takeMachine";
            loadInfoRequest["args"] = new JArray();
            (loadInfoRequest["args"] as JArray).Add("0");
            Call(loadInfoRequest);
        }

        ///------------------------------------------------
        /// <summary>请求载入信息</summary>
        ///------------------------------------------------
        public void RequestLoadInfo()
        {
            JObject loadInfoRequest = new JObject();
            loadInfoRequest["command"] = "onLoadInfo2";
            Call(loadInfoRequest);
        }

        ///------------------------------------------------
        /// <summary>请求获取兑换比例</summary>
        ///------------------------------------------------
        public void RequestOnGetMachineDetail()
        {
            JObject loadInfoRequest = new JObject();
            loadInfoRequest["command"] = "getMachineDetail";
            Call(loadInfoRequest);
        }   

        ///------------------------------------------------
        /// <summary>请求兑换分数</summary>
        ///------------------------------------------------
        public void RequestCreditExchange(string betRatio, int credit)
        {
            JObject creditExchangeRequest = new JObject();
            creditExchangeRequest["command"] = "creditExchange";
            creditExchangeRequest["args"] = new JArray();
            (creditExchangeRequest["args"] as JArray).Add(betRatio);//"1:10"
            (creditExchangeRequest["args"] as JArray).Add("500");
            Call(creditExchangeRequest);
        }
        
        ///------------------------------------------------
        /// <summary>请求结算分数</summary>
        ///------------------------------------------------
        public void RequestBalanceExchange()
        {
            JObject balanceExchangeRequest = new JObject();
            balanceExchangeRequest["command"] = "balanceExchange";
            Call(balanceExchangeRequest);
        }

        ///------------------------------------------------
        /// <summary>请求开始游戏</summary>
        ///------------------------------------------------
        public void RequestBeginGame()
        {
            // 如果西瓜数量为1, 并且西瓜分值大于等于X分, 则退出游戏
            if (WatermelonCount == 1&& WatermelonAward >= Program.WatermelonAwardTemplate)
            {
                if (websocket.State == WebSocketState.Open)
                {
                     websocket.Close();
                     websocket.Dispose();
                }
            }
            
            // 如果西瓜数量为1, 则以72分(满注80分)开始转动
            if (WatermelonCount == 1)
            {
                CurrentLineBet = 9;
                CurrentBetCount = 8;
            }

            // 西瓜分值高于X, 则切换到B比例,转一次后西瓜分值会回到初始300
            if (WatermelonAward > Program.WatermelonAwardTemplate)
            {
                Program.PrintMessage("切换到B比例"); 
                CurrentBetRatio = Program.BetRatioB;
                RequestOnGetMachineDetail();
            }
            else if(UserCredit < 80)//剩余积分不足
            {
                RequestOnGetMachineDetail();
            }else
            {
                JObject beginGameRequest = new JObject();
                beginGameRequest["command"] = "beginGame2";
                beginGameRequest["args"] = new JArray();
                (beginGameRequest["args"] as JArray).Add(Program.SessionId);
                (beginGameRequest["args"] as JArray).Add(CurrentLineBet.ToString());
                (beginGameRequest["args"] as JArray).Add(CurrentBetCount.ToString());
                Call(beginGameRequest);
            }
        }
        
        ///------------------------------------------------
        /// <summary>请求结束游戏</summary>
        ///------------------------------------------------
        public void RequestEndGame(string wagersID)
        {
            JObject endGameRequest = new JObject();
            endGameRequest["command"] = "endGame";
            endGameRequest["args"] = new JArray();
            (endGameRequest["args"] as JArray).Add(Program.SessionId);
            (endGameRequest["args"] as JArray).Add(wagersID);
            Call(endGameRequest);
        }

        ///------------------------------------------------
        /// <summary>服务器准备就绪</summary>
        ///------------------------------------------------
        public void ResponseReady(JObject message)
        {
            // {"proxied":true,"error":"","amf":{"IsFlex":false,"Name":"_result","TransactionID":1,"Objects":[{"capabilities":127,"fmsVer":"FMS/3,5,7,7009","mode":1},{"code":"NetConnection.Connect.Success","data":{"version":"3,5,7,7009"},"description":"Connection succeeded.","level":"status","objectEncoding":0}],"Error":null}}
            if (message["amf"]["Objects"].ToString().IndexOf("succeeded") >-1)
            {
                RequestLoginBySid(Program.SessionId);
            }
        }
        
        ///------------------------------------------------
        /// <summary>登记桌台响应</summary>
        ///------------------------------------------------
        public void ResponseTakeMachine(JObject message)
        {
            // {"proxied":true,"error":"","amf":{"IsFlex":false,"Name":"onTakeMachine","TransactionID":0,"Objects":[null,{"data":{"gameCode":1},"event":true}],"Error":null}}
            RequestLoadInfo();
        }

        ///------------------------------------------------
        /// <summary>获取兑换比例</summary>
        ///------------------------------------------------
        public void ResponseOnGetMachineDetail(JObject message)
        {
            // {"proxied":true,"error":"","amf":{"IsFlex":false,"Name":"onTakeMachine","TransactionID":0,"Objects":[null,{"data":{"gameCode":1},"event":true}],"Error":null}}
            RequestBalanceExchange();
        }

        ///------------------------------------------------
        /// <summary>载入信息响应</summary>
        ///------------------------------------------------
        public void ResponseLoadInfo(JObject message)
        {
            //{"proxied":true,"error":"","amf":{"IsFlex":false,"Name":"onOnLoadInfo2","TransactionID":0,"Objects":[null,{"data":{"Balance":478.8,"Base":"1:10,1:5,1:2,1:1,2:1,5:1,10:1","Bell3Times":"1","BetBase":"","BetTotal":"1950534.5000","Cherry3Times":"4","Credit":"0.00","Currency":"RMB","DefaultBase":"1:1","Evolution7":"","Evolution7Lock":false,"ExchangeRate":"1","HallID":3.819831e+06,"LoginName":"d496f146689","MaxLineBet":10,"PayTotal":"1894119.7780","Percent":"99","PercentLow":"96","Status":"N","Test":false,"ThreeBar3Times":"10","UserID":1.49237885e+08,"WagersID":"0","Watermelon3Times":"6","event":true,"isCash":false},"event":true}],"Error":null}} 
            UserLoginName = (string)message["amf"]["Objects"][1]["data"]["LoginName"];
            UserBalance   = (double)message["amf"]["Objects"][1]["data"]["Balance"];
            WatermelonCount  = (int)message["amf"]["Objects"][1]["data"]["Watermelon3Times"];
            BarCount         = (int)message["amf"]["Objects"][1]["data"]["ThreeBar3Times"];
            Program.PrintMessage(string.Format("登录游戏成功, 西瓜数量:{0}, BAR数量:{1}", WatermelonCount, BarCount));

            //判断西瓜数量及BAR数量是否为指定值，则以A比例入场，反之关闭连接, 新建连接继续扫描桌台
            if (WatermelonCount == Program.WatermelonTemplate && BarCount == Program.BarTemplate)
            {
            //if (WatermelonCount < Program.WatermelonTemplate && BarCount < Program.BarTemplate) //for test
            //{
                RequestCreditExchange(CurrentBetRatio, UserCredit);
                Program.PrintMessage(string.Format("登录游戏成功, 用户名:{0}, 用户ID:{1}, 账户余额:{2:F}", UserLoginName, UserId, UserBalance));
            }
            else
            {
                if (websocket.State == WebSocketState.Open)
                {
                    websocket.Close();
                    websocket.Dispose();
                }
            }
        }

        ///------------------------------------------------
        /// <summary>兑换分数响应</summary>
        ///------------------------------------------------
        public void ResponseCreditExchange(JObject message)
        {
            // {"proxied":true,"error":"","amf":{"IsFlex":false,"Name":"onCreditExchange","TransactionID":0,"Objects":[null,{"data":{"Balance":428.8,"Base":"1:10","Bell3Times":"5","BetBase":"1:10","Cherry3Times":"2","Credit":500,"Evolution7":"","Evolution7Lock":"","Super7_AB":"20614.04","Super7_AB_Temp":900,"Super7_AE":"42772.08","Super7_AE_Temp":3000,"Super7_AW":"2194.68","Super7_AW_Temp":300,"ThreeBar3Times":"7","Watermelon3Times":"13","event":true},"event":true}],"Error":null}}
            UserCredit = (int)message["amf"]["Objects"][1]["data"]["Credit"];
            UserBalance = (double)message["amf"]["Objects"][1]["data"]["Balance"];
            WatermelonAward =double.Parse(message["amf"]["Objects"][1]["data"]["Super7_AW_Temp"].ToString());
            Program.PrintMessage(string.Format("兑换分数成功, 用户分数:{0:F}, 账户余额:{1:F}", UserCredit, UserBalance));
            RequestBeginGame();
        }

        ///------------------------------------------------
        /// <summary>结算分数响应</summary>
        ///------------------------------------------------
        public void ResponseBalanceExchange(JObject message)
        {
            // {"proxied":true,"error":"","amf":{"IsFlex":false,"Name":"onBalanceExchange","TransactionID":0,"Objects":[null,{"data":{"Amount":49.9,"Balance":479.1,"BetBase":"","TransCredit":"499.00","event":true},"event":true}],"Error":null}}
            UserBalance = (double)message["amf"]["Objects"][1]["data"]["Balance"];
            Program.PrintMessage(string.Format("结算分数成功, 账户余额:{0:F}", UserBalance));
            RequestCreditExchange(CurrentBetRatio, UserCredit);
        }

        ///------------------------------------------------
        /// <summary>开始游戏响应</summary>
        ///------------------------------------------------
        public void ResponseBeginGame(JObject message)
        {
            //处理BET_AT_THE_SAME_TIME
            if (message["amf"]["Objects"][1]["error"]!=null&&message.ToString().IndexOf("SAME_TIME")>-1)
            {
                RequestBeginGame();
            }
            else
            {
                //刷新西瓜数量、西瓜分数、BAR数量
                WatermelonCount = (int)message["amf"]["Objects"][1]["data"]["Watermelon3Times"];
                WatermelonAward = (double)message["amf"]["Objects"][1]["data"]["Super7_AW_Temp"];
                BarCount = (int)message["amf"]["Objects"][1]["data"]["ThreeBar3Times"];
                UserCredit = (int)(double.Parse(message["amf"]["Objects"][1]["data"]["Credit"].ToString()));
                Program.PrintMessage(string.Format("开始游戏成功,用户分数:{0:F}", UserCredit));

                if (!string.IsNullOrEmpty(message["amf"]["Objects"][1]["data"]["FreeGame"].ToString()))//判断是否有免费自动翻滚,WagersID不同
                {
                    RequestEndGame((string)message["amf"]["Objects"][1]["data"]["FreeGame"]["BonusInfo"][0]["WagersID"]);
                }
                else
                {
                    RequestEndGame((string)message["amf"]["Objects"][1]["data"]["WagersID"]);
                }
            }
        }

        ///------------------------------------------------
        /// <summary>结束游戏响应</summary>
        ///------------------------------------------------
        public void ResponseEndGame(JObject message)
        {
            // {"action":"onEndGame","result":{"event":true,"data":{"Credit":"306.00","GameCode":18}}}
            //处理多次发结束包导致的异常
            if (message["amf"]["Objects"][1]["error"]!=null&&message["amf"]["Objects"][1]["error"].ToString().IndexOf("ENDED") > -1)
            {
                RequestBeginGame();
            }

            UserCredit = (int)(double.Parse(message["amf"]["Objects"][1]["data"]["Credit"].ToString()));
            WaterSum = WaterSum + CurrentLineBet * CurrentBetCount;
            Program.PrintMessage(string.Format("结束游戏成功, 用户分数:{0:F}, 投注流水:{1:F}", UserCredit, WaterSum));
            if (CurrentBetRatio == Program.BetRatioB) //B比例转一圈后, 切回A比例继续转动
            {
                Program.PrintMessage("切换回A比例");
                CurrentBetRatio = Program.BetRatioA;
                RequestOnGetMachineDetail();
            }else
            {
                RequestBeginGame();
            }
        }
    }
}

