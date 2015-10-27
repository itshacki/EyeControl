using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

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
            var constants = new Constants();
            data.userPages.Add(new ClusterSection());
            data.userPages[0].north.SetElements("A", "B", "C", "D", "E", null);
            data.userPages[0].south.SetElements("G", "H", "I", "J", "K", null);
            data.userPages[0].west.SetElements("M", "N", "O", "P", "K", null);
            data.userPages[0].east.SetElements("R", "S", "T", "X", "Y", null);
            data.userPages[0].center.SetElements(null, null, null, null, null, null);

            data.userPages[0].north.SetImgElements(null, null, null, null, null, null);
            data.userPages[0].south.SetImgElements(null, null, null, null, null, null);
            data.userPages[0].west.SetImgElements(null, null, null, null, null, null);
            data.userPages[0].east.SetImgElements(null, null, null, null, null, null);
            data.userPages[0].center.SetImgElements(constants.simbols["space"], constants.simbols["backSpace"], constants.simbols["hamburger"], constants.simbols["nextPage"],constants.simbols["speak"], null);

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
            if (prefix == "")
            {
                return "";
            }
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

        // The object for controlling the speech synthesis engine (voice).
        private SpeechSynthesizer synth = new SpeechSynthesizer();

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

        public async void HandleSpeakEvent(MediaElement mediaElement)
        {
            string currentLine = lineSection.line + lineSection.lineComplete;
            if (currentLine == "")
                return;
            SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync(currentLine);
            // if the SSML stream is not in the correct format throw an error message to the user
            if (stream == null)
            {
                MessageDialog dialog = new MessageDialog("unable to synthesize text");
                await dialog.ShowAsync();
                return;
            }
            // start this audio stream playing
            mediaElement.AutoPlay = true;
            mediaElement.SetSource(stream, stream.ContentType);
            mediaElement.Play();
            foreach (string word in currentLine.Split(' ')) loggedInUser.UpdateVocbulary(word);
            logSection.AddLineToLog(currentLine);
            lineSection.EnterLine();
        }

        public void HandleSpaceEvent()
        {
            lineSection.line = lineSection.line + " ";
            string lastWord = lineSection.line.Split(' ').Last();
            lineSection.UpdateLineComplete(loggedInUser.GetWordComplete(lastWord));
        }

        public void HandleBackspaceEvent()
        {
            if (lineSection.line.Length > 0)
            {
                lineSection.line = lineSection.line.Remove(lineSection.line.Length - 1);
                string lastWord = lineSection.line.Split(' ').Last();
                lineSection.UpdateLineComplete(loggedInUser.GetWordComplete(lastWord));
            }
        }

        public string HandleClusterEvent(ICluster cluster)
        {
            string UIrequest = null;
            if (cluster.single != null)
            {
                lineSection.AddElementToLine(cluster.single);
                string lastWord = lineSection.line.Split(' ').Last();
                lineSection.UpdateLineComplete(loggedInUser.GetWordComplete(lastWord));
                SetPage(0);
            }
            else if (cluster.singleImg != null)
            {
                UIrequest = cluster.singleImg;
                SetPage(0);
            }
            else
            {
                string north = null, east = null, west = null, south = null, center = null;
                string northImg = null, eastImg = null, westImg = null, southImg = null, centerImg = null;
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
                clusterSection.east.SetImgElements(null, null, null, null, null, eastImg);
                clusterSection.west.SetImgElements(null, null, null, null, null, westImg);
                clusterSection.south.SetImgElements(null, null, null, null, null, southImg);
                clusterSection.center.SetImgElements(null, null, null, null, null, centerImg);
                clusterSection.north.SetImgElements(null, null, null, null, null, northImg);
            }
            return UIrequest;
        }
        
    }

}
