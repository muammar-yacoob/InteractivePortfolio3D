using System;
using System.Collections.Generic;

namespace SparkGames.Portfolio3D
{
    [Serializable]
    public class CVDataModel
    {
        public string Name;
        public string JobTitle;
        public string WorkExperience;
        public string Education;
        public List<Project> Projects;
    }

    [Serializable]
    public class Project
    {
        public string Title;
        public string Description;
        public string Icon;
        public string SFX;
        public string URL;
    }
}