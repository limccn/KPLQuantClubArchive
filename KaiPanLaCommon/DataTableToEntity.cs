using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;

namespace KaiPanLaCommon
{
    /// <summary>
    /// DataTable转实体类集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataTableToEntity<T> where T : new()
    {
        /// <summary>
        /// table转实体集合
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<T> FillModel(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
                return null;
            List<T> result = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    T res = new T();
                    for (int i = 0; i < dr.Table.Columns.Count; i++)
                    {
                        PropertyInfo propertyInfo = res.GetType().GetProperty(dr.Table.Columns[i].ColumnName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        if (propertyInfo != null && dr[i] != DBNull.Value)
                        {
                            var value = dr[i];
                            switch (propertyInfo.PropertyType.FullName)
                            {
                                case "System.Decimal":
                                    propertyInfo.SetValue(res, Convert.ToDecimal(value), null); break;
                                case "System.Double":
                                    propertyInfo.SetValue(res, Convert.ToDouble(value), null); break;
                                case "System.DateTime":
                                    propertyInfo.SetValue(res, Convert.ToDateTime(value), null); break;
                                case "System.String":
                                    propertyInfo.SetValue(res, value, null); break;
                                case "System.Int32":
                                    propertyInfo.SetValue(res, Convert.ToInt32(value), null); break;
                                case "System.Single":
                                    propertyInfo.SetValue(res, Convert.ToInt32(value), null); break;
                                case "System.Int64":
                                    propertyInfo.SetValue(res, Convert.ToInt64(value), null); break;
                                default:
                                    propertyInfo.SetValue(res, value, null); break;
                            }
                        }
                    }
                    result.Add(res);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }

            }
            return result;
        }

        /// <summary>
        /// 读取json内容转成实体类集合
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public List<T> ReadDataToModel(Stream stream)
        {
            StreamReader sr = new StreamReader(stream);
            try
            {
                string temp = sr.ReadToEnd();
                return JsonConvert.DeserializeObject<List<T>>(temp);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                sr.Dispose();
                sr.Close();
            }
        }

        /// <summary>
        /// 实体类集合转table
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataTable FillDataTable(List<T> modelList)
        {
            if (modelList == null || modelList.Count == 0)
                return null;
            DataTable dt = CreatTable(modelList[0]);
            foreach (T model in modelList)
            {
                DataRow dr = dt.NewRow();
                foreach (PropertyInfo p in typeof(T).GetProperties())
                {
                    dr[p.Name] = p.GetValue(model, null);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// 根据实体创建table
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private DataTable CreatTable(T model)
        {
            DataTable dt = new DataTable(typeof(T).Name);
            foreach (PropertyInfo p in typeof(T).GetProperties())
            {
                dt.Columns.Add(new DataColumn(p.Name, p.PropertyType));
            }
            return dt;
        }
    }

}
