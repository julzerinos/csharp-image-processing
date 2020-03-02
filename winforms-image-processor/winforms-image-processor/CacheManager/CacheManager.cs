using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace winforms_image_processor
{
    /// <summary>
    /// The CacheManager Class is responsible for storing caches of image filter states.
    /// This greatly saves processing time, but increases load on memory.
    /// </summary>
    static class CacheManager
    {
        public static List<string> filterState;
        public static Dictionary<List<string>, Bitmap> cachedFilterStates;

        public static void InitializeWithOriginal(Bitmap bmp)
        {
            filterState = new List<string>();

            cachedFilterStates = new Dictionary<List<string>, Bitmap>(new StringListEqComparer())
            {
                { new List<string>(), DeepCopy(bmp)}
            };
        }

        public static void UpdateFilterState(string filter, bool newChecked)
        {
            if (newChecked)
                filterState.Add(filter);
            else
                filterState.Remove(filter);
        }

        public static Bitmap GetBitmapForFilterState() => cachedFilterStates.ContainsKey(filterState) ? cachedFilterStates[filterState] : null;

        public static void SetBitmapForFilterState(Bitmap bmp) => cachedFilterStates[filterState] = DeepCopy(bmp);

        public static Bitmap GetOriginalImage() => cachedFilterStates[new List<string>()];

        private static T DeepCopy<T>(T source)
        // Sources used
        // Deepcopy w/ stream:      https://stackoverflow.com/questions/43039099/creating-a-completely-new-copy-of-bitmap-from-a-bitmap-in-c-sharp/43042865#43042865
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }

    public class StringListEqComparer : IEqualityComparer<List<string>>
    // Sources used
    // Custom compare:              https://stackoverflow.com/questions/14663168/an-integer-array-as-a-key-for-dictionary
    {
        public bool Equals(List<string> x, List<string> y)
        {
            if (x.Count != y.Count)
            {
                return false;
            }
            for (int i = 0; i < x.Count; i++)
            {
                if (x[i] != y[i])
                {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode(List<string> obj)
        {
            int result = 17;
            for (int i = 0; i < obj.Count; i++)
            {
                unchecked
                {
                    result = result * 23 + obj[i].GetHashCode();
                }
            }
            return result;
        }
    }
}
