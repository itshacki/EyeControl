using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeControl
{
    public class UserData
    {
        // user name field
        public string userName { get; set; }

        // user most common vocabulary. A dictionary contains user's words history, sorted by usage weight.
        public Dictionary<string, int> vocabulary { get; set; }

        // user log. Contains former phrases that had been written, with max capacity of _MAX_NUM_OF_LOG_LINES
        public string log { get; set; }

        public List<IClusterSection> userPages { get; set; }

        public static int _MAX_NUM_OF_LOG_LINES = 10;
        
    }

    // An abstaract user class. contains abstaract methods and members.
    public class User
    {
        private UserData data;

        public User()
        {
            data = new UserData
            {
                userName = "New User",
                vocabulary = new Dictionary<string, int>(),
                log = "",
                userPages = new List<IClusterSection>()
            };
        }

        public User(string fromFile)
        {
            data = Newtonsoft.Json.JsonConvert.DeserializeObject<UserData>(System.IO.File.ReadAllText(fromFile));
        }

        public void temp()
        {
            data.userPages.Add(new ClusterSection());
            data.userPages[0].north.SetElements("A", "B", "C", "D", "E", null);
            data.userPages[0].south.SetElements("G", "H", "I", "J", "K", null);
            data.userPages[0].west.SetElements("M", "N", "O", "P", "K", null);
            data.userPages[0].east.SetElements("R", "S", "T", "X", "Y", null);
            data.userPages[0].center.SetElements("W", "V", null, null, null, null);

            data.userPages[0].north.SetImgElements("null", "null", "null", "null", "null", "null");
            data.userPages[0].south.SetImgElements("null", "null", "null", "null", "null", "null");
            data.userPages[0].west.SetImgElements("null", "null", "null", "null", "null", "null");
            data.userPages[0].east.SetImgElements("null", "null", "null", "null", "null", "null");
            data.userPages[0].center.SetImgElements("null", "null", "Assets/hamburger.png", "Assets/back.png", "Assets/mute.png", "null");

            UpdateVocbulary("HELLO");
            UpdateVocbulary("HOME");
            UpdateVocbulary("HOME");
            UpdateVocbulary("ISAAC");
        }

        public void DumpUserData(string toFile)
        {
            System.IO.File.WriteAllText(toFile, Newtonsoft.Json.JsonConvert.SerializeObject(data));
        }

        public IClusterSection GetCurrentPage(int pageIndex)
        {
            //TODO: add try&catch
            return data.userPages[pageIndex];
        }

        /// <summary>
        /// function checks if a specific word is in the user's vocabulary
        /// </summary>
        /// <param name="word"> the word to check </param>
        /// <returns> true\false if word exists in vocabulary </returns>
        public bool IsWordInVocabulary(string word)
        {
            return data.vocabulary.ContainsKey(word);
        }

        /// <summary>
        /// Updates word in user's vocabulary. in case word already exists - its weight increases.
        /// </summary>
        /// <param name="word"> the word to update </param>
        public void UpdateVocbulary(string word)
        {
            if (this.IsWordInVocabulary(word)) {
               data.vocabulary[word]++;
            }
            else {
                data.vocabulary.Add(word, 1);
            }
        }

        /// <summary>
        /// Reduces user vocabulary by finding all user vocabulary keys starting with the prefix.
        /// </summary>
        /// <param name="prefix">the prefix to sort by. can be more thsn one char</param>
        /// <returns>A reduces vocabulary</returns>
        private Dictionary<string, int> ReduceVocabularyByPrefix(string prefix)
        {
            var tightVocabulary = data.vocabulary.Where(d => d.Key.StartsWith(prefix)).ToDictionary(d => d.Key, d => d.Value);
            return tightVocabulary;
        }

        public string GetWordComplete(string prefix)
        {
            Dictionary<string, int> reducedDict = ReduceVocabularyByPrefix(prefix);
            var sortedDict = from entry in reducedDict orderby entry.Value ascending select entry;
            if (sortedDict.Count() == 0)
                return "";
            return sortedDict.Last().Key;
        }
    }

    class UserScreen : IScreen
    {

        /// <summary>
        /// constructor
        /// </summary>

        private User loggedInUser = new User();
        private IClusterSection _clusterSection = new ClusterSection();
        private ILogSection _logSection = new LogSection();
        private ILineSection _lineSection = new LineSection();

        public ILogSection logSection { get { return this._logSection; } set { logSection = this._logSection; } }
        public ILineSection lineSection { get { return _lineSection; } set { lineSection = _lineSection; } }
        public IClusterSection clusterSection { get { return this._clusterSection; } set { clusterSection = this._clusterSection; } }
        
        public void test()
        {
            loggedInUser.temp();
            SetPage(0);
        }
        public void SetPage(int pageIndex)
        {
            IClusterSection currentPage = loggedInUser.GetCurrentPage(pageIndex);

            clusterSection.north.SetElements(currentPage.north);
            clusterSection.south.SetElements(currentPage.south);
            clusterSection.east.SetElements(currentPage.east);
            clusterSection.west.SetElements(currentPage.west);
            clusterSection.center.SetElements(currentPage.center);

            clusterSection.north.SetImgElements(currentPage.north);
            clusterSection.south.SetImgElements(currentPage.south);
            clusterSection.east.SetImgElements(currentPage.east);
            clusterSection.west.SetImgElements(currentPage.west);
            clusterSection.center.SetImgElements(currentPage.center);
        }

        public void HandleSpeakEvent()
        {
            loggedInUser.UpdateVocbulary(lineSection.line + lineSection.lineComplete);
            logSection.AddLineToLog(lineSection.line + lineSection.lineComplete);
            lineSection.EnterLine();
        }

        public void HandleClusterEvent(ICluster cluster)
        {
            if (cluster.single != null)
            {
                lineSection.AddElementToLine(cluster.single);
                lineSection.UpdateLineComplete(loggedInUser.GetWordComplete(lineSection.line));
                SetPage(0);
                return;
            }
            if (cluster.singleImg != "null")
            {
                SetPage(0);
                //TODO: set action
                return;
            }
            string north = null, east = null, west = null, south = null, center = null;
            string northImg = "null", eastImg = "null", westImg = "null", southImg = "null", centerImg = "null";
            if (cluster.up == null)
                northImg = cluster.upImg;
            else
                north = cluster.up;
            if (cluster.right == null)
                eastImg = cluster.rightImg;
            else
                east = cluster.right;
            if (cluster.left == null)
                westImg = cluster.leftImg;
            else
                west = cluster.left;
            if (cluster.down == null)
                southImg = cluster.downImg;
            else
                south = cluster.down;
            if (cluster.center == null)
                centerImg = cluster.centerImg;
            else
                center = cluster.center;
            clusterSection.east.SetElements(null, null, null, null, null, east);
            clusterSection.west.SetElements(null, null, null, null, null, west);
            clusterSection.south.SetElements(null, null, null, null, null, south);
            clusterSection.center.SetElements(null, null, null, null, null, center);
            clusterSection.north.SetElements(null, null, null, null, null, north);
            clusterSection.east.SetImgElements("null", "null", "null", "null", "null", eastImg);
            clusterSection.west.SetImgElements("null", "null", "null", "null", "null", westImg);
            clusterSection.south.SetImgElements("null", "null", "null", "null", "null", southImg);
            clusterSection.center.SetImgElements("null", "null", "null", "null", "null", centerImg);
            clusterSection.north.SetImgElements("null", "null", "null", "null", "null", northImg);
        }
        
    }

}
