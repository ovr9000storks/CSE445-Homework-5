using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Microsoft.ML;


// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
[ServiceContract]
public interface IService
{

    [OperationContract]
    [WebGet(UriTemplate = "replaceString?x={input}&y={changeThis}&z={changeToThis}", ResponseFormat = WebMessageFormat.Json)]
    string replaceString(string input, string changeThis, string changeToThis);

}

[DataContract]
public class TextData
{
    public string Text { get; set; }
}

[DataContract]
public class TextTokens
{
    public string[] Tokens { get; set; }
}



