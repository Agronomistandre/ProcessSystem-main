using System.Collections.Generic;
using Common.DB;
using Newtonsoft.Json;

namespace ProcessSystem.Contracts
{
    public class Register : IAggregateRoot
    {
        public long Id { get; private set; }
        public string Token { get; private set; }
        public string Url { get; private set; }
        public string Name { get; private set; }
        public string ProcessTypes { get; private set; }

        public Register(string token, string url, string name)
        {
            Token = token;
            Url = url;
            Name = name;
        }

        public void SetEventTypes(IList<string> eventTypesList) => ProcessTypes = JsonConvert.SerializeObject(eventTypesList);
    }
}