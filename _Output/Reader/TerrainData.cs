// generation time=2020-02-10 21:36:35

using System;
using System.Collections.Generic;

namespace StaticData
{
    using pkey = Int64;

    public class TerrainData
    {
        public pkey terrainId; // 地形編號
        public int terrainType; // 地形型態
        public string icon; // 圖示名稱
        public string sprite; // 圖形名稱
        public List<double> testReal; // 測試字串(real)
        public List<string> testText; // 測試字串(text)
        public const string filename = "TerrainData.json";
    }
}