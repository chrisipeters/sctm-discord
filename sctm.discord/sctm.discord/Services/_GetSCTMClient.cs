﻿using Newtonsoft.Json;
using Serilog;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace sctm.discord
{
    public partial class Services
    {
        public async Task<HttpClient> GetSCTMClient()
        {
            if (await _GetSCTMToken() == null) return null;
            else
            {
                var _client = new HttpClient();
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                return _client;
            }
        }

        private async Task<string> _GetSCTMToken()
        {
            var _logAction = "Services_GetSCTMToken";


            var _loginUrl = _config["SCTM:Urls:Login"];
            var _email = _config["SCTM:Account:Email"]; ;
            var _password = _config["SCTM:Account:Password"];

            Log.Information($"Getting JWT Token from: {_loginUrl}");

            if (_token == null)
            {
                // no token
                var _loginModel = new SCTMLoginModel
                {
                    Email = _email,
                    Password = _password,
                };

                var _json = JsonConvert.SerializeObject(_loginModel);

                HttpContent c = new StringContent(_json, Encoding.UTF8, "application/json");

                var _client = new HttpClient();
                var _res = await _client.PostAsync(_loginUrl, c);
                if (_res.IsSuccessStatusCode)
                {
                    Log.Information($"Getting JWT Token - success");

                    var _result = JsonConvert.DeserializeObject<SCTMLoginResponse>(await _res.Content.ReadAsStringAsync());
                    _token = _result.JWT;
                    _tokenDate = DateTime.Now;
                }
                else
                {
                    Log.Error($"Getting JWT Token - Error");

                    _token = null;
                    _tokenDate = DateTime.MinValue;
                }

                _client.Dispose();

            }
            else if (_tokenDate < DateTime.Now.AddMinutes(-15))
            {
                // expired token
                _token = null;
                return await _GetSCTMToken();

            }
            else
            {
                // valid token
            }

            return _token;
        }
    }

    public class SCTMLoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class SCTMLoginResponse
    {
        public string JWT { get; set; }
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        public int Intel { get; set; }
        public int Prestige { get; set; }
        public string Experience { get; set; }
    }
}
