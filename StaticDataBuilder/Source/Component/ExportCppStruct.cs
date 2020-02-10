using System;
using System.Collections.Generic;
using System.IO;

namespace StaticData
{
    /// <summary>
    /// 匯出cpp版本的資料結構
    /// </summary>
    public class ExportCppStruct
    {
        /// <summary>
        /// 匯出到cpp檔案
        /// </summary>
        /// <param name="settingGlobal">全域設定資料</param>
        /// <param name="settingReader">讀取器設定資料</param>
        /// <param name="settingElement">項目設定資料</param>
        /// <param name="import">匯入器</param>
        /// <returns>true表示成功, false則否</returns>
        public static bool Export(SettingGlobal settingGlobal, SettingElement settingElement, Import import) {
            var filepath = Path.Combine(settingGlobal.outputPathCpp, settingElement.elementName + EXPORT_EXT);
            var libraryPath = Path.Combine(settingGlobal.cppLibraryPath, JSON_INCLUDE).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            var templateField = new List<string>();
            var fields = import.GetFields();

            foreach (var itor in fields) {
                if (itor.fieldType.IsExport())
                    templateField.Add(string.Format(TEMPLATE_FIELD, itor.fieldType.TypeCpp(), itor.name, itor.note));
            } //for

            var templateFromJson = new List<string>();

            foreach (var itor in fields) {
                if (itor.fieldType.IsExport())
                    templateFromJson.Add(string.Format(TEMPLATE_FROM_JSON, itor.fieldType.TypeCpp(), itor.name));
            } //for

            var templateToJson = new List<string>();

            foreach (var itor in fields) {
                if (itor.fieldType.IsExport())
                    templateToJson.Add(string.Format(TEMPLATE_TO_JSON, itor.name));
            } //for

            var context = string.Format(TEMPLATE_CODE, DateTime.Now, libraryPath, settingElement.elementName,
                string.Join("\n", templateField),
                settingElement.elementName + EXPORT_JSON,
                string.Join("\n", templateFromJson),
                string.Join("\n", templateToJson));

            Directory.CreateDirectory(settingGlobal.outputPathCpp);
            UtilityFile.WriteAllText(filepath, context);

            return true;
        }

        /// <summary>
        /// 匯出副檔名
        /// </summary>
        private const string EXPORT_EXT = ".hpp";

        /// <summary>
        /// 匯出Json檔名
        /// </summary>
        private const string EXPORT_JSON = ".json";

        /// <summary>
        /// 程式碼樣板
        /// </summary>
        private const string TEMPLATE_CODE =
    @"// generation time={0:yyyy-MM-dd HH:mm:ss}
// use nlohmann's json library
// github: https://github.com/nlohmann/json

#pragma once

#include <stdint.h>
#include <string>
#include <vector>

#include ""{1}""

namespace StaticData {{
using nlohmann::json;

#ifndef PKEY
#define PKEY
using pkey = uint64_t;
#endif // !PKEY

struct {2} {{
{3}
}};

inline std::string get_filename()
{{
    return ""{4}"";
}}

inline json get_untyped(const json& j, const char* property)
{{
    return j.find(property) != j.end() ? j.at(property).get<json>() : json();
}}
}} // namespace StaticData

namespace nlohmann {{
inline void from_json(const json& _j, struct StaticData::{2}& _x)
{{
{5}
}}

inline void to_json(json& _j, const struct StaticData::{2}& _x)
{{
    _j = json::object();
{6}
}}
}} // namespace nlohmann";

        /// <summary>
        /// json引入檔名稱
        /// </summary>
        private const string JSON_INCLUDE = "json.hpp";

        /// <summary>
        /// 程式碼樣板: 欄位
        /// </summary>
        private const string TEMPLATE_FIELD = @"    {0} {1}; // {2}";

        /// <summary>
        /// 程式碼樣板: 從json字串解析
        /// </summary>
        private const string TEMPLATE_FROM_JSON = @"    _x.{1} = _j.at(""{1}"").get<{0}>();";

        /// <summary>
        /// 程式碼樣板: 轉換到json字串
        /// </summary>
        private const string TEMPLATE_TO_JSON = @"    _j[""{0}""] = _x.{0};";
    }
}