using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace EditorsParadise
{
    public partial class Default : System.Web.UI.Page
    {
        bool validFileType = true;
        bool fileLoaded = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            

            HttpCookie cookie = Request.Cookies["myCookie"];
            if(cookie != null)
            {
                MainTextArea.InnerText = cookie["textArea"];
                LoadedFileLabel.Text = cookie["fileName"];
            }
        }

        protected void ResetTextButton_Click(object sender, EventArgs e)
        {
            if(SavedText.InnerText.Length > 0)
            {
                MainTextArea.InnerText = SavedText.InnerText;
                LoadedFileLabel.Text = SavedFileName.InnerText;
                Top10WordsArea.Visible = false;
            }
        }

        protected void ActionButton_Click(object sender, EventArgs e)
        {
            if(MainTextArea.InnerText != "")
            {
                if (SelectedOptionList.SelectedValue == "top10")
                {
                    Top10WordsArea.Visible = true;
                    outputTop10Strings(MainTextArea.InnerText);
                }
                else
                {
                    //hide the top 10 words div
                    Top10WordsArea.Visible = false;


                    if (SelectedOptionList.SelectedValue == "replace")
                    {
                        outputReplacedString(MainTextArea.InnerText, TextToReplace.Text, ReplacementText.Text);
                    }
                    else if (SelectedOptionList.SelectedValue == "filter")
                    {
                        outputFilteredString(MainTextArea.InnerText);
                    }
                    else
                    {
                        MainTextArea.InnerText = "Selection Error!\nReload the page or contact the service provider";
                    }
                }

                HttpCookie cookie = new HttpCookie("myCookie");
                cookie["textArea"] = MainTextArea.InnerText;
                cookie["fileName"] = LoadedFileLabel.Text;
                cookie.Expires = DateTime.Now.AddMinutes(2);
                Response.Cookies.Add(cookie);

            }





        }

        protected void TextUploadButton_Click(object sender, EventArgs e)
        {
            if (InputTextFileUpload.HasFile)
            {
                string filename = Server.HtmlEncode(InputTextFileUpload.FileName);
                string extension = System.IO.Path.GetExtension(filename);

                LoadedFileLabel.Text = filename;

                if(extension == ".txt")
                {
                    validFileType = true;
                    SavedText.InnerText = System.Text.Encoding.Default.GetString(InputTextFileUpload.FileBytes);
                    MainTextArea.InnerText = SavedText.InnerText;
                    fileLoaded = true;
                    SavedFileName.InnerText = filename;
                }
                else
                {
                    validFileType = false;
                    MainTextArea.InnerText = "File is not a plain text (.txt) file\nPlease upload a plain text file";
                }


            }
            else
            {
                MainTextArea.InnerText = "No file selected\nPlease select a file before performing an action";
                LoadedFileLabel.Text = "";
            }


            //Add the content of the main text area to the cookie list
            //Expires in 2 minutes
            HttpCookie cookie = new HttpCookie("myCookie");
            cookie["textArea"] = MainTextArea.InnerText;
            cookie["fileName"] = LoadedFileLabel.Text;
            cookie.Expires = DateTime.Now.AddMinutes(2);
            Response.Cookies.Add(cookie);

        }

        protected string webFriendlySpace(string input)
        {
            return input.Replace(" ", "|");
        }

        protected void outputTop10Strings(string input)
        {
            string x = "x=" + webFriendlySpace(input);
            string apiCall = @"http://localhost:62514/Service.svc/top10Strings?" + x;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiCall);
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            String rawJSON = reader.ReadToEnd();

            List<KeyValuePair<string, int>> top10List = JsonConvert.DeserializeObject<List<KeyValuePair<string, int>>>(rawJSON);

            foreach(var pair in top10List)
            {
                Top10WordsOut.InnerText = Top10WordsOut.InnerText + pair.Key + " (" + pair.Value + ")\n";
            }
        }

        protected void outputReplacedString(string input, string changeThis, string toThis)
        {
            string x = "x=" + webFriendlySpace(input);
            string y = "y=" + webFriendlySpace(changeThis);
            string z = "z=" + webFriendlySpace(toThis);
            string apiCall = @"http://localhost:62516/Service.svc/replaceString?" + x + "&" + y + "&" + z;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiCall);
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            String rawJSON = reader.ReadToEnd();

            MainTextArea.InnerText = JsonConvert.DeserializeObject<string>(rawJSON);
        }

        protected void outputFilteredString(string input)
        {
            string x = "x=" + webFriendlySpace(input);
            string apiCall = @"http://localhost:62517/Service.svc/removeStops?" + x;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiCall);
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            String rawJSON = reader.ReadToEnd();

            MainTextArea.InnerText = JsonConvert.DeserializeObject<string>(rawJSON);
        }
    }
}