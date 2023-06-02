using System;
namespace CandleLightApi.Models
{
	public class openApi
	{
        public class OpenAiRequestBody
        {
            public string Model { get; set; }
            public List<Message> Messages { get; set; }
            public double Temperature { get; set; }
        }

        public class Message
        {
            public string Role { get; set; }
            public string Content { get; set; }
        }
    }
}

