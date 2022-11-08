using FluentResults;
using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenFTTH.APIGateway.CoreTypes;
using OpenFTTH.APIGateway.GraphQL.Core.Model;
using OpenFTTH.APIGateway.GraphQL.Schematic.Subscriptions;
using OpenFTTH.APIGateway.Settings;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;

namespace OpenFTTH.APIGateway.GraphQL.Outage.Mutations
{
    public class OutageMutations : ObjectGraphType
    {
        private readonly OutageServiceSetting _outageServiceSetting;

        public OutageMutations(IOptions<OutageServiceSetting> outageServiceSetting, ILogger<OutageServiceSetting> logger)
        {
            _outageServiceSetting = outageServiceSetting.Value;

            Description = "Outage / trouble ticket mutations";

            FieldAsync<CommandResultType>(
              "sendTroubleTicket",
              description: "Send trouble ticket information to external systems",
              arguments: new QueryArguments(
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "workTaskId" },
                  new QueryArgument<NonNullGraphType<ListGraphType<StringGraphType>>> { Name = "installationsIds" }
              ),
              resolve: async context =>
              {
                  if (_outageServiceSetting == null || _outageServiceSetting.OutageServiceUrl == null)
                      return new CommandResult(Result.Fail(new Error("OutageServiceSetting config is missing.")));


                  var workTaskId = context.GetArgument<Guid>("workTaskId");

                  if (workTaskId == Guid.Empty)
                      return new CommandResult(Result.Fail(new Error("workTaskId cannot be empty")));


                  var installationsIds = context.GetArgument<List<string>>("installationsIds");

                  if (installationsIds.Count == 0)
                      return new CommandResult(Result.Fail(new Error("installationsIds list cannot be empty")));


                  // Call external service responsible for publishing trouble ticket information to external systems
                  var client = new HttpClient();

                  var troubleTicketRequestJson = JsonConvert.SerializeObject(new TroubleTicketRequest(workTaskId, installationsIds));

                  var content = new StringContent(troubleTicketRequestJson, System.Text.Encoding.UTF8, "application/json");

                  try
                  {
                      HttpResponseMessage result = await client.PostAsync(_outageServiceSetting.OutageServiceUrl, content).ConfigureAwait(false);

                      if (result.StatusCode == System.Net.HttpStatusCode.OK)
                          return new CommandResult(Result.Ok());

                      return new CommandResult(Result.Fail(new Error(result.ReasonPhrase)));
                  }
                  catch (Exception ex)
                  {
                      logger.LogError(ex.Message, ex.StackTrace);
                      return new CommandResult(Result.Fail(new Error(ex.Message)));
                  }
              }
            );
        }

        internal sealed record TroubleTicketRequest
        {
            [JsonPropertyName("workTaskId")]
            public Guid WorkTaskId { get; init; }

            [JsonPropertyName("installationIds")]
            public IEnumerable<string> InstallationIds { get; init; }

            [Newtonsoft.Json.JsonConstructor]
            public TroubleTicketRequest(
                Guid workTaskId,
                IEnumerable<string> installationIds)
            {
                WorkTaskId = workTaskId;
                InstallationIds = installationIds;
            }
        }
    }
}
