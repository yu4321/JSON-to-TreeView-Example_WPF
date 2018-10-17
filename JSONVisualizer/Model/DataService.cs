using System;

namespace JSONVisualizer.Model
{
    public class DataService : IDataService
    {
        //static string targeturl = "http://ws.bus.go.kr/api/rest/stationinfo/getStationByUid?serviceKey=K9rX8pWW%2BSURqwb7bC2iIJCWzp0WrOajRo%2BUxzpFnedxZ9Jf9xRcrWxD%2BaCCRv11nHGDBvLtVSesxyphAgt9UA%3D%3D&arsId=23804";

        public void GetData(Action<DataItem, Exception> callback)
        {
            // Use this to connect to the actual data service

            var item = new DataItem("Welcome to MVVM Light");
            callback(item, null);
        }
    }
}