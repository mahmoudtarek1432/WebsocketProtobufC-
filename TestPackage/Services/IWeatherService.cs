﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestPackage.DTO;

namespace TestPackage.Services
{
    internal interface IWeatherService
    {
        public WeatherResponse GetService(WeatherRequest weather);

        public void NotifyWeatherChange();
    }
}
