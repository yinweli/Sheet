using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Sheet {

    internal class Program {

        private static int Main(string[] args_) {
            return Start(args_) ? 0 : 1;
        }

        /// <summary>
        /// 開始建立表格資料
        /// </summary>
        /// <param name="args_">參數列表</param>
        /// <returns>true表示成功, false則否</returns>
        private static bool Start(string[] args_) {
            using (AutoStopwatch autoStopWatchGlobal = new AutoStopwatch("Sheet builder")) {
                File.Delete(Output.errorLog);

                if (args_.Length <= 0)
                    return Output.Error("must specify the setting file path");

                Setting setting = ReadSetting(args_[0]);

                if (setting == null)
                    return Output.Error("setting read failed");

                if (setting.Check() == false)
                    return Output.Error("setting check failed");

                var result = true;
                List<Collection> collections = new List<Collection>();

                foreach (var itor in setting.elements) {
                    using (AutoStopwatch autoStopWatchLocal = new AutoStopwatch(itor.ToString())) {
                        Import import = new Import();

                        result &= import.Read(setting.global, itor) &&
                            ExportJson.Export(setting.global, itor, import) &&
                            ExportCsStruct.Export(setting.global, itor, import) &&
                            ExportCppStruct.Export(setting.global, itor, import);

                        collections.Add(new Collection() { settingElement = itor, import = import });
                    }//using
                }//for

                return result;
            }//using
        }

        /// <summary>
        /// 讀取設定檔
        /// </summary>
        /// <param name="path_">設定檔路徑</param>
        /// <returns>true表示成功, false則否</returns>
        private static Setting ReadSetting(string path_) {
            try {
                if (File.Exists(path_) == false) {
                    Output.Error("setting file not exist");
                    return null;
                }//if

                var deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
                var document = File.ReadAllText(path_);
                var input = new StringReader(document);

                return deserializer.Deserialize<Setting>(input);
            } catch (Exception e) {
                Output.Error(e.InnerException.Message);
                return null;
            }
        }
    }
}