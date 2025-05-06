using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
///Author: Andrew Winward
///Teacher: Professor Cowder
///Date: 7/1/24
namespace StargateMath
{
    /// <summary>
    /// This class keeps track of the users information such as Name, and Age. 
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// Keeps track of the user's name
        /// </summary>
        private string name;

        /// <summary>
        /// Keeps track of the user's age
        /// </summary>
        private int age;

        /// <summary>
        /// Constructor for the user info class
        /// </summary>
        /// <param name="name"></param>
        /// <param name="age"></param>
        public UserInfo(string name, int age)
        {
            try
            {
                this.name = name;
                this.age = age;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in creating user info: {ex.Message}");
            }
        }

        /// <summary>
        /// Getter and Setter method for age
        /// </summary>
        public int Age
        {
            get { return age; }
            set
            {
                try
                {
                    age = value;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error, could not set age: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Getter and Setter method for name
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                try
                {
                    name = value;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error, could not set name: {ex.Message}");
                }
            }
        }
    }
}
