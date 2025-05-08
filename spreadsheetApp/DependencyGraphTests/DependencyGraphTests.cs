using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
namespace DevelopmentTests
{
    /// <summary>
    /// Author: Andrew Winward
    /// Partner: -none-
    /// Date: 1/27/24
    /// Course: CS 3500
    /// 
    /// This MSTEST file is a tester for the DependencyGraph program.
    /// </summary>


    /// <summary>
    ///This is a test class for DependencyGraphTest and is intended
    ///to contain all DependencyGraphTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DependencyGraphTest
    {
        /// <summary>
        /// Tests the Dependency add method
        /// </summary>
        [TestMethod()]
        public void SimpleAddTest()
        {
            var graph = new DependencyGraph();
            graph.AddDependency("A", "B");

            Assert.IsTrue(graph.HasDependents("A"));
            Assert.IsTrue(graph.GetDependents("A").Contains("B"));

            Assert.IsTrue(graph.HasDependees("B"));
            Assert.IsTrue(graph.GetDependees("B").Contains("A"));
        }

        /// <summary>
        /// Tests the add method with multiple added dependencies
        /// </summary>
        [TestMethod()]
        public void multipleAddTest()
        {
            var graph = new DependencyGraph();

           
            graph.AddDependency("A","B");//
            graph.AddDependency("A","C");//
            graph.AddDependency("D","C");//
            graph.AddDependency("B", "C");//
            graph.AddDependency("C", "E");

            // A B
            Assert.IsTrue(graph.HasDependents("A"));
            Assert.IsTrue(graph.GetDependents("A").Contains("B"));
            
            Assert.IsTrue(graph.HasDependees("B"));
            Assert.IsTrue(graph.GetDependees("B").Contains("A"));
            //A C
            Assert.IsTrue(graph.HasDependents("A"));
            Assert.IsTrue(graph.GetDependents("A").Contains("C"));

            Assert.IsTrue(graph.HasDependees("C"));
            Assert.IsTrue(graph.GetDependees("C").Contains("A"));
            //D C
            Assert.IsTrue(graph.HasDependents("D"));
            Assert.IsTrue(graph.GetDependents("D").Contains("C"));

            Assert.IsTrue(graph.HasDependees("C"));
            Assert.IsTrue(graph.GetDependees("C").Contains("D"));

            //B C
            Assert.IsTrue(graph.HasDependents("B"));
            Assert.IsTrue(graph.GetDependents("B").Contains("C"));
           
            Assert.IsTrue(graph.HasDependees("C"));
            Assert.IsTrue(graph.GetDependees("C").Contains("B"));
           
            //C,E
            Assert.IsTrue(graph.HasDependents("C"));
            Assert.IsTrue(graph.GetDependents("C").Contains("E"));

            Assert.IsTrue(graph.HasDependees("E"));
            Assert.IsTrue(graph.GetDependees("E").Contains("C"));


        }
        /// <summary>
        /// Simple use of the removal method
        /// </summary>
        [TestMethod()]
        public void SimpleRemoveTest()
        {
            var graph = new DependencyGraph();

            graph.AddDependency("A", "B");

            Assert.AreEqual(1, graph.Size);

            graph.RemoveDependency("A", "B");

            Assert.AreEqual(0, graph.Size);
        }


        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void SimpleEmptyTest()
        {
            var t = new DependencyGraph();
            Assert.AreEqual(0, t.Size);
        }
        
        /// <summary>
        ///Empty graph with removal method
        ///</summary>
        [TestMethod()]
        public void SimpleEmptyRemoveTest()
        {
            var t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(1, t.Size);
            t.RemoveDependency("x", "y");
            Assert.AreEqual(0, t.Size);
        }
        
        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void SimpleEmptyHasTest()
        {
            DependencyGraph t = new DependencyGraph();

            Assert.IsFalse(t.HasDependents("x"));
            Assert.IsFalse(t.HasDependees("x"));
        }


        /// <summary>
        ///Making sure that the empty test does throw a exception
        ///with removing zero pairs
        ///</summary>
        [TestMethod()]
        public void emptyRemoveTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.RemoveDependency("x", "y");

        }
        /// <summary>
        /// Checking to see if all the has, and get methods function properly,
        /// and to see if add and remove work on a broad graph
        /// </summary>
        [TestMethod()]
        public void multipleAddRemoveTest()
        {
            var graph = new DependencyGraph();


            graph.AddDependency("A", "B");//
            graph.AddDependency("A", "C");//
            graph.AddDependency("D", "C");//
            graph.AddDependency("B", "C");//
            graph.AddDependency("C", "E");

            // A B
            Assert.IsTrue(graph.HasDependents("A"));
            Assert.IsTrue(graph.GetDependents("A").Contains("B"));

            Assert.IsTrue(graph.HasDependees("B"));
            Assert.IsTrue(graph.GetDependees("B").Contains("A"));
            //A C
            Assert.IsTrue(graph.HasDependents("A"));
            Assert.IsTrue(graph.GetDependents("A").Contains("C"));

            Assert.IsTrue(graph.HasDependees("C"));
            Assert.IsTrue(graph.GetDependees("C").Contains("A"));
            //D C
            Assert.IsTrue(graph.HasDependents("D"));
            Assert.IsTrue(graph.GetDependents("D").Contains("C"));

            Assert.IsTrue(graph.HasDependees("C"));
            Assert.IsTrue(graph.GetDependees("C").Contains("D"));

            //B C
            Assert.IsTrue(graph.HasDependents("B"));
            Assert.IsTrue(graph.GetDependents("B").Contains("C"));

            Assert.IsTrue(graph.HasDependees("C"));
            Assert.IsTrue(graph.GetDependees("C").Contains("B"));

            //C,E
            Assert.IsTrue(graph.HasDependents("C"));
            Assert.IsTrue(graph.GetDependents("C").Contains("E"));

            Assert.IsTrue(graph.HasDependees("E"));
            Assert.IsTrue(graph.GetDependees("E").Contains("C"));

            graph.RemoveDependency("A", "B");//
            graph.RemoveDependency("A", "C");//
            graph.RemoveDependency("D", "C");//
            graph.RemoveDependency("B", "C");//
            graph.RemoveDependency("C", "E");

            Assert.AreEqual(0, graph.Size);
            Assert.AreEqual(0, graph["A"]);
        }
        
        /// <summary>
        /// testing the this[] method to make sure it returns the correct 
        /// amount of dependees
        /// </summary>
        [TestMethod()]
        public void dependeeSizeTest()
        {
            var graph = new DependencyGraph();

            graph.AddDependency("A", "B");
            graph.AddDependency("A", "C");
            graph.AddDependency("B", "C");
            graph.AddDependency("C", "D");

            int sizeA = graph["A"];
            int sizeB = graph["B"];
            int sizeC = graph["C"];
            int sizeD = graph["D"];

            Assert.AreEqual(0, sizeA);
            Assert.AreEqual(1, sizeB);
            Assert.AreEqual(2, sizeC);
            Assert.AreEqual(1, sizeD);

        }

        /// <summary>
        /// Checking to see if multiple graphs run correctly
        /// </summary>
        [TestMethod()]
        public void multipleGraphsTest()
        {
            var graph1 = new DependencyGraph();
            var graph2 = new DependencyGraph();
            var graph3 = new DependencyGraph();

            graph1.AddDependency("A", "B");
            graph2.AddDependency("A", "C");
            graph3.AddDependency("B", "C");

            Assert.AreEqual(1, graph1.Size);
            Assert.AreEqual(1, graph2.Size);
            Assert.AreEqual(1, graph3.Size);
        }

        /// <summary>
        /// Checking to see if it handles duplicates correctly
        /// </summary>
        [TestMethod()]
        public void duplicateSizeTest()
        {
            var graph = new DependencyGraph();

            graph.AddDependency("A", "B");
            graph.AddDependency("B", "C");
            graph.AddDependency("A", "B");
            graph.AddDependency("C", "D");
            graph.AddDependency("D", "B");
            graph.AddDependency("C", "A");
            graph.AddDependency("A", "B");
            graph.AddDependency("A", "C");



            Assert.AreEqual(6, graph.Size);
           
        }

        /// <summary>
        /// Checking to see if it handles duplicates correctly with dependees size method
        /// </summary>
        [TestMethod()]
        public void duplicateDependeeSizeTest()
        {
            var graph = new DependencyGraph();

            graph.AddDependency("A", "B");
            graph.AddDependency("B", "C");
            graph.AddDependency("A", "B");
            graph.AddDependency("C", "D");
            graph.AddDependency("D", "B");
            graph.AddDependency("C", "A");
            graph.AddDependency("A", "B");
            graph.AddDependency("A", "C");



            Assert.AreEqual(2, graph["C"]);

        }
        
        /// <summary>
        /// Testing to see if it handles Nulls and empty String correctly
        /// </summary>
        [TestMethod()]
        public void nullEmptyStringTest()
        {
            var graph = new DependencyGraph();

            
            Assert.ThrowsException<ArgumentException>(() => graph.AddDependency(null, "B"));
            Assert.ThrowsException<ArgumentException>(() => graph.AddDependency("A", null));
            Assert.ThrowsException<ArgumentException>(() => graph.AddDependency("", "B"));
            Assert.ThrowsException<ArgumentException>(() => graph.AddDependency("A", ""));

            
            Assert.ThrowsException<ArgumentException>(() => graph.RemoveDependency(null, "B"));
            Assert.ThrowsException<ArgumentException>(() => graph.RemoveDependency("A", null));
            Assert.ThrowsException<ArgumentException>(() => graph.RemoveDependency("", "B"));
            Assert.ThrowsException<ArgumentException>(() => graph.RemoveDependency("A", ""));
        }
        
        /// <summary>
        /// checking to see if it handles an empty removal
        /// </summary>
        [TestMethod()]
        public void emptyRemovalTest()
        {
            var graph = new DependencyGraph();

            Assert.AreEqual(0, graph.Size);
            graph.RemoveDependency("X", "Y");
            Assert.AreEqual(0, graph.Size);
        }
        
        /// <summary>
        /// This test is to see if the replace methods work correctly
        /// </summary>
        [TestMethod()]
        public void replaceDependentsAndDepeesTest()
        {
            var graph = new DependencyGraph();

           
            graph.AddDependency("A", "B");
            graph.AddDependency("A", "C");
            graph.AddDependency("D", "C");

            graph.ReplaceDependents("A", new List<string> { "D", "E" });
            Assert.AreEqual(2, graph.GetDependents("A").Count());
            Assert.IsTrue(graph.GetDependents("A").Contains("D"));
            Assert.IsTrue(graph.GetDependents("A").Contains("E"));

            graph.ReplaceDependees("C", new List<string> { "X", "Y" });
            Assert.AreEqual(2, graph.GetDependees("C").Count());
            Assert.IsTrue(graph.GetDependees("C").Contains("X"));
            Assert.IsTrue(graph.GetDependees("C").Contains("Y"));
        }
       
        /// <summary>
        /// This is a big iteration to see if it handles a big DG
        /// </summary>
        [TestMethod()]
        public void largeDGTest()
        {
            var graph = new DependencyGraph();
          //got help from ChatGPT to set up this test, did not know the syntax to add to the Node
            for (int i = 0; i < 1000; i++)
            {
                graph.AddDependency($"Node{i}", $"Node{i + 1}");
            }

            Assert.AreEqual(1000, graph.Size);
            Assert.AreEqual(0, graph.GetDependents("Node1000").Count());
        }




        /////////////////////////Given Test Cases/////////////////////////////////



        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]

        public void EmptyEnumeratorTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            IEnumerator<string> e1 = t.GetDependees("y").GetEnumerator();
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("x", e1.Current);
            IEnumerator<string> e2 = t.GetDependents("x").GetEnumerator();
            Assert.IsTrue(e2.MoveNext());
            Assert.AreEqual("y", e2.Current);
            t.RemoveDependency("x", "y");
            Assert.IsFalse(t.GetDependees("y").GetEnumerator().MoveNext());
            Assert.IsFalse(t.GetDependents("x").GetEnumerator().MoveNext());
        }
        /// <summary>
        ///Replace on an empty DG shouldn't fail
        ///</summary>
        [TestMethod()]
        public void SimpleReplaceTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(t.Size, 1);
            t.RemoveDependency("x", "y");
            t.ReplaceDependents("x", new HashSet<string>());
            t.ReplaceDependees("y", new HashSet<string>());
        }
        ///<summary>
        ///It should be possibe to have more than one DG at a time.
        ///</summary>
        [TestMethod()]
        public void StaticTest()
        {
            DependencyGraph t1 = new DependencyGraph();
            DependencyGraph t2 = new DependencyGraph();
            t1.AddDependency("x", "y");
            Assert.AreEqual(1, t1.Size);
            Assert.AreEqual(0, t2.Size);
        }
        

    }


}