// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta
// (Clarified meaning of dependent and dependee.)
// (Clarified names in solution/project structure.)
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
namespace SpreadsheetUtilities
{
    /// <summary>
    /// Author: Andrew Winward
    /// Partner: -none-
    /// Date: 1/27/24
    /// Course: CS 3500
    /// 
    /// This library class is for a Dependency Graph that uses the dependencies between
    /// nodes. It allows users to add, remove, and show the input dependencies.
    /// </summary>
    
    
    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    ///
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings. Two
    /// ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1
    /// equals t2.
    /// 
    /// Recall that sets never contain duplicates. If an attempt is made to add an
    /// element to a set, and the element is already in the set, the set remains unchanged.
    ///
    /// Given a DependencyGraph DG:
    ///
    /// (1) If s is a string, the set of all strings t such that (s,t) is in DG is
    /// called dependents(s).
    /// (The set of things that depend on s)
    ///
    /// (2) If s is a string, the set of all strings t such that (t,s) is in DG is
    /// called dependees(s).
    /// (The set of things that s depends on)
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    // dependents("a") = {"b", "c"}
    // dependents("b") = {"d"}
    // dependents("c") = {}
    // dependents("d") = {"d"}
    // dependees("a") = {}
    // dependees("b") = {"a"}
    // dependees("c") = {"a"}
    // dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        private Dictionary<string, HashSet<string>> dependentGraph;
        private Dictionary<string, HashSet<string>> dependeeGraph;
        private int size = 0;
       
        /// <summary>
        /// Creates two empty DependencyGraphs one for dependents and dependees
        /// </summary>
        public DependencyGraph()
        {
            dependentGraph = new Dictionary<string, HashSet<string>>();
            dependeeGraph = new Dictionary<string, HashSet<string>>();

        }
       
        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        /// <param> graph</param>
        /// <returns>returns the amount of ordered pairs in the DP graph </returns>>
        public int Size => size; 
       
        
        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer. If dg is a DependencyGraph, you
        ///would invoke it like this: dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        /// <param name="s"></param>
        /// <return>size of dependees</return>
        public int this[string s]  
        {   
            get {
                if (string.IsNullOrEmpty(s) || HasDependees(s) == true) //reference #6 in README
                {
                    return dependeeGraph[s].Count;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>true or false if it has a dependent</returns>
        public bool HasDependents(string s)
        {
            //if it is has the key send true
            if (dependentGraph.ContainsKey(s))
            {
                HashSet<string> dependents = dependentGraph[s];
                return dependents != null && dependents.Count > 0;
            }

            return false;
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>true or false if it has a dependee</returns>
        public bool HasDependees(string s) { 
            
            //if it has dependees send true
        if (dependeeGraph.ContainsKey(s))
            {
                HashSet<string> dependees = dependeeGraph[s];
                return dependees != null && dependees.Count > 0;
            }

            return false;
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        /// <param name="s"></param>
        /// <returns> the enumerated dependent(s)</returns>
        public IEnumerable<string> GetDependents(string s)
        {

            if (dependentGraph.ContainsKey(s))
                return dependentGraph[s];
            else
                return Enumerable.Empty<string>();
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        /// <param name="s"></param>
        /// <returns>the enumerated dependees</returns>
        public IEnumerable<string> GetDependees(string s)
        {
            if (dependeeGraph.ContainsKey(s))
                return dependeeGraph[s];
            else
                return Enumerable.Empty<string>();
        }

        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        ///
        /// <para>This should be thought of as:</para>
        ///
        /// t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param> ///
        public void AddDependency(string s, string t)
        {
            //if either s or t is empty or null throw exception
            if (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(t))//reference #6 in README
            {
                throw new ArgumentException("Cannot be null or empty");
            }

            bool depAdded = false;
            if (!dependentGraph.ContainsKey(s))
            {
                dependentGraph[s] = new HashSet<string>();
            }
            if (dependentGraph[s].Add(t))
            {
                // If a dependent was added, set to true
               depAdded = true;
            }

           
          
            // if the graph did not contain t, make a new hashset
            if (!dependeeGraph.ContainsKey(t))
            {
                dependeeGraph[t] = new HashSet<string>();
            }
              //dependee is added to t
            if(dependeeGraph[t].Add(s) && depAdded)
            {
                // If a dependee and dependent were added successfully, increment the size by 1
                size++;
            }
        }

        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            //if either s or t is empty or null throw exception
            if (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(t))//reference #6 in README
            {
                throw new ArgumentException("Cannot be null or empty");
            }


            bool depRemoved = false;
            if (dependentGraph.ContainsKey(s))
            {

                if (dependentGraph[s].Remove(t))
                {
                    depRemoved = true;

                    if (dependentGraph[s].Count == 0)
                    {
                        dependentGraph.Remove(s);
                    }
                }
            }
            
            if (dependeeGraph.ContainsKey(t))
            {
                if (dependeeGraph[t].Remove(s))
                {
                    depRemoved = true;

                    if (dependeeGraph[t].Count == 0)
                    {
                        dependeeGraph.Remove(t);
                    }

                  
                }
            }
            if (depRemoved)
            {
                size--;
            }
        }

             /// <summary>
             /// Removes all existing ordered pairs of the form (s,r). Then, for each
             /// t in newDependents, adds the ordered pair (s,t).
             /// </summary>
             public void ReplaceDependents(string s, IEnumerable<string> newDependents)
             {
                if (dependentGraph.ContainsKey(s))
                {
                //removes current dependents from s
                foreach (string dependent in dependentGraph[s].ToList())
                {
                    RemoveDependency(s,dependent);
                }
                    }
                //adds the newDependents into s
                foreach (string dependent in newDependents)
                    {
                AddDependency(s, dependent);
                 }
             }
        
            /// <summary>
            /// Removes all existing ordered pairs of the form (r,s). Then, for each
            /// t in newDependees, adds the ordered pair (t,s).
            /// </summary>
            public void ReplaceDependees(string s, IEnumerable<string> newDependees)
            {
                if (dependeeGraph.ContainsKey(s))
                {
                //removes current dependees from s
                foreach (string dependee in dependeeGraph[s].ToList())
                {
                    RemoveDependency(dependee, s);
                }
            }
            //adds the newDependees into s
            foreach (string dependee in newDependees)
            {
                AddDependency(dependee, s);
            }
        }
    }
}