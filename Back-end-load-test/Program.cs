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
            var scenarios = new[]
            {
                AccessWebsiteTest(),
                LogInLogOutTest(),
                SpamGenericWord(),
                SpamQuizGame(),
                SpamGetJobs(),
                SpamGetSavedJobs(),
                SpamSaveUnsaveJob(),
                SpamAddDeleteJob(),
                SpamGetComments(),
                SpamUpdateProfile(),
                SpamAddDeleteExperience()
            };

            foreach (var scenario in scenarios)
            {
                NBomberRunner
                    .RegisterScenarios(scenario)
                    .Run();
            }
            
        }  

        static async Task<HttpResponseMessage> Login(HttpClient client, string email = "testuser1@loadtest.com", string password = "password")
        {
            using StringContent jsonContent = new(
            JsonSerializer.Serialize(new
            {
                Email = email,
                Password = password
            }),
            Encoding.UTF8,
            "application/json");

            return await client.PostAsync("https://localhost/api/users/login",jsonContent);
        }

        static HttpClient CreateClient()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            return new HttpClient(clientHandler);
        }
        static async Task<bool> LoginMulti(List<HttpClient> httpClients)
        {
            for (int i = 1; i <= 20; i++)
            {
                var client = CreateClient();
                var response = await Login(client,$"testuser{i}@loadtest.com","password");
                httpClients.Add(client);
                if (!response.IsSuccessStatusCode)
                {
                    return response.IsSuccessStatusCode;
                }
            }
            return true;
        }
        static async Task LogOutMulti(List<HttpClient> httpClients)
        {
            foreach (var client in httpClients)
            {
                var response = await LogOut(client);
            }
        }
        static async Task<HttpResponseMessage> LogOut(HttpClient client)
        {
            using StringContent jsonContent = new StringContent("");
            return await client.PostAsync("https://localhost/api/users/logout", jsonContent);
        }

        static ScenarioProps AccessWebsiteTest()
        {
            List<HttpClient> httpClients = [];

            var scenario = Scenario.Create("Accessing website", async context =>
            {
                int index = context.Random.Next(httpClients.Count);
                var response = await httpClients[index].GetAsync("https://localhost");

                return response.IsSuccessStatusCode
                    ? Response.Ok()
                    : Response.Fail();
            })
            .WithoutWarmUp()
            .WithLoadSimulations(
                Simulation.Inject(rate: 10,
                                  interval: TimeSpan.FromSeconds(1),
                                  during: TimeSpan.FromSeconds(30))
            ).WithInit(async context =>
            {
                if(!await LoginMulti(httpClients))
                {
                    throw new Exception("Could not Initialize accounts");
                }
            })
            .WithClean(async context =>
            {
                await LogOutMulti(httpClients);
            });
            return scenario;
        }

        static ScenarioProps LogInLogOutTest()
        {

            List<HttpClient> httpClients = [];
            var scenario = Scenario.Create("quick Login Logout", async context =>
            {
                int index = context.Random.Next(httpClients.Count);
                var client = httpClients[index];

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
            })
            .WithoutWarmUp()
            .WithLoadSimulations(
                Simulation.Inject(rate: 5,
                                  interval: TimeSpan.FromSeconds(1),
                                  during: TimeSpan.FromSeconds(30))
            ).WithInit(async context =>
            {
                if(!await LoginMulti(httpClients))
                {
                    throw new Exception("Could not Initialize accounts");
                }
            })
            .WithClean(async context =>
            {
                await LogOutMulti(httpClients);
            });

            return scenario;
        }

        static ScenarioProps SpamGenericWord()
        {
            List<HttpClient> httpClients = [];

            var scenario = Scenario.Create("Spam Generic Word", async context =>
            {
                int index = context.Random.Next(httpClients.Count);
                using StringContent jsonContent = new(
                    JsonSerializer.Serialize(new
                    {
                        genericWord = "I am a hardworking and Motivated Individual who has proven to be excellent at getting results"
                    }),
                    Encoding.UTF8,
                    "application/json");

                var response = await httpClients[index].PostAsync("https://localhost/api/genericWord", jsonContent);
                return response.IsSuccessStatusCode
                    ? Response.Ok()
                    : Response.Fail();
            })
            .WithoutWarmUp()
            .WithLoadSimulations(
                Simulation.Inject(rate: 10,
                                  interval: TimeSpan.FromSeconds(1),
                                  during: TimeSpan.FromSeconds(30))
            ).WithInit(async context =>
            {
                if(!await LoginMulti(httpClients))
                {
                    throw new Exception("Could not Initialize accounts");
                }
            })
            .WithClean(async context =>
            {
                await LogOutMulti(httpClients);
            });

            return scenario;
        }

        static ScenarioProps SpamQuizGame()
        {
            List<HttpClient> httpClients = [];

            var scenario = Scenario.Create("Spam Quiz Game", async context =>
            {
                int index = context.ScenarioInfo.InstanceNumber % httpClients.Count;
                var client = httpClients[index];

                var step1 = await Step.Run("initialize quiz game",context, async () =>
                {
                    using StringContent jsonContent = new StringContent("");
                    var response = await client.PostAsync("https://localhost/api/quiz/game", jsonContent);
                    return response.IsSuccessStatusCode
                        ? Response.Ok()
                        : Response.Fail();
                });

                var step2 = await Step.Run("spam quiz game",context, async () =>
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

                return Response.Ok();        
            })
            .WithoutWarmUp()
            .WithLoadSimulations(
                Simulation.KeepConstant(copies: 20, during: TimeSpan.FromSeconds(30))
            ).WithInit(async context =>
            {
                if(!await LoginMulti(httpClients))
                {
                    throw new Exception("Could not Initialize accounts");
                }
            })
            .WithClean(async context =>
            {
                await LogOutMulti(httpClients);
            });

            return scenario;
        }

        static ScenarioProps SpamGetJobs()
        {
            List<HttpClient> httpClients = [];

            var scenario = Scenario.Create("Spam getting all jobs", async context =>
            {
                int index = context.Random.Next(httpClients.Count);
                var client = httpClients[index];

                var response = await client.GetAsync("https://localhost/api/jobs");
                return response.IsSuccessStatusCode
                    ? Response.Ok()
                    : Response.Fail();
            })
            .WithoutWarmUp()
            .WithLoadSimulations(
                Simulation.Inject(rate: 10,
                                  interval: TimeSpan.FromSeconds(1),
                                  during: TimeSpan.FromSeconds(30))
            ).WithInit(async context =>
            {
                if(!await LoginMulti(httpClients))
                {
                    throw new Exception("Could not Initialize accounts");
                }
            })
            .WithClean(async context =>
            {
                await LogOutMulti(httpClients);
            });

            return scenario;
        }

        static ScenarioProps SpamGetSavedJobs()
        {
            List<HttpClient> httpClients = [];

            var scenario = Scenario.Create("Spam getting saved jobs", async context =>
            {
                int index = context.Random.Next(httpClients.Count);
                var client = httpClients[index];

                var response = await client.GetAsync("https://localhost/api/jobs/saved");
                return response.IsSuccessStatusCode
                    ? Response.Ok()
                    : Response.Fail();
  
            })
            .WithoutWarmUp()
            .WithLoadSimulations(
                Simulation.Inject(rate: 10,
                                  interval: TimeSpan.FromSeconds(1),
                                  during: TimeSpan.FromSeconds(30))
            ).WithInit(async context =>
            {
                if(!await LoginMulti(httpClients))
                {
                    throw new Exception("Could not Initialize accounts");
                }
            })
            .WithClean(async context =>
            {
                await LogOutMulti(httpClients);
            });
            return scenario;
        }

        static ScenarioProps SpamSaveUnsaveJob()
        {

            List<HttpClient> httpClients = [];

            var scenario = Scenario.Create("Spam saving and unsaving a job", async context =>
            {
                int index = context.ScenarioInfo.InstanceNumber % httpClients.Count;
                var client = httpClients[index];
                using StringContent jsonContentEmpty = new StringContent("");
                var response = await client.PostAsync("https://localhost/api/users/save?JobId=1", jsonContentEmpty);
                var unsaveResponse = await client.PostAsync("https://localhost/api/users/unsave?JobId=1", jsonContentEmpty);
                return unsaveResponse.IsSuccessStatusCode
                    ? Response.Ok()
                    : Response.Fail();

            })
            .WithoutWarmUp()
            .WithLoadSimulations(
                Simulation.KeepConstant(copies: 20, during: TimeSpan.FromSeconds(30))
            ).WithInit(async context =>
            {
                if(!await LoginMulti(httpClients))
                {
                    throw new Exception("Could not Initialize accounts");
                }
            })
            .WithClean(async context =>
            {
                await LogOutMulti(httpClients);
            });

            return scenario;
        }

        static ScenarioProps SpamAddDeleteExperience()
        {
            List<HttpClient> httpClients = [];

            var scenario = Scenario.Create("Spam add and remove experience", async context =>
            {
                int index = context.ScenarioInfo.InstanceNumber % httpClients.Count;
                var client = httpClients[index];
                
                var step1 = await Step.Run("add experience",context, async () =>
                {
                    using StringContent jsonBody = new(
                        JsonSerializer.Serialize(new
                        {
                            companyName = "Load Testers Ltd.",
                            positionTitle = "Load Tester",
                            jobDescription = "Tested many loads"
                        }),
                        Encoding.UTF8,
                        "application/json");
                    var addResponse = await client.PostAsync("https://localhost/api/users/experiences", jsonBody);

                    return addResponse.IsSuccessStatusCode
                        ? Response.Ok()
                        : Response.Fail();
                });

                var step2 = await Step.Run("delete experience", context, async () =>
                {
                    using StringContent jsonBody = new(
                        JsonSerializer.Serialize(new
                        {
                            companyName = "Load Testers Ltd.",
                            positionTitle = "Load Tester",
                            jobDescription = "Tested many loads"
                        }),
                        Encoding.UTF8,
                        "application/json");
                    HttpRequestMessage deleteRequest = new HttpRequestMessage
                    {
                        Content = jsonBody,
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri("https://localhost/api/users/experiences")
                    };
                    var deleteResponse = await client.SendAsync(deleteRequest);

                    return deleteResponse.IsSuccessStatusCode
                        ? Response.Ok()
                        : Response.Fail();
                });

                return Response.Ok();        
            })
            .WithoutWarmUp()
            .WithLoadSimulations(
                Simulation.KeepConstant(copies: 20, during: TimeSpan.FromSeconds(30))
            ).WithInit(async context =>
            {
                if(!await LoginMulti(httpClients))
                {
                    throw new Exception("Could not Initialize accounts");
                }
            })
            .WithClean(async context =>
            {
                await LogOutMulti(httpClients);
            });

            return scenario;
        }

        static ScenarioProps SpamGetComments()
        {
            List<HttpClient> httpClients = [];

            var scenario = Scenario.Create("Spam getting comments", async context =>
            {
                int index = context.ScenarioInfo.InstanceNumber % httpClients.Count;
                var client = httpClients[index];

                var response = await client.GetAsync("https://localhost/api/comments/1");
                return response.IsSuccessStatusCode
                    ? Response.Ok()
                    : Response.Fail();

            })
            .WithoutWarmUp()
            .WithLoadSimulations(
                Simulation.KeepConstant(copies: 20, during: TimeSpan.FromSeconds(30))
            ).WithInit(async context =>
            {
                if(!await LoginMulti(httpClients))
                {
                    throw new Exception("Could not Initialize accounts");
                }
            })
            .WithClean(async context =>
            {
                await LogOutMulti(httpClients);
            });

            return scenario;
        }

        static ScenarioProps SpamUpdateProfile()
        {
            List<HttpClient> httpClients = [];

            var scenario = Scenario.Create("Spam update profile", async context =>
            {
                int index = context.Random.Next(httpClients.Count);
                var client = httpClients[index];

                var step2 = await Step.Run("update profile",context, async () =>
                {
                    using StringContent updateJson = new(
                        JsonSerializer.Serialize(new
                        {
                            about="blah blah"
                        }),
                        Encoding.UTF8,
                        "application/json");
                    var response = await client.PutAsync("https://localhost/api/users/", updateJson);

                    return response.IsSuccessStatusCode
                        ? Response.Ok()
                        : Response.Fail();
                });

                var step3 = await Step.Run("revert profile", context, async () =>
                {
                    using StringContent revertJson = new (
                        JsonSerializer.Serialize(new
                        {
                            about=""
                        }),
                        Encoding.UTF8,
                        "application/json"
                    );
                    var response = await client.PutAsync("https://localhost/api/users/", revertJson);

                    return response.IsSuccessStatusCode
                        ? Response.Ok()
                        : Response.Fail();
                });

                return Response.Ok(); 
            })
            .WithoutWarmUp()
            .WithLoadSimulations(
                Simulation.Inject(rate: 10,
                                interval: TimeSpan.FromSeconds(1),
                                during: TimeSpan.FromSeconds(30))
            ).WithInit(async context =>
            {
                if(!await LoginMulti(httpClients))
                {
                    throw new Exception("Could not Initialize accounts");
                }
            })
            .WithClean(async context =>
            {
                await LogOutMulti(httpClients);
            });

            return scenario;
        }

        static ScenarioProps SpamAddDeleteJob()
        {
            List<HttpClient> httpClients = [];

            var scenario = Scenario.Create("Spam adding and deleting a job", async context =>
            {
                int index = context.ScenarioInfo.InstanceNumber % httpClients.Count;
                var client = httpClients[index];
                var jobId = -1;

                var step2 = await Step.Run("add job",context, async () =>
                {
                    using StringContent jsonContent = new(
                        JsonSerializer.Serialize(new
                        {
                            title = "Software Developer",
                            deadline = "2026-12-31",
                            applicationLink = "www.test.org",
                            hasRemote = true,
                            hasHybrid = true,
                            positionType = "Full stack",
                            employmentType = "Co-op",
                            locations = new [] {"Winnipeg, MB, Canada"},
                            programmingLanguages = new [] {"Java"},
                            jobDescription = "As a Software Development Intern, you will work closely with the team to support the development and modernization of our backend systems.",
                            employerPoster = false
                        }),
                        Encoding.UTF8,
                        "application/json");
                    var response = await client.PostAsync("https://localhost/api/job/add", jsonContent);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    jobId = (int)JsonNode.Parse(responseBody);
                    return response.IsSuccessStatusCode
                        ? Response.Ok()
                        : Response.Fail();
                });

                var step3 = await Step.Run("delete job",context, async () =>
                {
                    string jobIdForURI = jobId.ToString();
                    var response = await client.DeleteAsync($"https://localhost/api/jobs/{jobIdForURI}");
                    return response.IsSuccessStatusCode
                        ? Response.Ok()
                        : Response.Fail();
                });

                return Response.Ok();        
            })
            .WithoutWarmUp()
            .WithLoadSimulations(
                Simulation.KeepConstant(copies: 20, during: TimeSpan.FromSeconds(30))
            ).WithInit(async context =>
            {
                if(!await LoginMulti(httpClients))
                {
                    throw new Exception("Could not Initialize accounts");
                }
            })
            .WithClean(async context =>
            {
                await LogOutMulti(httpClients);
            });

            return scenario;
        }
    }
}