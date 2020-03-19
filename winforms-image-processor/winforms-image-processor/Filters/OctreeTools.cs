using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winforms_image_processor
{
    // Source for below classes
    // https://github.com/pmichna/NewOctree

    internal class OctreeNode
    {
        private static readonly Byte[] Mask = new Byte[] { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };

        private int red;
        private int green;
        private int blue;

        private int pixelCount;
        private int paletteIndex;

        private readonly OctreeNode[] nodes;

        public OctreeNode(int level, OctreeQuantizer parent) //level - level of parent
        {
            nodes = new OctreeNode[8];

            if (level < 7)
            {
                parent.AddLevelNode(level, this);
            }
        }

        public Boolean IsLeaf
        {
            get { return pixelCount > 0; }
        }

        public Color Color
        {
            get
            {
                Color result;

                // determines a color of the leaf
                if (IsLeaf)
                {
                    result = pixelCount == 1 ?
                        Color.FromArgb(255, red, green, blue) :
                        Color.FromArgb(255, red / pixelCount, green / pixelCount, blue / pixelCount);
                }
                else
                {
                    throw new InvalidOperationException("Cannot retrieve a color for other node than leaf.");
                }

                return result;
            }
        }

        public int ActiveNodesPixelCount
        {
            get
            {
                int result = pixelCount;

                // sums up all the pixel presence for all the active nodes
                for (int index = 0; index < 8; index++)
                {
                    OctreeNode node = nodes[index];

                    if (node != null)
                    {
                        result += node.pixelCount;
                    }
                }

                return result;
            }
        }

        public IEnumerable<OctreeNode> ActiveNodes
        {
            get
            {
                List<OctreeNode> result = new List<OctreeNode>();

                // adds all the active sub-nodes to a list
                for (int index = 0; index < 8; index++)
                {
                    OctreeNode node = nodes[index];

                    if (node != null)
                    {
                        if (node.IsLeaf)
                        {
                            result.Add(node);
                        }
                        else
                        {
                            result.AddRange(node.ActiveNodes);
                        }
                    }
                }

                return result;
            }
        }

        public void AddColor(Color color, int level, OctreeQuantizer parent) //level - depth level
        {
            // if this node is a leaf, then increase a color amount, and pixel presence
            if (level == 8)
            {
                red += color.R;
                green += color.G;
                blue += color.B;
                pixelCount++;
            }
            else if (level < 8) // otherwise goes one level deeper
            {
                // calculates an index for the next sub-branch
                int index = GetColorIndexAtLevel(color, level); //current level

                // if that branch doesn't exist, grows it
                if (nodes[index] == null)
                {
                    nodes[index] = new OctreeNode(level, parent);
                }

                // adds a color to that branch
                nodes[index].AddColor(color, level + 1, parent);
            }
        }

        public int GetPaletteIndex(Color color, int level)
        {
            int result;

            // if a node is leaf, then we've found the best match already
            if (IsLeaf)
            {
                result = paletteIndex;
            }
            else // otherwise continue in to the lower depths
            {
                int index = GetColorIndexAtLevel(color, level);

                result = nodes[index] != null ? nodes[index].GetPaletteIndex(color, level + 1) : nodes.
                    Where(node => node != null).
                    First().
                    GetPaletteIndex(color, level + 1);
            }

            return result;
        }

        public int RemoveLeaves()
        {
            int result = 0;

            // scans thru all the active nodes
            for (int index = 0; index < 8; index++)
            {
                OctreeNode node = nodes[index];

                if (node != null)
                {
                    // sums up their color components
                    red += node.red;
                    green += node.green;
                    blue += node.blue;

                    // and pixel presence
                    pixelCount += node.pixelCount;

                    // increases the count of reduced nodes
                    result++;
                }
            }

            // returns a number of reduced sub-nodes, minus one because this node becomes a leaf
            return result - 1;
        }

        private static int GetColorIndexAtLevel(Color color, int level)
        {
            return ((color.R & Mask[level]) == Mask[level] ? 4 : 0) |
                   ((color.G & Mask[level]) == Mask[level] ? 2 : 0) |
                   ((color.B & Mask[level]) == Mask[level] ? 1 : 0);
        }

        internal void SetPaletteIndex(int index)
        {
            paletteIndex = index;
        }
    }



    public class OctreeQuantizer
    {

        private OctreeNode root;
        private List<OctreeNode>[] levels;

        public OctreeQuantizer()
        {
            Prepare();
        }

        private void Prepare()
        {
            levels = new List<OctreeNode>[7];
            // creates the octree level lists
            for (Int32 level = 0; level < 7; level++)
            {
                levels[level] = new List<OctreeNode>();
            }
            root = new OctreeNode(0, this);
        }

        public void Init()
        {
            Prepare();
        }

        public void Clear()
        {
            Prepare();
        }


        internal IEnumerable<OctreeNode> Leaves
        {
            get { return root.ActiveNodes.Where(node => node.IsLeaf); }
        }

        internal void AddLevelNode(int level, OctreeNode octreeNode)
        {
            levels[level].Add(octreeNode);
        }

        public void AddColor(Color color)
        {
            root.AddColor(color, 0, this);
        }

        public List<Color> GetPalette(int colorCount)
        {
            List<Color> result = new List<Color>();
            int leafCount = Leaves.Count();
            int paletteIndex = 0;

            // goes thru all the levels starting at the deepest, and goes upto a root level
            for (int level = 6; level >= 0; level--)
            {
                // if level contains any node
                if (levels[level].Count > 0)
                {
                    // orders the level node list by pixel presence (those with least pixels are at the top)
                    IEnumerable<OctreeNode> sortedNodeList = levels[level].OrderBy(node => node.ActiveNodesPixelCount);

                    // removes the nodes unless the count of the leaves is lower or equal than our requested color count
                    foreach (OctreeNode node in sortedNodeList)
                    {
                        // removes a node
                        leafCount -= node.RemoveLeaves();

                        // if the count of leaves is lower then our requested count terminate the loop
                        if (leafCount <= colorCount) break;
                    }

                    // if the count of leaves is lower then our requested count terminate the level loop as well
                    if (leafCount <= colorCount) break;

                    // otherwise clear whole level, as it is not needed anymore
                    levels[level].Clear();
                }
            }

            // goes through all the leaves that are left in the tree (there should now be less or equal than requested)
            foreach (OctreeNode node in Leaves.OrderByDescending(node => node.ActiveNodesPixelCount))
            {
                if (paletteIndex >= colorCount) break;

                // adds the leaf color to a palette
                if (node.IsLeaf)
                {
                    result.Add(node.Color);
                }

                // and marks the node with a palette index
                node.SetPaletteIndex(paletteIndex++);
            }

            // we're unable to reduce the Octree with enough precision, and the leaf count is zero
            if (result.Count == 0)
            {
                throw new NotSupportedException("The Octree contains after the reduction 0 colors.");
            }

            // returns the palette
            return result;
        }

        public int GetPaletteIndex(Color color)
        {
            // retrieves a palette index
            return root.GetPaletteIndex(color, 0);
        }
    }
}



