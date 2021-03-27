using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace winforms_image_processor
{
    public static class ShapeSerializer
    {
        public static BindingList<T> Load<T>(string filename) where T : class
        {
            if (File.Exists(filename))
            {
                BindingList<T> bList = new BindingList<T>();

                using (Stream stream = File.OpenRead(filename))
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    while (stream.Position != stream.Length)
                    {
                        object result = formatter.Deserialize(stream);
                        bList.Add((T)result);
                    }

                    return bList;
                }
            }
            return new BindingList<T>();
        }

        public static void Save<T>(string filename, BindingList<T> data) where T : class
        {
            using (Stream stream = File.OpenWrite(filename))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                foreach (var shape in data)
                {
                    var shaped = shape as Shape;
                    switch (shaped.shapeType)
                    {
                        case DrawingShape.CIRCLE:
                            formatter.Serialize(stream, (MidPointCircle)shaped);
                            break;
                        case DrawingShape.LINE:
                            formatter.Serialize(stream, (MidPointLine)shaped);
                            break;
                        case DrawingShape.POLY:
                            formatter.Serialize(stream, (Polygon)shaped);
                            break;
                        case DrawingShape.CPOLY:
                            formatter.Serialize(stream, (ClippedPolygon)shaped);
                            break;
                        case DrawingShape.CAPS:
                            formatter.Serialize(stream, (Capsule)shaped);
                            break;
                        case DrawingShape.RECT:
                            formatter.Serialize(stream, (Rectangle)shaped);
                            break;
                        case DrawingShape.FILL:
                            formatter.Serialize(stream, (Fill)shaped);
                            break;
                    }
                }
            }
        }

    }
}
