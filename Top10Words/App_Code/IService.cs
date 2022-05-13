using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService" in both code and config file together.
[ServiceContract]
public interface IService
{

	[OperationContract]
	[WebGet(UriTemplate="top10Strings?x={input}", ResponseFormat=WebMessageFormat.Json)]
	List<KeyValuePair<string, int>> calculateTop10(string input);
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


