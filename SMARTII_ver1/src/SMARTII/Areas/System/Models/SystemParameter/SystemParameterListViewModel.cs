namespace SMARTII.Areas.System.Models.SystemParameter
{
    public class SystemparameterListViewModel
    {
        public SystemparameterListViewModel(Domain.System.SystemParameter systemParameter)
        {
            this.ID = systemParameter.ID;
            this.Key = systemParameter.Key;
            this.Text = systemParameter.Text;
            this.Value = systemParameter.Value;
        }

        /// <summary>
        /// 代號
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 關鍵值
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 實際資料
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 代表文字
        /// </summary>
        public string Text { get; set; }
    }
}