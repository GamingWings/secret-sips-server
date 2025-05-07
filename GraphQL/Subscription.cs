using HotChocolate;
using HotChocolate.Subscriptions;

namespace SecretSipsServer.GraphQL;

public class Subscription
{
    [Subscribe]
    [Topic("TestPing")]
    public string OnTestPing([EventMessage] string message) => message;
}
