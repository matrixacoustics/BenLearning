using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Management;
using BenLearning.Models;

namespace BenLearning
{
    class Program
    {
        static void Main(string[] args)
        {
            

            runXL2.XL2GetResults();

            

        }


    }

    class runXL2
    {
        public static void XL2GetResults()
        {
            XL2 newXL2 = new XL2();
            ApplicationDbContext db = new ApplicationDbContext();
            SerialPort xl2port = XL2.getSerialPort();

            var out1 = XL2.NoiseMeasurementResult(xl2port);
            //Console.WriteLine(out1.make);
            //Console.WriteLine(out1.model);
            Console.WriteLine(out1.Firmware);
            Console.WriteLine(out1.SLMSerialNumber);
            Console.WriteLine(out1.MicSensitivity);
            Console.WriteLine(out1.MicType);
            Console.WriteLine(out1.StartTimeOfMeasurement);

            foreach (var x in XL2.singleValueCommands)
            {
                var out2 = XL2.NoiseMeasurementDetailResultSingle(xl2port, x.command, x.fweight, x.tweight, x.metric);
                Console.WriteLine(out2.Metric);
                Console.WriteLine(out2.Overall);
                Console.WriteLine(out2.MeasurementQuality);
            }

            foreach (var x in XL2.thirdValueCommands)
            {
                var out2 = XL2.NoiseMeasurementDetailResultThird(xl2port, x.command, x.fweight, x.tweight, x.metric);
                Console.WriteLine(out2.Metric);
                Console.WriteLine(out2.Hz6);
                Console.WriteLine(out2.MeasurementQuality);
            }

            db.NoiseMeasurements.Add(new NoiseMeasurement() { Firmware = "bill" });
            db.SaveChanges();

            Console.ReadLine();
        }
    }

    class XL2
    {
        public class NoiseMeasurementCommands
        {
            public string command { get; set; }
            public string fweight { get; set; }
            public string tweight { get; set; }
            public string metric { get; set; }
        }

        public static List<NoiseMeasurementCommands> idCommands = new List<NoiseMeasurementCommands>
        {
            new NoiseMeasurementCommands() {command = "*IDN?\n" },
            new NoiseMeasurementCommands() {command = "CALI:MIC:TYPE?\n" },
            new NoiseMeasurementCommands() {command = "CALIB:MIC:SENS:VALU?\n" },
        };

        public class NoiseMeasurementResults
        {
            //public string make { get; set; }
            //public string model { get; set; }
            public string SLMSerialNumber { get; set; }
            public string Firmware { get; set; }
            public string MicType { get; set; }
            public string MicSensitivity { get; set; }
            public DateTime StartTimeOfMeasurement { get; set; }
            public DateTime SensorStartTimeOfMeasurement { get; set; }
            public DateTime GPSStartTimeOfMeasurement { get; set; }
            public decimal Latitude { get; set; }
            public decimal Longitude { get; set; }
            public int MonitorFrequencyInMinutes { get; set; }

        }
        public static NoiseMeasurementResults NoiseMeasurementResult(SerialPort port)
        {
            XL2.NoiseMeasurementResults R1 = new XL2.NoiseMeasurementResults();
            List<string> result1 = XL2.GetXL2String(port, idCommands[0].command);
            R1.Firmware = result1[3];
            //R1.make = result1[0];
            //R1.model = result1[1];
            R1.SLMSerialNumber = result1[2];
            List<string> result2 = XL2.GetXL2String(port, idCommands[1].command);
            R1.MicType = result2[0];
            List<string> result3 = XL2.GetXL2String(port, idCommands[2].command);
            R1.MicSensitivity = result3[0];
            R1.StartTimeOfMeasurement = DateTime.Now;
            return R1;
        }

        public static List<NoiseMeasurementCommands> singleValueCommands = new List<NoiseMeasurementCommands>
        {
            new NoiseMeasurementCommands() {command = "MEAS:SLM:123? LAEQ\n", fweight = "A", metric = "EQ"},
            new NoiseMeasurementCommands() {command = "MEAS:SLM:123? LAFMAX\n", fweight = "A", tweight = "F", metric = "MAX"},
            new NoiseMeasurementCommands() {command = "MEAS:SLM:123? LAFMIN\n", fweight = "A", tweight = "F", metric = "MIN"},
            new NoiseMeasurementCommands() {command = "MEAS:SLM:123? L1%\n", fweight = "A", tweight = "F", metric = "L1"}, //needs to be collected from the setup of the logger as the logger doesn't report which settings are for the percentil channels.
            new NoiseMeasurementCommands() {command = "MEAS:SLM:123? L5%\n", fweight = "A", tweight = "F", metric = "L5"},
            new NoiseMeasurementCommands() {command = "MEAS:SLM:123? L10%\n", fweight = "A", tweight = "F", metric = "L10"},
            new NoiseMeasurementCommands() {command = "MEAS:SLM:123? L50%\n", fweight = "A", tweight = "F", metric = "L50"},
            new NoiseMeasurementCommands() {command = "MEAS:SLM:123? L90%\n", fweight = "A", tweight = "F", metric = "L90"},
            new NoiseMeasurementCommands() {command = "MEAS:SLM:123? L95%\n", fweight = "A", tweight = "F", metric = "L95"},
            new NoiseMeasurementCommands() {command = "MEAS:SLM:123? L99%\n", fweight = "A", tweight = "F", metric = "L99"},
        };

        public static List<NoiseMeasurementCommands> thirdValueCommands = new List<NoiseMeasurementCommands>
        {
            new NoiseMeasurementCommands() {command = "MEAS:SLM:RTA? E\n", fweight = "A", metric = "E"},
            new NoiseMeasurementCommands() {command = "MEAS:SLM:RTA? EQ\n", fweight = "A", metric = "EQ"},
            new NoiseMeasurementCommands() {command = "MEAS:SLM:RTA? MAX\n", fweight = "A", tweight = "F", metric = "MAX"},
            new NoiseMeasurementCommands() {command = "MEAS:SLM:RTA? MIN\n", fweight = "A", tweight = "F", metric = "MIN"},
            new NoiseMeasurementCommands() {command = "MEAS:SLM:RTA? 1%\n", fweight = "A", tweight = "F", metric = "L1"}, //needs to be collected from the setup of the logger as the logger doesn't report which settings are for the percentil channels.
            new NoiseMeasurementCommands() {command = "MEAS:SLM:RTA? 5%\n", fweight = "A", tweight = "F", metric = "L5"},
            new NoiseMeasurementCommands() {command = "MEAS:SLM:RTA? 10%\n", fweight = "A", tweight = "F", metric = "L10"},
            new NoiseMeasurementCommands() {command = "MEAS:SLM:RTA? 50%\n", fweight = "A", tweight = "F", metric = "L50"},
            new NoiseMeasurementCommands() {command = "MEAS:SLM:RTA? 90%\n", fweight = "A", tweight = "F", metric = "L90"},
            new NoiseMeasurementCommands() {command = "MEAS:SLM:RTA? 95%\n", fweight = "A", tweight = "F", metric = "L95"},
            new NoiseMeasurementCommands() {command = "MEAS:SLM:RTA? 99%\n", fweight = "A", tweight = "F", metric = "L99"},
        };

        public class NoiseMeasurementDetailResults
        {
            public string FreqWeight { get; set; }
            public string TimeWeight { get; set; }
            public string Metric { get; set; }
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


        public static NoiseMeasurementDetailResults NoiseMeasurementDetailResultSingle(SerialPort port, string command, string fweight, string tweight, string metric)
        {
            XL2.NoiseMeasurementDetailResults R1 = new XL2.NoiseMeasurementDetailResults();
            List<string> result1 = XL2.GetXL2String(port, command);
            //foreach (var x in result1)
            //    Console.WriteLine("full results = {0}" ,x);
            R1.FreqWeight = fweight;
            R1.TimeWeight = tweight;
            R1.Metric = metric;
            R1.Overall = result1[0];
            R1.MeasurementQuality = result1[1];
            return R1;
        }

        public static NoiseMeasurementDetailResults NoiseMeasurementDetailResultThird(SerialPort port, string command, string fweight, string tweight, string metric)
        {
            XL2.NoiseMeasurementDetailResults R1 = new XL2.NoiseMeasurementDetailResults();
            List<string> result1 = XL2.GetXL2String(port, command);
            //foreach (var x in result1)
            //    Console.WriteLine("full results = {0}" ,x);
            R1.FreqWeight = fweight;
            R1.TimeWeight = tweight;
            R1.Metric = metric;
            R1.Hz6 = result1[0];
            R1.Hz8 = result1[1];
            R1.Hz10 = result1[2];
            R1.Hz12 = result1[3];
            R1.Hz16 = result1[4];
            R1.Hz20 = result1[5];
            R1.Hz25 = result1[6];
            R1.Hz31 = result1[7];
            R1.Hz40 = result1[8];
            R1.Hz50 = result1[9];
            R1.Hz63 = result1[10];
            R1.Hz80 = result1[11];
            R1.Hz100 = result1[12];
            R1.Hz125 = result1[13];
            R1.Hz160 = result1[14];
            R1.Hz200 = result1[15];
            R1.Hz250 = result1[16];
            R1.Hz315 = result1[17];
            R1.Hz400 = result1[18];
            R1.Hz500 = result1[19];
            R1.Hz630 = result1[20];
            R1.Hz800 = result1[21];
            R1.Hz1000 = result1[22];
            R1.Hz1250 = result1[23];
            R1.Hz1600 = result1[24];
            R1.Hz2000 = result1[25];
            R1.Hz2500 = result1[26];
            R1.Hz3150 = result1[27];
            R1.Hz4000 = result1[28];
            R1.Hz5000 = result1[29];
            R1.Hz6300 = result1[30];
            R1.Hz8000 = result1[31];
            R1.Hz10000 = result1[32];
            R1.Hz12500 = result1[33];
            R1.Hz16000 = result1[34];
            R1.Hz20000 = result1[35];
            R1.MeasurementQuality = result1[36];
            return R1;
        }


        public static SerialPort getSerialPort()
        {
            SerialPort ComPort = new SerialPort();
            foreach (ManagementObject device in new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_SerialPort").Get())
            {
                // Select device based on VendorID.
                if (device["PNPDeviceID"].ToString().Contains("VID_1A2B"))
                //if (device["PNPDeviceID"].ToString().Contains(VID))
                {
                    ComPort = new SerialPort(device["DeviceID"].ToString());
                }
            }
            return ComPort;
        }

        public static List<string> GetXL2String(SerialPort port, string command)
        {
            if (!port.IsOpen)
            {
                port.Open();
            }
            port.Write("MEAS:INIT\n");
            port.Write(command);
            string MesOut = port.ReadLine();
            //Console.WriteLine("direct output = " + MesOut);
            List<string> MesSplit = MesOut.Split(',').ToList<string>();
            return MesSplit;
        }

    };
}

