using System;
using System.Collections.Generic;
using System.IO;

namespace Sheet {

    /// <summary>
    /// 匯出cpp版本的資料結構
    /// </summary>
    public class ExportCppStruct {

        /// <summary>
        /// 程式檔案的副檔名
        /// </summary>
        public const string codeExtension = ".hpp";

        /// <summary>
        /// Json檔案的副檔名
        /// </summary>
        public const string jsonExtension = ".json";

        /// <summary>
        /// json引入檔名稱
        /// </summary>
        public const string jsonLibraryName = "json.hpp";

        /// <summary>
        /// 程式碼樣板
        /// </summary>
        public const string codeTemplate =
    @"// generation time={0:yyyy-MM-dd HH:mm:ss}
// use nlohmann's json library
// github: https://github.com/nlohmann/json

#pragma once

#include <stdint.h>
#include <string>
#include <vector>

#include ""{1}""

namespace Sheet {{
using nlohmann::json;

#ifndef PKEY
#define PKEY
using pkey = uint64_t;
#endif // !PKEY

struct {2} {{
{3}

    static std::string get_filename()
    {{
        return ""{4}"";
    }}
}};

inline json get_untyped(const json& j, const char* property)
{{
    return j.find(property) != j.end() ? j.at(property).get<json>() : json();
}}
}} // namespace Sheet

namespace nlohmann {{
inline void from_json(const json& _j, struct Sheet::{2}& _x)
{{
{5}
}}

inline void to_json(json& _j, const struct Sheet::{2}& _x)
{{
    _j = json::object();
{6}
}}
}} // namespace nlohmann";

        /// <summary>
        /// 程式碼樣板: 欄位
        /// </summary>
        public const string fieldTemplate = @"    {0} {1}; // {2}";

        /// <summary>
        /// 程式碼樣板: 從json字串解析
        /// </summary>
        public const string fromJsonTemplate = @"    _x.{1} = _j.at(""{1}"").get<{0}>();";

        /// <summary>
        /// 程式碼樣板: 轉換到json字串
        /// </summary>
        public const string toJsonTemplate = @"    _j[""{0}""] = _x.{0};";

        /// <summary>
        /// 匯出到cpp檔案
        /// </summary>
        /// <param name="settingGlobal_">全域設定資料</param>
        /// <param name="settingReader">讀取器設定資料</param>
        /// <param name="settingElement_">項目設定資料</param>
        /// <param name="import_">匯入器</param>
        /// <returns>true表示成功, false則否</returns>
        public static bool Export(SettingGlobal settingGlobal_, SettingElement settingElement_, Import import_) {
            var elementName = settingElement_.GetElementName();
            var filepath = Path.Combine(settingGlobal_.outputPathCpp, elementName + codeExtension);
            var libraryPath = Path.Combine(settingGlobal_.cppLibraryPath, jsonLibraryName).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            var templateField = new List<string>();
            var fields = import_.GetFields();

            foreach (var itor in fields) {
                if (itor.fieldType.IsExport())
                    templateField.Add(string.Format(fieldTemplate, itor.fieldType.TypeCpp(), itor.name, itor.note));
            } //for

            var fromJsons = new List<string>();

            foreach (var itor in fields) {
                if (itor.fieldType.IsExport())
                    fromJsons.Add(string.Format(fromJsonTemplate, itor.fieldType.TypeCpp(), itor.name));
            } //for

            var toJsons = new List<string>();

            foreach (var itor in fields) {
                if (itor.fieldType.IsExport())
                    toJsons.Add(string.Format(toJsonTemplate, itor.name));
            } //for

            var context = string.Format(
                codeTemplate,
                DateTime.Now,
                libraryPath,
                elementName,
                string.Join(Environment.NewLine, templateField),
                elementName + jsonExtension,
                string.Join(Environment.NewLine, fromJsons),
                string.Join(Environment.NewLine, toJsons));

            Directory.CreateDirectory(settingGlobal_.outputPathCpp);
            UtilityFile.WriteAllText(filepath, context);

            return true;
        }
    }
}