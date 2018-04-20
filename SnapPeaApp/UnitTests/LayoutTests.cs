using System;
using System.IO;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SnapPeaApp;
using SnapPeaApp.ViewModels;

namespace UnitTests
{
    [TestClass]
    public class LayoutTests
    {
        private static string sampleLayout = "{\"Name\":\"test\",\"Regions\":[{\"LeftF\":0.004166667,\"TopF\":0.451485157,\"WidthF\":0.4904762,\"HeightF\":0.480198026,\"Color\":0},{\"LeftF\":0.5005952,\"TopF\":0.0881188139,\"WidthF\":0.483928561,\"HeightF\":0.446534663,\"Color\":0},{\"LeftF\":0.007142857,\"TopF\":0.0118811885,\"WidthF\":0.4857143,\"HeightF\":0.349504948,\"Color\":0}]}";

        /*
         * Ensure that creating a region with specified position
         * and size works as expected.
         */
        [TestMethod]
        public void CreateRegion()
        {
            Rectangle rect = new Rectangle(100, 200, 300, 400);

            SnapPeaApp.Region region = new SnapPeaApp.Region(rect);

            Assert.IsNotNull(region);

            Assert.AreEqual(region.Left, 100.0f, 0.000001f);
            Assert.AreEqual(region.Top, 200.0f, 0.000001f);
            Assert.AreEqual(region.Width, 300.0f, 0.000001f);
            Assert.AreEqual(region.Height, 400.0f, 0.000001f);
        }

        /*
         * Ensures that Region.IsPointIn functions as it should.
         */
        [TestMethod]
        public void RegionIsPointIn()
        {
            Rectangle rect = new Rectangle(100, 200, 300, 400);

            SnapPeaApp.Region region = new SnapPeaApp.Region(rect);

            Assert.IsNotNull(region);

            //0,0 is not in the region
            Assert.IsFalse(region.IsPointIn(new System.Windows.Point(0.0, 0.0)));

            //250,100 is not in the region (above)
            Assert.IsFalse(region.IsPointIn(new System.Windows.Point(250.0, 100.0)));

            //500,100 is not in the region (to the right)
            Assert.IsFalse(region.IsPointIn(new System.Windows.Point(500.0, 100.0)));

            //250, 400 is in the region (center)
            Assert.IsTrue(region.IsPointIn(new System.Windows.Point(250.0, 400.0)));
        }

        /*
         * Create an empty layout and ensure the object's
         * member fields are as expected.
         */
        [TestMethod]
        public void EmptyLayoutCreation()
        {
            Layout layout = new Layout();

            Assert.IsNotNull(layout);

            Assert.AreEqual(layout.Regions.Count, 0);
            Assert.AreEqual(layout.Name, "");
        }

        /*
         * Ensure the getter/setters for the layout name
         * works.
         */
        [TestMethod]
        public void LayoutSetGetName()
        {
            Layout layout = new Layout();

            Assert.IsNotNull(layout);

            Assert.AreEqual(layout.Name, "");
            layout.Name = "TEST";
            Assert.AreEqual(layout.Name, "TEST");
        }

        /*
         * Test adding regions to the layout.
         * 
         * Several regions are added and then the region
         * list is checked to ensure they made it there.
         */
        [TestMethod]
        public void LayoutAddRegions()
        {
            Layout layout = new Layout();

            Assert.IsNotNull(layout);

            Assert.AreEqual(layout.Regions.Count, 0);

            //create the regions we will add
            Rectangle rect1 = new Rectangle(0, 0, 0, 0);
            SnapPeaApp.Region region1 = new SnapPeaApp.Region(rect1);
            Rectangle rect2 = new Rectangle(100, 200, 300, 400);
            SnapPeaApp.Region region2 = new SnapPeaApp.Region(rect2);

            //add the first region
            layout.AddRegion(region1);

            //the region count should now be 1
            Assert.AreEqual(layout.Regions.Count, 1);

            //ensure that the layout does in fact contain the region
            Assert.IsTrue(layout.Regions.Contains(region1));

            //the layout should not contain region2 since we did not add it yet
            Assert.IsFalse(layout.Regions.Contains(region2));

            //add the second
            layout.AddRegion(region2);

            //there should now be 2 regions
            Assert.AreEqual(layout.Regions.Count, 2);

            //now, the layout should contain both regions
            Assert.IsTrue(layout.Regions.Contains(region2));
            Assert.IsTrue(layout.Regions.Contains(region1));
        }

        /*
         * Test loading the layout from a file.
         * Due to the way the test is implemented,
         * screen resolution scaling is also tested.
         * 
         * A temp file is created with a layout.
         */
        [TestMethod]
        public void LayoutLoadLayout()
        {
            string path = Path.GetTempFileName();

            //save the file we will use for testing
            File.WriteAllText(path, sampleLayout);

            Layout layout = Layout.LoadLayout(path);

            //get rid of the temp file before we proceed
            File.Delete(path);

            Assert.IsNotNull(layout);

            //ensure that the layout has 3 regions
            Assert.AreEqual(layout.Regions.Count, 3);

            //we will initialize arrays containing the expected values
            //for the regions
            int[] expectedLeft = new int[3]
            {
                (int)(0.004166667f * Screen.PrimaryScreen.WorkingArea.Width),
                (int)(0.5005952f * Screen.PrimaryScreen.WorkingArea.Width),
                (int)(0.007142857f * Screen.PrimaryScreen.WorkingArea.Width)
            };

            int[] expectedTop = new int[3]
            {
                (int)(0.451485157f * Screen.PrimaryScreen.WorkingArea.Height),
                (int)(0.0881188139f * Screen.PrimaryScreen.WorkingArea.Height),
                (int)(0.0118811885f * Screen.PrimaryScreen.WorkingArea.Height)
            };

            int[] expectedWidth = new int[3]
            {
                (int)(0.4904762f * Screen.PrimaryScreen.WorkingArea.Width),
                (int)(0.483928561f * Screen.PrimaryScreen.WorkingArea.Width),
                (int)(0.4857143f * Screen.PrimaryScreen.WorkingArea.Width)
            };

            int[] expectedHeight = new int[3]
            {
                (int)(0.480198026f * Screen.PrimaryScreen.WorkingArea.Height),
                (int)(0.446534663f * Screen.PrimaryScreen.WorkingArea.Height),
                (int)(0.349504948f * Screen.PrimaryScreen.WorkingArea.Height)
            };

            //now go through each region and ensure that it has the
            //expected values.
            //this may actually fail if the order is different
            for (int i = 0; i < 3; ++i)
            {
                Assert.AreEqual(layout.Regions[i].Left, expectedLeft[i]);
                Assert.AreEqual(layout.Regions[i].Top, expectedTop[i]);
                Assert.AreEqual(layout.Regions[i].Width, expectedWidth[i]);
                Assert.AreEqual(layout.Regions[i].Height, expectedHeight[i]);
            }
        }
    }
}