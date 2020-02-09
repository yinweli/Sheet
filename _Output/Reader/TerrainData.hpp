// generation time=2020-02-09 22:55:29
// use nlohmann's json library
// github: https://github.com/nlohmann/json

#pragma once

#include <stdint.h>
#include <string>
#include <vector>

#include "nlohmann/json.hpp"

namespace StaticData {
using nlohmann::json;

#ifndef PKEY
#define PKEY
using pkey = uint64_t;
#endif // !PKEY

struct TerrainData {
    StaticData::pkey terrainId; // 地形編號
    int32_t terrainType; // 地形型態
    std::string icon; // 圖示名稱
    std::string sprite; // 圖形名稱
    std::vector<double> testReal; // 測試字串(real)
    std::vector<std::string> testText; // 測試字串(text)
};

inline json get_untyped(const json& j, const char* property)
{
    return j.find(property) != j.end() ? j.at(property).get<json>() : json();
}

inline std::string get_filename()
{
    return "TerrainData.json";
}
} // namespace StaticData

namespace nlohmann {
inline void from_json(const json& _j, struct StaticData::TerrainData& _x)
{
    _x.terrainId = _j.at("terrainId").get<StaticData::pkey>();
    _x.terrainType = _j.at("terrainType").get<int32_t>();
    _x.icon = _j.at("icon").get<std::string>();
    _x.sprite = _j.at("sprite").get<std::string>();
    _x.testReal = _j.at("testReal").get<std::vector<double>>();
    _x.testText = _j.at("testText").get<std::vector<std::string>>();
}

inline void to_json(json& _j, const struct StaticData::TerrainData& _x)
{
    _j = json::object();
    _j["terrainId"] = _x.terrainId;
    _j["terrainType"] = _x.terrainType;
    _j["icon"] = _x.icon;
    _j["sprite"] = _x.sprite;
    _j["testReal"] = _x.testReal;
    _j["testText"] = _x.testText;
}
} // namespace nlohmann