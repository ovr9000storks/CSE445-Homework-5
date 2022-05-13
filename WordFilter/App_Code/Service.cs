using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Microsoft.ML;


    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service : IService
    {
        //heavily relies on the open-source Microsoft Machine Learning API
        public string removeStopWords(string input)
        {
            //extend the size allowed for transfer
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = 2 << 20;

            //create the context for the data
            var context = new MLContext();

            //create an empty data set
            var emptyData = new List<TextData>();

            //create the data
            var data = context.Data.LoadFromEnumerable(emptyData);

            //create the parameters for tokenization
            //remove the stop words
            //return the tokens of modified input
            var tokenization = context.Transforms.Text.TokenizeIntoWords("Tokens", "Text", separators: new[] { ' ', '.', ',', '|' }).Append(context.Transforms.Text.RemoveDefaultStopWords("Tokens", "Tokens", Microsoft.ML.Transforms.Text.StopWordsRemovingEstimator.Language.English));

            //create the data model
            var model = tokenization.Fit(data);

            //create the engine with the previously made context
            var engine = context.Model.CreatePredictionEngine<TextData, TextTokens>(model);

            //create the text with the stop words removed
            var text = engine.Predict(new TextData { Text = input });


            return CreateOutput(text);
        }

        public void setMaxMessageSize()
        {
            BasicHttpBinding binding = new BasicHttpBinding();

            binding.MaxReceivedMessageSize = 2 << 20;
        }

        private string CreateOutput(TextTokens tokens)
        {
            var builder = new StringBuilder();

            //adds the tokens and a space to the string being built 
            foreach (var token in tokens.Tokens)
            {
                builder.Append(token);
                builder.Append(" ");
            }

            return builder.ToString();
        }
    }

