// generation by sheetBuilder ^o<
// use nlohmann's json library
// github: https://github.com/nlohmann/json

#pragma once

#include <stdint.h>
#include <string>
#include <vector>

#include "nlohmann/json.hpp"

namespace SheetDefine {
using nlohmann::json;

#ifndef PKEY
#define PKEY
using pkey = int32_t;
#endif // !PKEY

struct TerrainData {
    Sheet::pkey terrainId; // 地形編號
    bool isLand; // 是否是陸地
    int32_t terrainType; // 地形型態
    std::string icon; // 圖示名稱
    std::string sprite; // 圖形名稱
    std::vector<int32_t> testInt; // 測試字串(int)
    std::vector<double> testDouble; // 測試字串(real)
    std::vector<std::string> testText; // 測試字串(text)

    static std::string get_filename()
    {
        return "TerrainData.json";
    }
};

inline json get_untyped(const json& j, const char* property)
{
    return j.find(property) != j.end() ? j.at(property).get<json>() : json();
}
} // namespace Sheet

namespace nlohmann {
inline void from_json(const json& _j, struct Sheet::TerrainData& _x)
{
    _x.terrainId = _j.at("terrainId").get<Sheet::pkey>();
    _x.isLand = _j.at("isLand").get<bool>();
    _x.terrainType = _j.at("terrainType").get<int32_t>();
    _x.icon = _j.at("icon").get<std::string>();
    _x.sprite = _j.at("sprite").get<std::string>();
    _x.testInt = _j.at("testInt").get<std::vector<int32_t>>();
    _x.testDouble = _j.at("testDouble").get<std::vector<double>>();
    _x.testText = _j.at("testText").get<std::vector<std::string>>();
}

inline void to_json(json& _j, const struct Sheet::TerrainData& _x)
{
    _j = json::object();
    _j["terrainId"] = _x.terrainId;
    _j["isLand"] = _x.isLand;
    _j["terrainType"] = _x.terrainType;
    _j["icon"] = _x.icon;
    _j["sprite"] = _x.sprite;
    _j["testInt"] = _x.testInt;
    _j["testDouble"] = _x.testDouble;
    _j["testText"] = _x.testText;
}
} // namespace nlohmann