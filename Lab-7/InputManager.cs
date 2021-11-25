// https://docs.microsoft.com/en-us/nuget/consume-packages/install-use-packages-visual-studio

using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

using Psim.Materials;

namespace Psim.IOManagers
{
    public static class InputManager
    {
        public static Model InitializeModel(string path)
        {
            JObject modelData = LoadJson(path);
            // This model can only handle 1 material
            Material material = GetMaterial(modelData["materials"][0]);
            Model model = GetModel(material, modelData["settings"]);
            AddSensors(model, modelData["sensors"]);
            AddCells(model, modelData["cells"]);
            return model;
        }
        private static JObject LoadJson(string path)
        {
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                JObject modelData = JObject.Parse(json);
                return modelData;
            }
        }

        private static void AddCells(Model m, JToken cellData)
        {
            
            var length = (double)cellData["length"];
            var width = (double)cellData["width"];
            var sensorid = (int)cellData["sensorID"];
            foreach (double cells in cellData)
            {
                System.Console.WriteLine(cells);
                System.Console.WriteLine($"Successfully added a ${length}x{width} to the model. The cell is linked to {sensorid}");
                System.Console.WriteLine(m);
            }
            
        }

        private static void AddSensors(Model m, JToken sensorData)
        {
            
            var id = (int)sensorData["id"];
            var tinit = (int)sensorData["t_init"];
            foreach (int sensors in sensorData)
            {
                System.Console.WriteLine(sensors);
                System.Console.WriteLine($"Successfully added sensor ${id} to the model. The sensor's initial temperature is {tinit}");
                System.Console.WriteLine(m);
            }

            
        }

        private static Model GetModel(Material material, JToken settingsData)
        {
            
            var highTemp = (double)settingsData["high_temp"];
            var lowTemp = (double)settingsData["low_temp"];
            var simTime = (double)settingsData["sim_time"];
            return new Model(material,highTemp,lowTemp,simTime);
           
            
        }

        private static Material GetMaterial(JToken materialData)
        {
            var dData = GetDispersionData(materialData["d_data"]);
            var rData = GetRelaxationData(materialData["r_data"]);
            return new Material(dData,rData);

        }

        private static DispersionData GetDispersionData(JToken dData)
        {
            var wMaxla = (double)dData["max_freq_la"];
            var wMaxTa = (double)dData["max_freq_Ta"];
            var laData = dData["la_data"].ToObject<double[]>();
            var taData = dData["ta_data"].ToObject<double[]>();
            return new DispersionData(laData,wMaxla,taData,wMaxTa);

        }

        private static RelaxationData GetRelaxationData(JToken rData)
        {
            
            var Bl = (double)rData["b_l"];
            var Btn = (double)rData["b_tn"];
            var Btu = (double)rData["b_tu"];
            var BI = (double)rData["b_i"];
            var W = (double)rData["w"];
            return new RelaxationData(Bl,Btn,Btu,BI,W);

        }
    }
}
