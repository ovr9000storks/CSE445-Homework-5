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
    public string replaceString(string input, string changeThis, string changeToThis)
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


        return CreateOutput(text, changeThis, changeToThis);
    }

    private string CreateOutput(TextTokens tokens, string changeThis, string changeToThis)
    {
        StringBuilder builder = new StringBuilder();

        //Replace any token that is equal to the changeThis paremeter
        foreach (var tokenI in tokens.Tokens)
        {
            if (tokenI.Equals(changeThis))
            {
                builder.Append(changeToThis);
            }
            else
            {
                builder.Append(tokenI);
            }
            builder.Append(" ");
        }

        return builder.ToString();
    }
}

