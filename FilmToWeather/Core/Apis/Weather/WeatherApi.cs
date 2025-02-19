﻿using Core.Api.ConectionService;
using Core.Api.Weather.Entities.Responce;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Configuration;

namespace Core.Api.Weather
{
    public class WeatherApi : IWeatherApi, IWeatherApiPreLoad
    {
        private readonly IConectionHandler _conectionHandler;
        private readonly string _weatherApiKey;
        private readonly string _weatherBaseUrl;
        private readonly string _weatherDocsForAutoLoad;
        private readonly string _currentWeather = "current.json";
        private readonly string _moonPhase = "astronomy.json";
        public WeatherApi(IConfiguration configuration, IConectionHandler conectionHandler)
        {
            _weatherApiKey = configuration["WeatherApiKey"];
            _weatherBaseUrl = configuration["WeatherBaseUrl"];
            _weatherDocsForAutoLoad = configuration["WeatherDocsBaseUrl"];
            _conectionHandler = conectionHandler;
        }

        public async Task<WeatherApiBodyResponce> GetCurrentWeather(string city)
        {
            var currentWeather = _weatherBaseUrl.AppendPathSegment(_currentWeather).SetQueryParams(new
            {
                key = _weatherApiKey,
                q = city
            });

            return await _conectionHandler.CallApi(() => currentWeather.GetJsonAsync<WeatherApiBodyResponce>());
        }

        public async Task<MoonPhaseResponce> GetMoonPhase(string city)
        {
            var moonPhase = _weatherBaseUrl.AppendPathSegment(_moonPhase).SetQueryParams(new
            {
                key = _weatherApiKey,
                q = city
            });

            return await _conectionHandler.CallApi(() => moonPhase.GetJsonAsync<MoonPhaseResponce>());
        }
        
        public async Task<ConditionForPreloadResponce[]> GetWeatherConditionsDoc()
        {
            var x = await _conectionHandler.CallApi(() => _weatherDocsForAutoLoad.GetJsonAsync<ConditionForPreloadResponce[]>());
            return x; 
        }
    }
}
