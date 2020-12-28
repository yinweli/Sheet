using System.Collections.Generic;
using System.Linq;

namespace Sheet {

    /// <summary>
    /// 設定資料
    /// </summary>
    public class Setting {

        /// <summary>
        /// 檢查是否正確
        /// </summary>
        /// <returns>true表示正確, false則否</returns>
        public bool Check() {
            if (global == null)
                return Output.Error("setting", "global not found");

            if (global.Check() == false)
                return Output.Error("setting", "global check failed");

            if (elements == null)
                return Output.Error("setting", "element not found");

            if (elements.Count <= 0)
                return Output.Error("setting", "element empty");

            if (elements.Any(itor => itor.Check() == false))
                return Output.Error("setting", "element check failed");

            return true;
        }

        /// <summary>
        /// 全域設定資料
        /// </summary>
        public SettingGlobal global = null;

        /// <summary>
        /// 項目設定資料列表
        /// </summary>
        public List<SettingElement> elements = null;
    }
}