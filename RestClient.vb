Imports System.Collections.Specialized
Imports System.Net
Imports System.Net.Http
Imports System.Text
Imports Newtonsoft.Json

Public Class RestClient
	Private Const endpoint = "https://rest.payamak-panel.com/api/SendSMS/"
	Private Const sendOp = "SendSMS"
	Private Const getDeliveryOp = "GetDeliveries2"
	Private Const getMessagesOp = "GetMessages"
	Private Const getCreditOp = "GetCredit"
	Private Const getBasePriceOp = "GetBasePrice"
	Private Const getUserNumbersOp = "GetUserNumbers"
	Private Const sendByBaseNumberOp = "BaseServiceNumber"
	Private UserName As String
	Private Password As String

	Public Sub New(ByVal username As String, ByVal password As String)
		username = username
		password = password
	End Sub

	Private Function makeRequest(ByVal values As Dictionary(Of String, String), ByVal op As String) As RestResponse
		Using client = New WebClient
			Dim response = client.UploadValues(endpoint & op, values.Aggregate(New NameValueCollection, Function(seed, current)
																											seed.Add(current.Key, current.Value)
																											Return seed
																										End Function))
			Return JsonConvert.DeserializeObject(Of RestResponse)(Encoding.Default.GetString(response))
		End Using
	End Function

	Public Function Send(ByVal [to] As String, ByVal from As String, ByVal message As String, ByVal isflash As Boolean) As RestResponse
		Dim values = New Dictionary(Of String, String) From {
			{"username", UserName},
			{"password", Password},
			{"to", [to]},
			{"from", from},
			{"text", message},
			{"isFlash", isflash.ToString}
		}
		Return makeRequest(values, sendOp)
	End Function

	Public Function SendByBaseNumber(ByVal text As String, ByVal [to] As String, ByVal bodyId As Integer) As RestResponse
		Dim values = New Dictionary(Of String, String) From {
			{"username", UserName},
			{"password", Password},
			{"text", text},
			{"to", [to]},
			{"bodyId", bodyId.ToString}
		}
		Return makeRequest(values, sendByBaseNumberOp)
	End Function

	Public Function GetDelivery(ByVal recid As Long) As RestResponse
		Dim values = New Dictionary(Of String, String) From {
			{"UserName", UserName},
			{"PassWord", Password},
			{"recID", recid.ToString}
		}
		Return makeRequest(values, getDeliveryOp)
	End Function

	Public Function GetMessages(ByVal location As Integer, ByVal from As String, ByVal index As String, ByVal count As Integer) As RestResponse
		Dim values = New Dictionary(Of String, String) From {
			{"UserName", UserName},
			{"PassWord", Password},
			{"Location", location.ToString},
			{"From", from},
			{"Index", index},
			{"Count", count.ToString}
		}
		Return makeRequest(values, getMessagesOp)
	End Function

	Public Function GetCredit() As RestResponse
		Dim values = New Dictionary(Of String, String) From {
			{"UserName", UserName},
			{"PassWord", Password}
		}
		Return makeRequest(values, getCreditOp)
	End Function

	Public Function GetBasePrice() As RestResponse
		Dim values = New Dictionary(Of String, String) From {
			{"UserName", UserName},
			{"PassWord", Password}
		}
		Return makeRequest(values, getBasePriceOp)
	End Function

	Public Function GetUserNumbers() As RestResponse
		Dim values = New Dictionary(Of String, String) From {
			{"UserName", UserName},
			{"PassWord", Password}
		}
		Return makeRequest(values, getUserNumbersOp)
	End Function
End Class

Public Class RestResponse

	Public Property Value As String

	Public Property RetStatus As Integer

	Public Property StrRetStatus As String
End Class
