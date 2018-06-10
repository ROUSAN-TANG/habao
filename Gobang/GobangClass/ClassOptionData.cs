using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;

namespace GobangClass
{
    /// <summary>
    /// ��SQL�������ݿ��е���Ϣ������ӡ��޸ļ���ѯ�Ĳ���
    /// </summary>
    public class ClassOptionData// : Component
    {
        private string ConStr = @"Data Source=XIAOKE;Initial Catalog=db_LANGobang;User ID=sa";

        public ClassOptionData()
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
        }

        #region  ִ���κ�SQL��䣬������Ӱ�������
        /// <summary>
        /// ִ���κ�SQL��䣬������Ӱ�������
        /// </summary>
        public int ExSQL(string SQLStr)                         //ִ���κ�SQL��䣬������Ӱ�������
        {
            try
            {
                SqlConnection cnn = new SqlConnection(ConStr);  //��SqlConnection������ָ�������ݿ�����
                SqlCommand cmd = new SqlCommand(SQLStr, cnn);   //����һ��SqlCommand����ִ��SQL���
                cnn.Open();                                 //�����ݿ������
                int i = 0;                                  //��ȡ��ǰ��Ӱ�������
                i = cmd.ExecuteNonQuery();
                cmd.Dispose();                              //�ͷ�cmd��ʹ�õ���Դ
                cnn.Close();                                    //�ر������ݿ������
                cnn.Dispose();                              //�ͷ�cnn��ʹ�õ���Դ
                return i;                                   //��������
            }
            catch { return 0; }
        }
        #endregion

        #region  ִ���κ�SQL��䣬������Ӱ�������
        /// <summary>
        /// ִ���κ�SQL��䣬������Ӱ�������
        /// </summary>
        public int ExSQLLengData(object Data, string par, string SQLStr)//ִ���κ�SQL��䣬������Ӱ�������
        {
            try
            {
                SqlConnection cnn = new SqlConnection(ConStr);
                SqlCommand cmd = new SqlCommand(SQLStr, cnn);
                cnn.Open();
                int i = 0;
                cmd.Parameters.Add(par, System.Data.SqlDbType.Binary);
                i = cmd.ExecuteNonQuery();
                cmd.Dispose();
                cnn.Close();
                cnn.Dispose();
                return i;
            }
            catch { return 0; }
        }
        #endregion

        #region  ִ���κ�SQL��ѯ��䣬������Ӱ�������
        /// <summary>
        /// ִ���κ�SQL��ѯ��䣬������Ӱ�������
        /// </summary>
        public int ExSQLR(string SQLStr)//ִ���κ�SQL��ѯ��䣬������Ӱ�������
        {
            try
            {
                SqlConnection cnn = new SqlConnection(ConStr);
                SqlCommand cmd = new SqlCommand(SQLStr, cnn);
                cnn.Open();
                SqlDataReader dr;
                int i = 0;
                dr = cmd.ExecuteReader();
                while (dr.Read())
                { i++; }
                cmd.Dispose();
                cnn.Close();
                cnn.Dispose();
                return i;
            }
            catch { return 0; }
        }
        #endregion

        #region  ִ���κ�SQL��ѯ��䣬����һ���ֶ�ֵ
        /// <summary>
        /// ִ���κ�SQL��ѯ��䣬����һ���ֶ�ֵ
        /// </summary>
        public object ExSQLReField(string field, string SQLStr)//ִ���κ�SQL��ѯ��䣬����һ���ֶ�ֵ
        {
            try
            {
                SqlConnection cnn = new SqlConnection(ConStr);
                SqlCommand cmd = new SqlCommand(SQLStr, cnn);
                cnn.Open();
                SqlDataReader dr;
                object fieldValue = null;
                dr = cmd.ExecuteReader();
                if (dr.Read())
                { fieldValue = dr[field]; }
                cmd.Dispose();
                cnn.Close();
                cnn.Dispose();
                return fieldValue;
            }
            catch { return null; }
        }
        #endregion

        #region  ִ���κ�SQL��ѯ��䣬����һ��SqlDataReader
        /// <summary>
        /// ִ���κ�SQL��ѯ��䣬����һ��SqlDataReader
        /// </summary>
        public SqlDataReader ExSQLReDr(string SQLStr)           //ִ���κ�SQL��ѯ��䣬����һ��SqlDataReader����
        {
            try
            {
                SqlConnection cnn = new SqlConnection(ConStr);  //��SqlConnection������ָ�������ݿ�������
                SqlCommand cmd = new SqlCommand(SQLStr, cnn);   //����һ��SqlCommand����ִ��SQL���
                cnn.Open();                                 //�ر����ݿ������
                SqlDataReader dr;                           //����һ��SqlDataReader����
                dr = cmd.ExecuteReader();                       //�����ݱ��е���Ϣ���뵽SqlDataReader������
                return dr;
            }
            catch { return null; }
        }
        #endregion
    }
}
