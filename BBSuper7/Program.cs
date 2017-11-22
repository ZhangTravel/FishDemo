#define KSS
using System;
using System.IO;
using System.Threading;
using System.Reflection;
using CustomTools;
using CustomTools.DialogTools;
using System.Windows.Forms;


#if KSS
[assembly: Obfuscation(Feature = "merge with CopyrightProtected.dll", Exclude = false)]
#endif
[assembly: Obfuscation(Feature = "merge with CustomTools.dll", Exclude = false)]
[assembly: Obfuscation(Feature = "merge with Newtonsoft.Json.dll", Exclude = false)]
[assembly: Obfuscation(Feature = "encrypt symbol names with password Spider", Exclude = false)]
[assembly: Obfuscation(Feature = "encrypt resources", Exclude = false)]
namespace BBSuper7
{
    [Obfuscation(Feature = "Apply to member * when method or constructor: virtualization", Exclude = false)]
    class Program
    {
        /// <summary>游戏链接</summary>
        public static string GameUrl { get; private set; } = string.Empty;
        /// <summary>会话ID</summary>
        public static string SessionId { get; private set; } = string.Empty;
        /// <summary>携带金钱</summary>
        public static double CarryBalance { get; private set; } = 0;
        /// <summary>投注注数</summary>
        public static int BetCount { get; private set; } = 0;
        /// <summary>每线投注</summary>
        public static int LineBet { get; private set; } = 0;
        /// <summary>西瓜数量指定值</summary>
        public static int WatermelonTemplate { get; private set; } = 10;
        /// <summary>西瓜分值指定值</summary>
        public static double WatermelonAwardTemplate { get;  set; } = 500;
        /// <summary>BAR数量指定值</summary>
        public static int BarTemplate { get; private set; } = 10;
        /// <summary>A投注比例</summary>
        public static string BetRatioA { get; private set; } = "1:10";
        /// <summary>B投注比例</summary>
        public static string BetRatioB { get; private set; } = "1:5";
    
        ///------------------------------------------------
        /// <summary>打印消息文本, 并且保存消息文本到磁盘</summary>
        ///------------------------------------------------
        public static void PrintMessage(string msg)
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            msg = string.Format("{0}--->{1}", time, msg);
            Console.WriteLine(msg);
            StreamWriter writer = new StreamWriter("message.txt", true);
            writer.WriteLine(msg);
            writer.Close();
        }

        ///------------------------------------------------
        /// <summary>程序入口函数</summary>
        ///------------------------------------------------
        static void Main(string[] args)
        {
#if DEBUG
            BetCount = 1;
            LineBet = 1;
            CarryBalance = 100;
            GameUrl = "ws://777.acsstech.com/websocket/SlotMachine/service.mob";
            //SessionId from Cookie
            SessionId = "a8e29fc76d92b3679c9f01e24a43111c9b4825ed";
#else
#if KSS
            MiniDatabase config = new MiniDatabase();

            // 初始化验证参数
            string soft_code = "1000006";
            string soft_key = "tBH4ri98Q8Vl8YF2D23NR5zk";
            string rsamod = @"87511866381601393955758054040600923683586568809125777612870390917937184444201";
            string rsapubkey = @"65537";
            string sign_data = @"e8dee41144940964fa31f090abd2e61d94865a644480685d0085929ed339522e02427e0ca6f8c228433efc66e66df5dc6c79519594933538182c92e4004a70ac8ea9285c540a1080e2f7f4942cef6eefc0136ce2544c9908d19a4f0d20704e145143625c0eed52b055d5fc67fe607ee12663af8478737453c797bc7389c6f04a1fe448de4c7021f72c790a444ca54f3d0e58259034c75e8e4db6d9fdaf8ea240d3e51c0b468bda17ef213dd1a66ccb8a5e42f5771bc3b9db11ec48cf1a11230961a3b787a42c19e458b142d2328fc2ebb34c26b30d4f9f5f2629c783bff7a8840135a07f5a121c34df23c24ec2d85146800f1f732f321faabf247c009ce129de352a8030454f64604e35852215ec53bf853cf900308c8c506396a2b8651b1f110364f610fe7a7cb339b7da2d1a82b0afe23aed10fd93d85f1ab78c6f4d559cc393a882be0bb36ad694ace47bfd6880bfebde209f33e2989b4ee90598425bad94f6c033f7b9fed6b3a388dfdcb21daaa65987dd8dac98b55077ff8c1160b3a0442bb565bff5b82d856c624aa5d59d5b1f";
            string license_key = @"Be1kSDgKIKSKpa/C6mBkYY05JLD9BQEiu4KX2ZL7PJ0Rd0WnT258sOB0QqsOvmmOFVAAh0Jmb12sKPBquTF7Xy1/CbnQ1t8Z+VkgVvJXKvA0OQ4yYXTgTMNRJYcLUb+s7JhqEEoPyR6S+e8ITDThwQqFs8bZQ0U7Q3niU0vevSuf7907U837CW6eIg0EUeNlEUoTZ0aMm8p/LzEM6FgD6lreJb7+p6yNABG/237pf5BDrWiHkb7vzdiUGA6roawaraLLAvVcr+qUpaeRuxwOJNiNUhGL5L6M25aPRhCvkxPzZZPCq0/S1Jd9tSZbfU7xQAeAefOSyQdECGQGkx99E1tkslNbejY5cYLrj5ECR3L1q20LRnj04WckfdXLDC2sZSoI/IgJpvvjGijS1EJbaXT+7UsXtGV3bkwCH92H+e2C6h1zeB7Xr43fHAcFgmJuY+2+Vc/zFU/6dOXN0rrM/+U1uudmDnQ2Sw99Mq5gjTR6HyFYoe9EsdNKpZotOQebDl1YvAzaSnjmUyKYS2DB4gLPmKXgluYQBZyabPdUabkEGnScUUctPVOIVfEPw9hBoE9yucnwCyTYzN2dcI5b55ia68j/X7ReN/nVB9zFyeyo3gZmfcLXuk0pgsnkxFpSJ4+ITC9lw2GZUiqIFmeyPVhGHynskteAoBoWxHfbI7CgC6biS9dbbjH1KAuUkYNov86Ygv26Mkku2swJBgluWSnzcyj9KET82R+Ts0+OW6g8g+TOVxlAMqPfSoGT0/WJq2C2tNYBv4e9W4jwz2HKFHfqw0y3AS1Fvf9KuD0Ua9hdVQf38sg5Bz575xOhcYYf++Hd8RXZimR7mj0qkO1f29GCYehFgEqzNKpnKzBkrUkOfnBMATHqvCRUsxJ0Ya/K1ZppRgo7uCP0113DlosFyPwq2y65GmV6F02OuZcP2OdJGcBE0zFbDLXyWURG+nwc28gy+vmizQZeb+n7WtDdlw/5isuhimejZ6J/j0W5xtAlqwK/gKSHzOAa0r77h72PGMa5bt+WvxsNpz3SeOLKc/FErDmtaKQuKH/y322nwe2D9SG/Vz+LnFzx0wB6JD22s5nqi2z6VqF+agVanIpskRyloze4FlmqHTPyEYsfriYMK8vCg2EENJUByAARLZ8r7E/n3qoK8b00KB+ic3Hm0fFnQaFEJD9RnnjWLEp32xU2zyBBd6RUpHrJR7Nj6lOB2DA8IaQ3o30uUBclTvKuRxDxVTOE81ejmZR23iBlOKDW6330DzXVj/4TxCrFoX9Usm+5phqRK1FUwOJw83XSo8cEdDs1Cz5coumNkRqhjs9uJSsaPCoJ1l5D/iUYd/9nNKeTph9BY4HQ4+mlciz+n5HNbp3xIlTTw/N+nyIRgfyVl1KOeRJaovJjNzpSSB0yLpC3WXGAsTDm8ndH6sOAd9k2SQURTnyWZPmq7gU0gM7AkY6UqZABGmptnbPvn2hy5TvcQ2sY0s0ym0lwfucM24k7+XtObAGFw3c2piLeMmkUxelyc0z/Pn7lRfCz70SpeRvuYTGJZ7iHrXXsy09tMrqM27jvw792GZtXW1cO9Pgmmk1SsnAoClmO3SmxIQc4DdSMSXmO8AQtO4GxcvVfdKWxRh/RvcpOOUtLNwTUZ5sYeIAMmof68bwt0GNUC4PCzw+30pFSxo8P7qSqrIuj/gNQ6hjm+cM0DuRJkP6UJPE=";
            kssPlugin kss_object = new kssPlugin(soft_code, soft_key, rsamod, rsapubkey, sign_data, license_key);
            

            // 从配置文件中读取注册卡号
            string card_number = config["运行参数", "注册卡号"];

            // 验证注册卡号是否有效
            while (card_number.Length == 0 || kss_object.card_number_login(card_number, 1, string.Empty) != string.Empty)
            {
                // 请求用户输入注册卡号
                InputTextDialog dialog_object = new InputTextDialog("卡号输入", "注册卡号:");
                DialogResult dialog_result = dialog_object.ShowDialog();
                if (dialog_result == DialogResult.OK)
                    card_number = dialog_object.InputText;
                else
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

            // 将注册卡号保存至配置文件中
            config["运行参数", "注册卡号"] = card_number;
#endif
            //WatermelonTemplate = 10;
            //BarTemplte = 10;
            BetCount = int.Parse(config["运行参数", "投注注数"]);
            LineBet = int.Parse(config["运行参数", "线注金额"]);
            CarryBalance = int.Parse(config["运行参数", "携带金钱"]);
            GameUrl = config["运行参数", "游戏链接"];
            SessionId = config["运行参数", "游戏会话"];
#endif
           BBSuper7 gameClient = new BBSuper7();
            while (true)
            {   

                if (gameClient.ConnectStatus == false) gameClient.LoginGame();
                Thread.Sleep(1000);
            }
        }
    }
}
