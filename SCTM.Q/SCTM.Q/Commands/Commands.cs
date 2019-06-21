using Discord.Commands;
using Newtonsoft.Json;
using SCTM.Q.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace SCTM.Q.Commands
{
    public partial class Commands : ModuleBase
    {
        private HttpClient _http;

        public Commands()
        {
            _http = new HttpClient();
            _http.BaseAddress = new Uri("https://localhost:44325");

            var _body = new LoginRequest
            {
                Email = "q@sctrademasters.com",
                Password = "jeaChr@1srevolt$4r"
            };
            var _bodyJson = JsonConvert.SerializeObject(_body);
            var _result = _http.PostAsync("auth/login", new StringContent(_bodyJson, Encoding.UTF8, "application/json")).Result;
            var _content = _result.Content.ReadAsStringAsync().Result;
            var _login = JsonConvert.DeserializeObject<LoginResult>(_content);
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _login.JWTId);
        }
    }
}
