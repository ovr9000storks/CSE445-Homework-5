using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Microsoft.ML;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
public class Service : IService
{
    public List<KeyValuePair<string, int>> calculateTop10(string input)
    {

        //extend the size allowed for transfer
        BasicHttpBinding binding = new BasicHttpBinding();
        binding.MaxReceivedMessageSize = 2 << 20;

        //create the context for the data
        var context = new MLContext();

        //create an empty data set
        var emptyData = new List<TextData>();

        //create the basis for the data
        var data = context.Data.LoadFromEnumerable(emptyData);

        //create the pipeline to tokenize the input
        var tokenization = context.Transforms.Text.TokenizeIntoWords("Tokens", "Text", separators: new[] { '-', '.', ',', '|' });

        //fit the tokenization data model
        var model = tokenization.Fit(data);

        //create the engine with the previously made context
        var engine = context.Model.CreatePredictionEngine<TextData, TextTokens>(model);

        //port the data into a TextData object type
        var text = engine.Predict(new TextData { Text = input });

        //count each word and set them in a list KeyValuePair of the word and the count
        List<KeyValuePair<string, int>> countedWords = countWords(text);

        //sort the list from most occurring to least
        var sortedWords = countedWords.OrderByDescending(pair => pair.Value).ToList();

        //remove all pairs beyond the top 10
        if(sortedWords.Count > 10)
        {
            sortedWords.RemoveRange(10, countedWords.Count-10);
        }


        return sortedWords;
    }

    List<KeyValuePair<string, int>> countWords(TextTokens textData)
    {
        List<KeyValuePair<string, int>> builtList = new List<KeyValuePair<string, int>>();

        foreach (var tokenI in textData.Tokens)
        {
            bool wordFound = false;
            foreach (var pair in builtList)
            {
                
                if(tokenI.Equals(pair.Key))
                {
                    wordFound = true;

                    //remove the old pair and save its values
                    string word = pair.Key;
                    int count = pair.Value;
                    builtList.Remove(pair);

                    //add the updated pair
                    builtList.Add(new KeyValuePair<string, int>(word, count + 1));

                    break;
                }
            }

            //if the word wasnt added before, add it and set the count to 1
            if (!wordFound)
            {
                builtList.Add(new KeyValuePair<string, int>(tokenI, 1));
            }
        }

        return builtList;
    }
}
