using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EyeControl
{
    class LogSection : ILogSection
    {
        private List<string> lines;
        
        List<string> ILogSection.lines { get { return lines; } set { lines = value; } }
        
        public void AddLogLine(string line)
        {
            if (lines.Count >=5)
            {
                lines.RemoveAt(5);
            }
            lines.Insert(0, line);
        }
    }

    class LineSection : ILineSection
    {
        private string _line = "";
        private string _lineComplete = "";

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public string line { get { return _line; } set { _line = value; NotifyPropertyChanged(); } }
        public string lineComplete { get { return _lineComplete; } set { _lineComplete = value; NotifyPropertyChanged(); } }

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                // Raise the PropertyChanged event, passing the name of the property whose value has changed.
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        void ILineSection.EnterLine()
        {

            line = "";
            lineComplete = "";
        }
       
        void ILineSection.AddElementToLine(string element)
        {
            line = line + element;
        }

        public void UpdateLineComplete(string wordComplete)
        {
            lineComplete = wordComplete;
        }
    }

    class Cluster : ICluster
    {
        // Defining private properties
        private string _up;
        private string _down;
        private string _left;
        private string _right;
        private string _center;
        private string _single;
        private string _upImg;
        private string _downImg;
        private string _leftImg;
        private string _rightImg;
        private string _centerImg;
        private string _singleImg;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// blank constructor for cluster's objects
        /// </summary>
        public Cluster()
        {
            _up = null;
            _down = null;
            _left = null;
            _right = null;
            _center = null;
            _single = null;
            _upImg = "null";
            _downImg = "null";
            _leftImg = "null";
            _rightImg = "null";
            _centerImg = "null";
            _singleImg = "null";
    }

        /// <summary>
        /// constructor for single cluster object
        /// </summary>
        /// <param name="single"> single string</param>
        /// <param name="down"> down string</param>
        /// <param name="left"> left string</param>
        /// <param name="right"> right string</param>
        /// <param name="center"> center string</param>
        public Cluster(string single)
        {
            _up = null;
            _down = null;
            _left = null;
            _right = null;
            _center = null;
            _single = single;
        }

        /// <summary>
        /// constructor for five cluster's objects
        /// </summary>
        /// <param name="up"> up string</param>
        /// <param name="down"> down string</param>
        /// <param name="left"> left string</param>
        /// <param name="right"> right string</param>
        /// <param name="center"> center string</param>
        public Cluster(string up, string down, string left, string right, string center)
        {
            _up = up;
            _down = down;
            _left = left;
            _right = right;
            _center = center;
            _single = null;
        }

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                // Raise the PropertyChanged event, passing the name of the property whose value has changed.
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // Interface properties implementation
        public string up { get { return _up; } set { _up = value; NotifyPropertyChanged(); } }
        public string down { get { return _down; } set { _down = value; NotifyPropertyChanged(); } }
        public string left { get { return _left; } set { _left = value; NotifyPropertyChanged(); } }
        public string right { get { return _right; } set { _right = value; NotifyPropertyChanged(); } }
        public string center { get { return _center; } set { _center = value; NotifyPropertyChanged(); } }
        public string single { get { return _single; } set { _single = value; NotifyPropertyChanged(); } }
        public string upImg { get { return _upImg; } set { _upImg = value; NotifyPropertyChanged(); } }
        public string downImg { get { return _downImg; } set { _downImg = value; NotifyPropertyChanged(); } }
        public string leftImg { get { return _leftImg; } set { _leftImg = value; NotifyPropertyChanged(); } }
        public string rightImg { get { return _rightImg; } set { _rightImg = value; NotifyPropertyChanged(); } }
        public string centerImg { get { return _centerImg; } set { _centerImg = value; NotifyPropertyChanged(); } }
        public string singleImg { get { return _singleImg; } set { _singleImg = value; NotifyPropertyChanged(); } }
        // Interface method implementation
        void ICluster.ClearElements()
        {
            _up = null;
            _down = null;
            _left = null;
            _right = null;
            _center = null;
            _single = null;
        }

        // Interface method implementation
        public void SetElements(string up, string down, string left, string right, string center, string single)
        {
            this.up = up;
            this.down = down;
            this.left = left;
            this.right = right;
            this.center = center;
            this.single = single;
        }

        public void SetElements(ICluster clusterToSet)
        {
            this.up = clusterToSet.up;
            this.down = clusterToSet.down;
            this.left = clusterToSet.left;
            this.right = clusterToSet.right;
            this.center = clusterToSet.center;
            this.single = clusterToSet.single;
        }

        public void SetImgElements(string upImg, string downImg, string leftImg, string rightImg, string centerImg, string singleImg)
        {
            this.upImg = upImg;
            this.downImg = downImg;
            this.leftImg = leftImg;
            this.rightImg = rightImg;
            this.centerImg = centerImg;
            this.singleImg = singleImg;
        }

        public void SetImgElements(ICluster clusterToSet)
        {
            this.upImg = clusterToSet.upImg;
            this.downImg = clusterToSet.downImg;
            this.leftImg = clusterToSet.leftImg;
            this.rightImg = clusterToSet.rightImg;
            this.centerImg = clusterToSet.centerImg;
            this.singleImg = clusterToSet.singleImg;
        }
    }

    class ClusterSection : IClusterSection
    {
        // Northern cluster property implementation
        private ICluster _north = new Cluster();
        ICluster IClusterSection.north { get { return _north; } set { _north = value; } }

        // Western cluster property implementation
        private ICluster _west = new Cluster();
        ICluster IClusterSection.west { get { return _west; } set { _west = value; } }

        // Eastern cluster property implementation
        private ICluster _east = new Cluster();
        ICluster IClusterSection.east { get { return _east; } set { _east = value; } }

        // Southern cluster property implementation
        private ICluster _south = new Cluster();
        ICluster IClusterSection.south { get { return _south; } set { _south = value; } }

        // Centern cluster property implementation
        private ICluster _center = new Cluster();
        ICluster IClusterSection.center { get { return _center; } set { _center = value; } }

        // clear clusters implementation
        void IClusterSection.ClearAllClusters()
        {
            _north.ClearElements();
            _west.ClearElements();
            _east.ClearElements();
            _south.ClearElements();
            _center.ClearElements();
        }
    }
    
}
