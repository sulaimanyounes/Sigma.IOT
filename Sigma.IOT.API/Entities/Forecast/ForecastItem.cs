﻿namespace Sigma.IOT.API.Entities.Forecast
{
    public class ForecastItem
    {
        public DateTime Date { get; set; }
        public Measurement? Measurement { get; set; }
    }
}