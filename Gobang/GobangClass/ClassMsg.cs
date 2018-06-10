using System;
using System.Collections.Generic;
using System.Text;

namespace GobangClass
{
    /// <summary>
    /// ClassMsg 的摘要说明。
    /// </summary>
    [Serializable]
    public class ClassMsg
    {
        public String SID = "";//发送方编号
        public String SIP = "";//发送方IP
        public String SPort = "";//发送方端口号

        public String RID = "";//接收方编号
        public String RIP = "";//接收方IP
        public String RPort = "";//接收方端口号

        public SendKind sendKind = SendKind.SendNone;//发送消息类型，默认为无类型
        public MsgCommand msgCommand = MsgCommand.None;//消息命令
        public String msgID = "";//消息ID，GUID
        public byte[] Data;

        public String UserName;//用户名
        public String PassWord;//密码

        public Int32 CPhoto = 0;//头像
        public Int32 Fraction = 0;//分数
        public Int32 Sex = 0;//性别

        public String AreaMark = "0";//区号
        public String RoomMark = "0";//房间号
        public String DeskMark = "";//桌号
        public String SeatMark = "";//坐位
        public String UserCaption = "";//用户名称

        public String chessRow = "0";//棋子的行数
        public String chessClomn = "0";//棋子的列数
        public String chessType = "0";//棋子的类型(黑棋或白棋)

        public String MsgText = "";//语聊
    }

    /// <summary>
    /// 消息命令
    /// </summary>
    public enum MsgCommand
    {
        None,//无
        Registering,//用户注册
        Registered,//用户注册结束
        RegisterAt,//注册用户已存在
        Logining,//用户登录
        Logined,//用户登录结束
        SendToOne,//发送单用户
        SendToAll,//发送消息所有用户
        UserList,//用户列表
        UpdateState,//更新用户状态

        BeginToGame,//打开游戏
        BeginToGameL,//返回单个用户
        BeginToGameH,//群发

        EndToGame,//关闭游戏
        EndToGameL,//返回单个用户
        EndToGameH,//群发

        ComeToHall,//进入大厅
        ComeToHallL,//返回单个用户
        ComeToHallH,//群发

        BegingRival,//发送对手信息
        EndRival,//返回对手信息

        ExitToArea,//退出区域
        ExitToHall,//退出大厅

        ComeToSay,//语聊
        BeginJoin,//对决连接
        UPDataFract,//在数据库中更改分数
        OddLittle,//对决双方单聊
        GetGameF,//获取玩家的分数
        EndJoin,//连接完毕
        ExitJoin,//退出对决游戏
        Close,//退出
    }

    /// <summary>
    /// 发送类型
    /// </summary>
    public enum SendKind
    {
        SendNone,//无类型
        SendCommand,//发送命令
        SendMsg,//发送消息
    }
}
