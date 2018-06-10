using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;

namespace GobangClass
{
    /// <summary>
    /// 用SQL语句对数据库中的信息进行添加、修改及查询的操作
    /// </summary>
    public class ClassOptionData// : Component
    {
        private string ConStr = @"Data Source=XIAOKE;Initial Catalog=db_LANGobang;User ID=sa";

        public ClassOptionData()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        #region  执行任何SQL语句，返回所影响的行数
        /// <summary>
        /// 执行任何SQL语句，返回所影响的行数
        /// </summary>
        public int ExSQL(string SQLStr)                         //执行任何SQL语句，返回所影响的行数
        {
            try
            {
                SqlConnection cnn = new SqlConnection(ConStr);  //用SqlConnection对象与指定的数据库连接
                SqlCommand cmd = new SqlCommand(SQLStr, cnn);   //创建一个SqlCommand对象，执行SQL语句
                cnn.Open();                                 //打开数据库的连接
                int i = 0;                                  //获取当前所影响的行数
                i = cmd.ExecuteNonQuery();
                cmd.Dispose();                              //释放cmd所使用的资源
                cnn.Close();                                    //关闭与数据库的连接
                cnn.Dispose();                              //释放cnn所使用的资源
                return i;                                   //返回行数
            }
            catch { return 0; }
        }
        #endregion

        #region  执行任何SQL语句，返回所影响的行数
        /// <summary>
        /// 执行任何SQL语句，返回所影响的行数
        /// </summary>
        public int ExSQLLengData(object Data, string par, string SQLStr)//执行任何SQL语句，返回所影响的行数
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

        #region  执行任何SQL查询语句，返回所影响的行数
        /// <summary>
        /// 执行任何SQL查询语句，返回所影响的行数
        /// </summary>
        public int ExSQLR(string SQLStr)//执行任何SQL查询语句，返回所影响的行数
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

        #region  执行任何SQL查询语句，返回一个字段值
        /// <summary>
        /// 执行任何SQL查询语句，返回一个字段值
        /// </summary>
        public object ExSQLReField(string field, string SQLStr)//执行任何SQL查询语句，返回一个字段值
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

        #region  执行任何SQL查询语句，返回一个SqlDataReader
        /// <summary>
        /// 执行任何SQL查询语句，返回一个SqlDataReader
        /// </summary>
        public SqlDataReader ExSQLReDr(string SQLStr)           //执行任何SQL查询语句，返回一个SqlDataReader对象
        {
            try
            {
                SqlConnection cnn = new SqlConnection(ConStr);  //用SqlConnection对象与指定的数据库相连接
                SqlCommand cmd = new SqlCommand(SQLStr, cnn);   //创建一个SqlCommand对象，执行SQL语句
                cnn.Open();                                 //关闭数据库的连接
                SqlDataReader dr;                           //定义一个SqlDataReader对象
                dr = cmd.ExecuteReader();                       //将数据表中的信息存入到SqlDataReader对象中
                return dr;
            }
            catch { return null; }
        }
        #endregion
    }
}
