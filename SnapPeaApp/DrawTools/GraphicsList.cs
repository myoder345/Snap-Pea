#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

#endregion

namespace DrawTools
{
    using DrawList = List<DrawObject>;

    /// <summary>
    /// List of graphic objects
    /// </summary>
    public class GraphicsList
    {
        #region Members

        private DrawList graphicsList;

        private const string entryCount = "Count";
        private const string entryType = "Type";
        /// <summary>
        /// Gets invoked when the collection or an object in the collection changes
        /// </summary>
        public Action GraphicsListChanged { get; set; }
        #endregion

        #region Constructor
        public GraphicsList()
        {
            graphicsList = new DrawList();
        }

        #endregion
        
        #region Other functions
        /// <summary>
        /// Draws each object in the collection
        /// </summary>
        /// <param name="graphics"></param>
        public void Draw(Graphics graphics)
        {
            int n = graphicsList.Count;
            DrawObject o;

            // Enumerate list in reverse order to get first
            // object on the top of Z-order.
            for (int i = n - 1; i >= 0; i--)
            {
                o = graphicsList[i];

                o.Draw(graphics);

                if (o.Selected == true)
                {
                    o.DrawTracker(graphics);
                }
            }
        }

        /// <summary>
        /// Dump (for debugging)
        /// </summary>
        public void Dump()
        {
            Trace.WriteLine("");

            foreach ( DrawObject o in graphicsList )
            {
                o.Dump();
            }
        }

        /// <summary>
        /// Clear all objects in the list
        /// </summary>
        /// <returns>
        /// true if at least one object is deleted
        /// </returns>
        public bool Clear()
        {
            bool result = (graphicsList.Count > 0);
            graphicsList.Clear();
            return result;
        }

        /// <summary>
        /// Count and this [nIndex] allow to read all graphics objects
        /// from GraphicsList in the loop.
        /// </summary>
        public int Count
        {
            get
            {
                return graphicsList.Count;
            }
        }


        /// <summary>
        /// subscript operator
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DrawObject this[int index]
        {
            get
            {
                if (index < 0 || index >= graphicsList.Count)
                    return null;

                return graphicsList[index];
            }
        }

        /// <summary>
        /// SelectedCount and GetSelectedObject allow to read
        /// selected objects in the loop
        /// </summary>
        public int SelectionCount
        {
            get
            {
                return Selection.Count();
            }
        }


        /// <summary>
        /// Returns INumerable object which may be used for enumeration
        /// of selected objects.
        /// 
        /// Note: returning IEnumerable<DrawObject> breaks CLS-compliance
        /// (assembly CLSCompliant = true is removed from AssemblyInfo.cs).
        /// To make this program CLS-compliant, replace 
        /// IEnumerable<DrawObject> with IEnumerable. This requires
        /// casting to object at runtime.
        /// </summary>
        /// <value></value>
        public IEnumerable<DrawObject> Selection
        {
            get
            {
                foreach (DrawObject o in graphicsList)
                {
                    if (o.Selected)
                    {
                        yield return o;
                    }
                }
            }
        }

        /// <summary>
        /// Gets an enumerable over all objects in the collections
        /// </summary>
        public IEnumerable<DrawObject> Enumeration
        {
            get
            {
                foreach (DrawObject o in graphicsList)
                {
                    yield return o;
                    
                }
            }
        }

        /// <summary>
        /// Adds a draw object to the collection
        /// </summary>
        /// <param name="obj"></param>
        public void Add(DrawObject obj)
        {
            // insert to the top of z-order
            graphicsList.Insert(0, obj);
        }

        /// <summary>
        /// Insert object to specified place.
        /// Used for Undo.
        /// </summary>
        public void Insert(int index, DrawObject obj)
        {
            if ( index >= 0  && index < graphicsList.Count )
            {
                graphicsList.Insert(index, obj);
            }
        }

        /// <summary>
        /// Replace object in specified place.
        /// Used for Undo.
        /// </summary>
        public void Replace(int index, DrawObject obj)
        {
            if (index >= 0 && index < graphicsList.Count)
            {
                graphicsList.RemoveAt(index);
                graphicsList.Insert(index, obj);
            }
        }

        /// <summary>
        /// Remove object by index.
        /// Used for Undo.
        /// </summary>
        public void RemoveAt(int index)
        {
            graphicsList.RemoveAt(index);
        }

        /// <summary>
        /// Delete last added object from the list
        /// (used for Undo operation).
        /// </summary>
        public void DeleteLastAddedObject()
        {
            if ( graphicsList.Count > 0 )
            {
                graphicsList.RemoveAt(0);
            }
        }

        /// <summary>
        /// Unselects any selected shapes
        /// </summary>
        public void UnselectAll()
        {
            foreach (DrawObject o in graphicsList)
            {
                o.Selected = false;
            }
        }

        /// <summary>
        /// Delete selected items
        /// </summary>
        /// <returns>
        /// true if at least one object is deleted
        /// </returns>
        public bool DeleteSelection()
        {
            bool result = false;

            int n = graphicsList.Count;

            for (int i = n - 1; i >= 0; i--)
            {
                if (((DrawObject)graphicsList[i]).Selected)
                {
                    graphicsList.RemoveAt(i);
                    result = true;
                }
            }

            return result;
        }


        /// <summary>
        /// Move selected items to front (beginning of the list)
        /// </summary>
        /// <returns>
        /// true if at least one object is moved
        /// </returns>
        public bool MoveSelectionToFront()
        {
            int n;
            int i;
            DrawList tempList;

            tempList = new DrawList();
            n = graphicsList.Count;

            // Read source list in reverse order, add every selected item
            // to temporary list and remove it from source list
            for (i = n - 1; i >= 0; i--)
            {
                if ((graphicsList[i]).Selected)
                {
                    tempList.Add(graphicsList[i]);
                    graphicsList.RemoveAt(i);
                }
            }

            // Read temporary list in direct order and insert every item
            // to the beginning of the source list
            n = tempList.Count;

            for (i = 0; i < n; i++)
            {
                graphicsList.Insert(0, tempList[i]);
            }

            return (n > 0);
        }

        /// <summary>
        /// Move selected items to back (end of the list)
        /// </summary>
        /// <returns>
        /// true if at least one object is moved
        /// </returns>
        public bool MoveSelectionToBack()
        {
            int n;
            int i;
            DrawList tempList;

            tempList = new DrawList();
            n = graphicsList.Count;

            // Read source list in reverse order, add every selected item
            // to temporary list and remove it from source list
            for (i = n - 1; i >= 0; i--)
            {
                if ((graphicsList[i]).Selected)
                {
                    tempList.Add(graphicsList[i]);
                    graphicsList.RemoveAt(i);
                }
            }

            // Read temporary list in reverse order and add every item
            // to the end of the source list
            n = tempList.Count;

            for (i = n - 1; i >= 0; i--)
            {
                graphicsList.Add(tempList[i]);
            }

            return (n > 0);
        }

        /// <summary>
        /// Sets all shapes to the given color
        /// </summary>
        /// <param name="color"></param>
        public void SetColorToAll(Color color)
        {
            foreach(DrawObject o in Enumeration)
            {
                o.Color = color;
            }
        }
        #endregion
    }
}
