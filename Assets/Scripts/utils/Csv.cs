using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace util
{
    public class Csv
    {
        public string path;
        private string[] lines;

        public Csv(string path)
        {
            this.path = path;
        }

        public string[] Read()
        {
            if (lines != null)
            {
                return lines;
            }
            else
            {
                if (File.Exists(path))
                {
                    Log.v(path + " Exists! Loading...");
                    string text = File.ReadAllText(path);
                    lines = text.Split('\n');

                    if (lines.Length > 0)
                    {
                        return lines.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    }
                    else
                    {
                        Log.e("Error Parsing File!");
                        return null;
                    }
                }
                else
                {
                    Log.e(path + " does not exist!");
                    return null;
                }
            }
        }

        public void writeLineToTerm(string line)
        {
            Console.WriteLine(line);
        }

        public void writeLinesToTerm(List<string> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                Console.WriteLine(lines[i]);
            }
        }

        public float[,] loadArray(vector2 size)
        {
            float[,] data = new float[size.x, size.y];

            List<string> lines = new List<string>(Read());

            for (int i = 0; i < lines.Count; i++)
            {
                string[] split = lines[i].Split(',');

                for (int j = 0; j < split.Length - 1; j++)
                {
                    if (!float.TryParse(split[j], out data[i, j]))
                    {
                        Log.e("Error loading : " + path);
                        return null;
                    }
                }
            }

            return data;
        }

        public void Write(string line)
        {
            using (var fileStream = File.Create(path))
            {
                using (var fileWriter = new StreamWriter(fileStream))
                {
                    fileWriter.WriteLine(line);
                }
            }
        }

        public void Write(List<string> lines)
        {
            Directory.CreateDirectory(Path.GetDirectoryName((path)));

            using (var fileStream = File.Create(path))
            {
                using (var fileWriter = new StreamWriter(fileStream))
                {
                    for (int i = 0; i < lines.Count; i++)
                    {
                        fileWriter.WriteLine(lines[i]);
                    }
                }
            }
        }
    }
}
