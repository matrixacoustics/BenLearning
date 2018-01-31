using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BenLearning.Models
{
    public class NoiseMeasurement
    {
        public int Id { get; set; }
        public DateTime StartTimeOfMeasurement { get; set; }
        public DateTime? SensorStartTimeOfMeasurement { get; set; }
        public DateTime? GPSStartTimeOfMeasurement { get; set; }
        public string MicSensitivity { get; set; }
        [StringLength(45)]
        public string MicType { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        [StringLength(50)]
        public string Firmware { get; set; }
        public int MonitorFrequencyInMinutes { get; set; }
        public string SLMSerialNumber { get; set; }
        public virtual ICollection<NoiseMeasurementDetail> NoiseMeasurementDetail { get; set; }

    }

    public class NoiseMeasurementDetail
    {
        public int Id { get; set; }
        public int NoiseMeasurementId { get; set; }
        public virtual NoiseMeasurement NoiseMeasurement { get; set; }
        public string FreqWeight { get; set; }
        public string TimeWeight { get; set; }
        public string MeasurementQuality { get; set; }
        public string Overall { get; set; }
        public string Hz6 { get; set; }
        public string Hz8 { get; set; }
        public string Hz10 { get; set; }
        public string Hz12 { get; set; }
        public string Hz16 { get; set; }
        public string Hz20 { get; set; }
        public string Hz25 { get; set; }
        public string Hz31 { get; set; }
        public string Hz40 { get; set; }
        public string Hz50 { get; set; }
        public string Hz63 { get; set; }
        public string Hz80 { get; set; }
        public string Hz100 { get; set; }
        public string Hz125 { get; set; }
        public string Hz160 { get; set; }
        public string Hz200 { get; set; }
        public string Hz250 { get; set; }
        public string Hz315 { get; set; }
        public string Hz400 { get; set; }
        public string Hz500 { get; set; }
        public string Hz630 { get; set; }
        public string Hz800 { get; set; }
        public string Hz1000 { get; set; }
        public string Hz1250 { get; set; }
        public string Hz1600 { get; set; }
        public string Hz2000 { get; set; }
        public string Hz2500 { get; set; }
        public string Hz3150 { get; set; }
        public string Hz4000 { get; set; }
        public string Hz5000 { get; set; }
        public string Hz6300 { get; set; }
        public string Hz8000 { get; set; }
        public string Hz10000 { get; set; }
        public string Hz12500 { get; set; }
        public string Hz16000 { get; set; }
        public string Hz20000 { get; set; }
    }

    public class Vibration
    {

    }

    public class AirQuality
    {

    }

    public class GPS
    {

    }
}

