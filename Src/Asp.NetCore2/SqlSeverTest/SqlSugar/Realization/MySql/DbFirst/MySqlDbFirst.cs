using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlSugar
{
    public class MySqlDbFirst : DbFirstProvider
    {
            // modify by kevin 添加json字段标记
        override protected string GetPropertyText(DbColumnInfo item, string PropertyText)
        {                
            string SugarColumnText = DbFirstTemplate.ValueSugarCoulmn;
            var propertyName = GetPropertyName(item);
            var isMappingColumn = propertyName != item.DbColumnName;
            var hasSugarColumn = item.IsPrimarykey == true || item.IsIdentity == true || isMappingColumn;


            var isJsonColumn = string.Equals(item.DataType,"json",StringComparison.CurrentCultureIgnoreCase);
            if(isJsonColumn){
                item.IsJson=true;
            }

            if (hasSugarColumn && this.IsAttribute)
            {
                List<string> joinList = new List<string>();
                if (item.IsPrimarykey)
                {
                    joinList.Add("IsPrimaryKey=true");
                }
                if (item.IsIdentity)
                {
                    joinList.Add("IsIdentity=true");
                }
                if (isMappingColumn)
                {
                    joinList.Add("ColumnName=\"" + item.DbColumnName + "\"");
                }
                if(isJsonColumn){
                    joinList.Add("IsJson=true");
                }
                SugarColumnText = string.Format(SugarColumnText, string.Join(",", joinList));
            }
            else
            {
                SugarColumnText = null;
            }
            string typeString = GetPropertyTypeName(item);
            PropertyText = PropertyText.Replace(DbFirstTemplate.KeySugarColumn, SugarColumnText);
            PropertyText = PropertyText.Replace(DbFirstTemplate.KeyPropertyType, typeString);
            PropertyText = PropertyText.Replace(DbFirstTemplate.KeyPropertyName, propertyName);
            return PropertyText;
        }
    }
}
