using System;
using System.Collections.Generic;
using System.Text;

namespace GobangClass
{
    /// <summary>
    /// ��¼���˵���Ϣ
    /// </summary>
    [Serializable]                                  //ָʾһ����������л�
    public class ClassUsers : System.Collections.CollectionBase
    {
        public void add(ClassUserInfo userInfo)         //����ǰ�û���Ϣ��ӵ��б���
        {
            base.InnerList.Add(userInfo);
        }
        public void Romove(ClassUserInfo userInfo)      //���б����Ƴ�ָ�����û�
        {
            base.InnerList.Remove(userInfo);
        }
        public ClassUserInfo this[int index]                //���������ţ����б��в���ָ�����û���Ϣ
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
