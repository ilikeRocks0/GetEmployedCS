using System;
using System.Threading.Tasks;
using NBomber.Http.CSharp;
using NBomber.CSharp;
using System.Text;
using System.Text.Json;
using NBomber.Contracts;
using System.Text.Json.Nodes;

namespace MyLoadTest
{
    class Program
    {
        static void Main(string[] args)
        {
            NBomberRunner
                .RegisterScenarios(
                    AccessWebsiteTest(), 
                    LogInLogOutTest(),
                    SpamGenericWord(),
                    SpamQuizGame(),
                    SpamGetJobs(), 
                    SpamGetSavedJobs(),
                    SpamSaveUnsaveJob())
                .Run();
            
        }  

        static async Task<HttpResponseMessage> Login(HttpClient client)
        {
            using StringContent jsonContent = new(
            JsonSerializer.Serialize(new
            {
                Email = "testuser1@loadtest.com",
                Password = "password" 
            }),
            Encoding.UTF8,
            "application/json");

            return await client.PostAsync("https://localhost/api/users/login",jsonContent);
        }
        static async Task<HttpResponseMessage> LogOut(HttpClient client)
        {
            using StringContent jsonContent = new StringContent("");
            return await client.PostAsync("https://localhost/api/users/logout", jsonContent);
        }

        static ScenarioProps AccessWebsiteTest()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient client = new HttpClient(clientHandler);

            var scenario = Scenario.Create("Accessing website", async context =>
            {
                var response = await client.GetAsync("https://localhost");

                return response.IsSuccessStatusCode
                    ? Response.Ok()
                    : Response.Fail();
            })
            .WithoutWarmUp()
            .WithLoadSimulations(
                Simulation.Inject(rate: 10,
                                  interval: TimeSpan.FromSeconds(1),
                                  during: TimeSpan.FromSeconds(30))
            );

            return scenario;
        }

        static ScenarioProps LogInLogOutTest()
        {
            var scenario = Scenario.Create("quick Login Logout", async context =>
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                HttpClient client = new HttpClient(clientHandler);

                var step1 = await Step.Run("login", context, async () =>
                {
                    var response = await Login(client);
                    return response.IsSuccessStatusCode
                    ? Response.Ok()
                    : Response.Fail();
                });

                var step2 = await Step.Run("logout", context, async () =>
                {
                    var response = await LogOut(client);

                    return response.IsSuccessStatusCode
                    ? Response.Ok()
                    : Response.Fail();
                });

                return Response.Ok();        
            });

            return scenario;
        }

        static ScenarioProps SpamGenericWord()
        {
            var scenario = Scenario.Create("Spam Generic Word", async context =>
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                HttpClient client = new HttpClient(clientHandler);

                var step1 = await Step.Run("login", context, async () =>
                {
                    var response = await Login(client);
                    return response.IsSuccessStatusCode
                    ? Response.Ok()
                    : Response.Fail();
                });

                var step2 = await Step.Run("read sentence",context, async () =>
                {
                    using StringContent jsonContent = new(
                        JsonSerializer.Serialize(new
                        {
                            genericWord = "I am a hardworking and Motivated Individual who has proven to be excellent at getting results"
                        }),
                        Encoding.UTF8,
                        "application/json");

                        var response = await client.PostAsync("https://localhost/api/genericWord", jsonContent);
                        return response.IsSuccessStatusCode
                            ? Response.Ok()
                            : Response.Fail();
                });

                var step3 = await Step.Run("logout", context, async () =>
                {
                    var response = await LogOut(client);

                    return response.IsSuccessStatusCode
                    ? Response.Ok()
                    : Response.Fail();
                });

                return Response.Ok();        
            });

            return scenario;
        }

        static ScenarioProps SpamQuizGame()
        {
            var scenario = Scenario.Create("Spam Quiz Game", async context =>
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                HttpClient client = new HttpClient(clientHandler);

                var step1 = await Step.Run("login", context, async () =>
                {
                    var response = await Login(client);
                    return response.IsSuccessStatusCode
                    ? Response.Ok()
                    : Response.Fail();
                });

                var step2 = await Step.Run("initialize quiz game",context, async () =>
                {
                    using StringContent jsonContent = new StringContent("");
                    var response = await client.PostAsync("https://localhost/api/quiz/game", jsonContent);
                    return response.IsSuccessStatusCode
                        ? Response.Ok()
                        : Response.Fail();
                });

                var step3 = await Step.Run("spam quiz game",context, async () =>
                {

                    using StringContent jsonContentEmpty = new StringContent("");
                    var response = await client.PostAsync("https://localhost/api/quiz/next", jsonContentEmpty);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var jsonNode = JsonNode.Parse(responseBody);
                    string sentence1 = (string)jsonNode["sentence1"];
                    using StringContent jsonContent = new(
                        JsonSerializer.Serialize(new
                        {
                            answer = sentence1
                        }),
                        Encoding.UTF8,
                        "application/json");
                    var answerResult = await client.PostAsync("https://localhost/api/quiz/answer", jsonContent);

                    return answerResult.IsSuccessStatusCode
                        ? Response.Ok()
                        : Response.Fail();
                });

                var step4 = await Step.Run("logout", context, async () =>
                {
                    var response = await LogOut(client);

                    return response.IsSuccessStatusCode
                    ? Response.Ok()
                    : Response.Fail();
                });

                return Response.Ok();        
            });

            return scenario;
        }

        static ScenarioProps SpamGetJobs()
        {
            var scenario = Scenario.Create("Spam getting all jobs", async context =>
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                HttpClient client = new HttpClient(clientHandler);

                var step1 = await Step.Run("login", context, async () =>
                {
                    var response = await Login(client);
                    return response.IsSuccessStatusCode
                    ? Response.Ok()
                    : Response.Fail();
                });

                var step2 = await Step.Run("get jobs",context, async () =>
                {
                    var response = await client.GetAsync("https://localhost/api/jobs");
                    return response.IsSuccessStatusCode
                        ? Response.Ok()
                        : Response.Fail();
                });

                var step3 = await Step.Run("logout", context, async () =>
                {
                    var response = await LogOut(client);

                    return response.IsSuccessStatusCode
                    ? Response.Ok()
                    : Response.Fail();
                });

                return Response.Ok();        
            });

            return scenario;
        }

        static ScenarioProps SpamGetSavedJobs()
        {
            var scenario = Scenario.Create("Spam getting saved jobs", async context =>
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                HttpClient client = new HttpClient(clientHandler);

                var step1 = await Step.Run("login", context, async () =>
                {
                    var response = await Login(client);
                    return response.IsSuccessStatusCode
                    ? Response.Ok()
                    : Response.Fail();
                });

                var step2 = await Step.Run("get saved jobs",context, async () =>
                {
                    var response = await client.GetAsync("https://localhost/api/jobs/saved");
                    return response.IsSuccessStatusCode
                        ? Response.Ok()
                        : Response.Fail();
                });

                var step3 = await Step.Run("logout", context, async () =>
                {
                    var response = await LogOut(client);

                    return response.IsSuccessStatusCode
                    ? Response.Ok()
                    : Response.Fail();
                });

                return Response.Ok();        
            });

            return scenario;
        }

        static ScenarioProps SpamSaveUnsaveJob()
        {
            var scenario = Scenario.Create("Spam saving and unsaving a job", async context =>
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                HttpClient client = new HttpClient(clientHandler);

                var step1 = await Step.Run("login", context, async () =>
                {
                    var response = await Login(client);
                    return response.IsSuccessStatusCode
                    ? Response.Ok()
                    : Response.Fail();
                });

                var step2 = await Step.Run("save and unsave job",context, async () =>
                {
                    using StringContent jsonContentEmpty = new StringContent("");
                    var response = await client.PostAsync("https://localhost/api/users/save?JobId=1", jsonContentEmpty);
                    var unsaveResponse = await client.PostAsync("https://localhost/api/users/unsave?JobId=1", jsonContentEmpty);
                    return unsaveResponse.IsSuccessStatusCode
                        ? Response.Ok()
                        : Response.Fail();
                });

                var step3 = await Step.Run("logout", context, async () =>
                {
                    var response = await LogOut(client);

                    return response.IsSuccessStatusCode
                    ? Response.Ok()
                    : Response.Fail();
                });

                return Response.Ok();        
            });

            return scenario;
        }
    }
}