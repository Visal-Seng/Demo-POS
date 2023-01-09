using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace DemoDataGridView
{
    public class IOManager
    {
        public string path = "./data/";
        public string extension = ".json";

        public void Save(object datas, string fileName)
        {
            string json = JsonConvert.SerializeObject(datas);
            string fullPath = path + fileName + extension;
            WriteText(json, fullPath);
        }

        public T Load <T>(string fileName)
        {
            string fullPath = path + fileName + extension;
            string json =  Readtext(fullPath);
            return JsonConvert.DeserializeObject<T>(json);
        }
        private string Readtext(string fullPath)
        {
            if (File.Exists(fullPath))
            {
                StreamReader sr = new StreamReader(fullPath);
                string json = sr.ReadToEnd();
                sr.Close();
                return json;
            }
            return "[]";
        }
        private void WriteText(string json, string fullPath)
        {
            StreamWriter sw = new StreamWriter(fullPath);
            sw.Write(json);
            sw.Close();
        }
    }
}
