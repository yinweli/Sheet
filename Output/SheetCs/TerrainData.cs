// generation time=2020-10-12 22:53:32

using System;
using System.Collections.Generic;

namespace Sheet {
    public class TerrainData {
        public const string filename = "TerrainData.json";
        public long terrainId; // 地形編號
        public bool isLand; // 是否是陸地
        public int terrainType; // 地形型態
        public string icon; // 圖示名稱
        public string sprite; // 圖形名稱
        public List<double> testDouble; // 測試字串(real)
        public List<string> testText; // 測試字串(text)
    }
}