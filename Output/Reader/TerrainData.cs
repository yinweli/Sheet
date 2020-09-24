// generation time=2020-09-24 11:11:12

using System;
using System.Collections.Generic;

namespace Sheet {
    using pkey = Int64;

    public class TerrainData {
        public const string filename = "TerrainData.json";
        public pkey terrainId; // 地形編號
        public int terrainType; // 地形型態
        public string icon; // 圖示名稱
        public string sprite; // 圖形名稱
        public List<double> testReal; // 測試字串(real)
        public List<string> testText; // 測試字串(text)
    }
}