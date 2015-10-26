using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeControl
{
    /// <summary>
    /// Defines a cluster of the cluster screen section
    /// </summary>
    public interface ICluster : INotifyPropertyChanged
    {
        // cluster string elements
        string up { get; set; }
        string down { get; set; }
        string left { get; set; }
        string right { get; set; }
        string center { get; set; }
        string single { get; set; }

        // cluster image elements
        string upImg { get; set; }
        string downImg { get; set; }
        string leftImg { get; set; }
        string rightImg { get; set; }
        string centerImg { get; set; }
        string singleImg { get; set; }

        /// <summary>
        /// set cluster elements
        /// </summary>
        void SetElements(string up, string down, string left, string right, string center, string single);
        void SetElements(ICluster clusterToSet);
        /// <summary>
        /// set cluster image elements
        /// </summary>
        void SetImgElements(string upImg, string downImg, string leftImg, string rightImg, string centerImg, string singleImg);
        void SetImgElements(ICluster clusterToSet);
        /// <summary>
        /// Clears clusterElements list
        /// </summary>
        void ClearElements();
    }

    /// <summary>
    /// Defines the clusters view of the cluster screen section
    /// </summary>
    public interface IClusterSection
    {
        // refers for the northern cluster object
        ICluster north { get; set; }

        // refers for the western cluster object
        ICluster west { get; set; }

        // refers for the eastern cluster object
        ICluster east { get; set; }

        // refers for the southern cluster object
        ICluster south { get; set; }

        // refers for the center cluster object
        ICluster center { get; set; }

        /// <summary>
        /// Clears all current clusters
        /// </summary>
        void ClearAllClusters();
    }

    /// <summary>
    /// Defines the log view of the log screen section
    /// </summary>
    public interface ILogSection : INotifyPropertyChanged
    {
        // the log that will apear at the log screen section
        string logLines { get; set; }

        void AddLineToLog(string line);
    }

    /// <summary>
    /// Defines the "to-speak" line view of the log screen section
    /// </summary>
    public interface ILineSection : INotifyPropertyChanged
    {
        // the line that will apear at the line screen section
        string line { get; set; }

        // the phrase auto-complete line
        string lineComplete { get; set; }

        /// <summary>
        /// Enters current line-view (includes the line-complete) to user dictionary and log
        /// </summary>
        void EnterLine();

        /// <summary>
        /// adds a letter or word to the line-view
        /// NOTE: A white-space means accept auto-complete.
        /// </summary>
        /// <param name="element">A char or string type element</param>
        void AddElementToLine(string element);

        void UpdateLineComplete(string wordComplete);
    }

    /// <summary>
    /// Defines the main screen view
    /// </summary>
    public interface IScreen
    {
        ILogSection logSection { get; set; }
        ILineSection lineSection { get; set; }
        IClusterSection clusterSection { get; set; }

        void SetPage(int pageIndex);

        void HandleClusterEvent(ICluster cluster);

        void HandleSpeakEvent();

        void test();
    }
}
