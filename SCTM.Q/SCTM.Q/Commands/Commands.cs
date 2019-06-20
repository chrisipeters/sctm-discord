using Discord.Commands;
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
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJraWQiOiJrcjVsTE1VeEpTMWZ2V0JWXC9mRnExRHYxRytsNFdDTGZMcFprS2F5dHlCaz0iLCJhbGciOiJSUzI1NiJ9.eyJzdWIiOiJjY2UzZDFmMy1lMDA0LTQ2MzctOWU1NC1jOTVlYTZjMTMyNzkiLCJjb2duaXRvOmdyb3VwcyI6WyJzY3RtLWRpc2NvcmQtbWVtYmVycy1tb2RpZnkiXSwiZW1haWxfdmVyaWZpZWQiOnRydWUsImlzcyI6Imh0dHBzOlwvXC9jb2duaXRvLWlkcC5hcC1zb3V0aGVhc3QtMi5hbWF6b25hd3MuY29tXC9hcC1zb3V0aGVhc3QtMl9WblN0Z2VnQUsiLCJjb2duaXRvOnVzZXJuYW1lIjoiY2NlM2QxZjMtZTAwNC00NjM3LTllNTQtYzk1ZWE2YzEzMjc5IiwiY3VzdG9tOnJzaUhhbmRsZXMiOiJjaHJpc3B5a29hbGEiLCJhdWQiOiIzdWZ2dnNlbmZubWc4M2VoZWN2cTh1cGJqOSIsImV2ZW50X2lkIjoiZDcwZjZlNjItMTk4OS00MzY0LTk4ZTItMzM0MTNmNmY5YjQ0IiwidG9rZW5fdXNlIjoiaWQiLCJhdXRoX3RpbWUiOjE1NjEwMzk3NDQsImV4cCI6MTU2MTA0MzM0NCwiaWF0IjoxNTYxMDM5NzQ0LCJlbWFpbCI6ImNocmlzcHlrb2FsYS5jb3JwQGdtYWlsLmNvbSJ9.OYRgvmgyfSPjBJrjGJiPZH8vBXiOIJZsvcWQGwlFdEgBBUaw_PQQGN9OWFMZ3sHwpx5zu0-QMb0Bvs5SJMTDZsuERgr9ProyUP9L2AbaQWkIr0qgNzgVV_TwjeSkJC4bqoYGT57R6tGsr-SuvBUszfPlnanrrcX3aPA5H9jtR8nlcTis0CdDCQtTJdEpLPvu1Bwjf4VAz0hyn0DTtfkzh-Mv_kxjnY3Ue-22M5RhLQTDwbHqlos2PrdXulPR8kGu8DAnExgVQTKLH8OPdK8AzNr8KCEBc3du4vH7wGEsiE4PLXzuHJ1hExE7TFTPQRQI_0Toeq6-cXAQIbI0u7t0-A");
        }
    }
}
