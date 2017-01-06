// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 01/03/2016@1:50 AM

using Assets.Source.OregonTrail.Module.Time;

namespace Assets.Source.OregonTrail.Entity.Location.Weather
{
    /// <summary>
    ///     Defines all the data for a given climate simulation for a location.
    /// </summary>
    public class ClimateData
    {
        private readonly int _humidity;
        private readonly Month _month;
        private readonly float _rainfall;
        private readonly float _temperature;
        private readonly float _temperatureMax;
        private readonly float _temperatureMin;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ClimateData" /> class.
        ///     Creates a new bit of climate data.
        /// </summary>
        /// <param name="month">Month this data is representative of.</param>
        /// <param name="averageTemp">Average set of temperatures for this month.</param>
        /// <param name="tempMax">Maximum temperature this month can have.</param>
        /// <param name="tempMin">Minimum temperature this month can have.</param>
        /// <param name="rainfall">Average rainfall for this month.</param>
        /// <param name="avgHumidity">Daily humidity for this month.</param>
        public ClimateData(
            Month month,
            float averageTemp,
            float tempMax,
            float tempMin,
            float rainfall,
            int avgHumidity)
        {
            _month = month;
            _temperature = averageTemp;
            _temperatureMax = tempMax;
            _temperatureMin = tempMin;
            _rainfall = rainfall;
            _humidity = avgHumidity;
        }

        /// <summary>
        ///     Month this data is representative of.
        /// </summary>
        public Month Month
        {
            get { return _month; }
        }

        /// <summary>
        ///     Average set of temperatures for this month.
        /// </summary>
        public float Temperature
        {
            get { return _temperature; }
        }

        /// <summary>
        ///     Maximum temperature this month can have.
        /// </summary>
        public float TemperatureMax
        {
            get { return _temperatureMax; }
        }

        /// <summary>
        ///     Minimum temperature this month can have.
        /// </summary>
        public float TemperatureMin
        {
            get { return _temperatureMin; }
        }

        /// <summary>
        ///     Average rainfall for this month.
        /// </summary>
        public float Rainfall
        {
            get { return _rainfall; }
        }

        /// <summary>
        ///     Daily humidity for this month.
        /// </summary>
        public int Humidity
        {
            get { return _humidity; }
        }
    }
}