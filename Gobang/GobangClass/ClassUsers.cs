using System;
using System.Collections.Generic;
using System.Text;

namespace GobangClass
{
    /// <summary>
    /// 记录多人的信息
    /// </summary>
    [Serializable]                                  //指示一个类可以序列化
    public class ClassUsers : System.Collections.CollectionBase
    {
        public void add(ClassUserInfo userInfo)         //将当前用户信息添加到列表中
        {
            base.InnerList.Add(userInfo);
        }
        public void Romove(ClassUserInfo userInfo)      //在列表中移除指定的用户
        {
            base.InnerList.Remove(userInfo);
        }
        public ClassUserInfo this[int index]                //根据索引号，在列表中查找指定的用户信息
        {
            get
            {
                return ((ClassUserInfo)List[index]);
            }
            set
            {
                List[index] = value;
            }
        }
    }
}
