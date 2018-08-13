using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
                    var text = File.ReadAllText(path);
                    lines = text.Split('\n');

                    if (lines.Length > 0)
                    {
                        return lines.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
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
            for (var i = 0; i < lines.Count; i++)
            {
                Console.WriteLine(lines[i]);
            }
        }

        public float[,] loadArray(vector2 size)
        {
            var data = new float[size.x, size.y];

            var lines = new List<string>(Read());

            for (var i = 0; i < lines.Count; i++)
            {
                var split = lines[i].Split(',');

                for (var j = 0; j < split.Length - 1; j++)
                {
                    if (!float.TryParse(split[j], out data[i, j]))
                    {
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
                    for (var i = 0; i < lines.Count; i++)
                    {
                        fileWriter.WriteLine(lines[i]);
                    }
                }
            }
        }
    }
}