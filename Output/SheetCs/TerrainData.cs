// generation by sheetBuilder ^o<

using System;
using System.Collections.Generic;

namespace SheetDefine {
    public class TerrainData {
        public const string filename = "TerrainData.json";
        public int terrainId; // 地形編號
        public bool isLand; // 是否是陸地
        public int terrainType; // 地形型態
        public string icon; // 圖示名稱
        public string sprite; // 圖形名稱
        public List<double> testDouble; // 測試字串(real)
        public List<string> testText; // 測試字串(text)
    }
}