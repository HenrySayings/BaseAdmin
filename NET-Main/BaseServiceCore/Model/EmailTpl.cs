﻿namespace BaseServiceCore.Model
{
    /// <summary>
    /// 邮件发送模板
    /// </summary>
    [SugarTable("emailTpl")]
    public class EmailTpl : SysBase
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public string Name { get; set; }
        [SugarColumn(ColumnDescription = "模板内容", ColumnDataType = StaticConfig.CodeFirst_BigString)]
        public string Content { get; set; }
    }
}
